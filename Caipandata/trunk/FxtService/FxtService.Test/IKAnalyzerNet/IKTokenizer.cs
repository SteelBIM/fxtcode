using System;
using IKAnalyzerNet.dict;
using Lucene.Net.Analysis;
using Wintellect.PowerCollections;
using System.IO;
using System.Collections;
namespace IKAnalyzerNet
{
    public sealed class IKTokenizer : Tokenizer
    {
        private int LastMatchEnd
        {
            set
            {
                if (lastMatchEnd < value)
                    lastMatchEnd = value;
            }

        }

        internal IKTokenizer(TextReader in_Renamed, bool mircoSupported)
            : base(in_Renamed)
        {
            dict = Dictionary.load();
            lastHitState = 0;
            segmentBuff = new char[2048];
            this.mircoSupported = mircoSupported;
            tokens = new ExtendOrderedSet<TokenDelegate>();
            numberSet = new ExtendOrderedSet<TokenDelegate>();
            initContext();
        }

        private void initContext()
        {
            inputStatus = 0;
            contextStatus = 0;
            beginIndex = 0;
            lastMatchEnd = -1;
            numberBeginIndex = -1;
            numberStatus = 0;
            unmatchFlag = false;
            unmatchBegin = -1;
            breakAndRead = false;
            bAr_offset = 0;
        }

        public override Token Next()
        {
            if (!(tokens.Count == 0))
            {
                TokenDelegate td = tokens.GetFirst();
                tokens.Remove(td);
                return td.Token;
            }
            int segLength = 0;
            if (breakAndRead)
            {
                segLength = input.Read(segmentBuff, 2048 - bAr_offset, bAr_offset);
                segLength += 2048 - bAr_offset;
            }
            else
            {
                segLength = input.Read(segmentBuff, 0, segmentBuff.Length);
            }
            if (segLength <= 0)
                return null;
            getTokens(segLength);
            offset += bAr_offset;
            if (!(tokens.Count == 0))
            {

                TokenDelegate td = tokens.GetFirst();
                tokens.Remove(td);
                return td.Token;
            }
            else
            {
                return null;
            }
        }

        private void getTokens(int segLength)
        {
            initContext();
            char current = '\x0000';
            for (int ci = 0; ci < segLength; ci++)
            {
                nextContextStatus = 0;
                nextNumberStatus = 0;
                segmentBuff[ci] = toDBC(segmentBuff[ci]);//全角转半角
                current = segmentBuff[ci];
                inputStatus = dict.identify(current);
                if (contextStatus == 0)
                {
                    procInitState(ci);
                }
                else
                {
                    if ((contextStatus & 1) > 0)
                        ci = procLetterState(ci);
                    if ((contextStatus & 0x10) > 0)
                        ci = procNumberState(ci);
                    if ((contextStatus & 0x100) > 0)
                        ci = procCJKState(ci);
                }
                contextStatus = nextContextStatus;
                numberStatus = nextNumberStatus;
                if (nextContextStatus != 0 || segLength - ci <= 1 || segLength - ci >= 50 || segLength != 2048)
                    continue;
                bAr_offset = ci + 1;
                breakAndRead = true;
                break;
            }

            if (!breakAndRead)
            {
                if (numberBeginIndex >= 0)
                    pushNumber(numberBeginIndex, segLength - 1);
                if (lastMatchEnd != segLength - 1)
                    if (unmatchFlag)
                        procSplitSeg(unmatchBegin, segLength - 1);
                    else if (beginIndex < segLength)
                        procSplitSeg(beginIndex, segLength - 1);
            }
            for (IEnumerator it = numberSet.GetEnumerator(); it.MoveNext(); pushTerm((TokenDelegate)it.Current))
                ;
            numberSet.Clear();
            if (!mircoSupported)
            {
                ExtendOrderedSet<TokenDelegate> tmpTokens = new ExtendOrderedSet<TokenDelegate>();
                for (System.Collections.IEnumerator it = tokens.GetEnumerator(); it.MoveNext(); )
                {
                    TokenDelegate td = (TokenDelegate)it.Current;
                    MTokenDelegate mtd = new MTokenDelegate(td.Offset, td.Begin, td.End);
                    String w = (new String(segmentBuff, td.Begin, (td.End - td.Begin) + 1)).ToLower();
                    if (!dict.isUselessWord(w))
                    {
                        mtd.Term = w;
                        tmpTokens.Add(mtd);
                    }
                }

                tokens.Clear();
                tokens = tmpTokens;
            }
            if (breakAndRead)
                Array.Copy(segmentBuff, bAr_offset, segmentBuff, 0, segLength - bAr_offset);
        }

        private void procInitState(int ci)
        {
            if ((inputStatus & Dictionary.BASECHARTYPE_LETTER) > 0)//英文
            {
                nextContextStatus |= 1;
                beginIndex = ci;
            }
            if ((inputStatus & Dictionary.BASECHARTYPE_NUMBER) > 0)
            {
                nextContextStatus |= 0x10;
                if ((inputStatus & Dictionary.NUMBER_CHN) > 0)
                    nextNumberStatus |= 1;
                if ((inputStatus & Dictionary.NUMBER_OTHER) > 0)
                    nextNumberStatus |= 2;
                numberBeginIndex = ci;
            }
            if ((inputStatus & Dictionary.BASECHARTYPE_CJK) > 0)
            {
                nextContextStatus |= 0x100;
                beginIndex = ci;
                procCJK(beginIndex, ci);
            }
            if ((inputStatus & Dictionary.BASECHARTYPE_OTHER) > 0)
                beginIndex = ci + 1;
        }

        private int procLetterState(int ci)
        {
            if ((inputStatus & Dictionary.BASECHARTYPE_LETTER) > 0)
            {
                nextContextStatus |= 1;
                return ci;
            }
            if ((inputStatus & Dictionary.BASECHARTYPE_NUMBER) > 0)
            {
                pushTerm(beginIndex, ci - 1, true);
                LastMatchEnd = ci - 1;
                nextContextStatus |= 0x10;
                if ((inputStatus & 1) > 0)
                    nextNumberStatus = 1;
                if ((inputStatus & 2) > 0)
                    nextNumberStatus = 2;
                numberBeginIndex = ci;
            }
            if ((inputStatus & Dictionary.BASECHARTYPE_CJK) > 0)
            {
                pushTerm(beginIndex, ci - 1, true);
                LastMatchEnd = ci - 1;
                nextContextStatus |= 0x100;
                beginIndex = ci;
                procCJK(beginIndex, ci);
            }
            if ((inputStatus & Dictionary.BASECHARTYPE_OTHER) > 0)
            {
                pushTerm(beginIndex, ci - 1, true);
                LastMatchEnd = ci - 1;
                nextContextStatus = 0;
                beginIndex = ci + 1;
            }
            return ci;
        }

        private int procNumberState(int ci)
        {
            if (numberBeginIndex > 0 && ci <= numberBeginIndex)
                return ci;
            if ((inputStatus & Dictionary.BASECHARTYPE_LETTER) > 0)
            {
                nextContextStatus |= 1;
                if ((inputStatus & Dictionary.BASECHARTYPE_NUMBER) > 0 && (contextStatus & 1) > 0)
                {
                    nextContextStatus |= 0x10;
                    nextNumberStatus = 2;
                }
                else if ((contextStatus & 1) > 0)
                {
                    nextContextStatus = 1;
                    nextNumberStatus = 0;
                    numberBeginIndex = -1;
                }
                else
                {
                    pushTerm(numberBeginIndex, ci - 1);
                    if (unmatchFlag)
                    {
                        pushTerm(unmatchBegin, numberBeginIndex - 1);
                        LastMatchEnd = ci - 1;
                        unmatchFlag = false;
                    }
                    else if (beginIndex < numberBeginIndex)
                    {
                        pushTerm(beginIndex, numberBeginIndex - 1);
                        LastMatchEnd = ci - 1;
                    }
                    beginIndex = ci;
                    numberBeginIndex = -1;
                }
                return ci;
            }
            if ((inputStatus & Dictionary.BASECHARTYPE_NUMBER) > 0)
            {
                nextContextStatus |= 0x10;
                if ((inputStatus & 1) > 0)
                {
                    nextNumberStatus = 1;
                    if (numberStatus == 0)
                    {
                        pushTerm(numberBeginIndex, ci - 1, true);
                        LastMatchEnd = ci - 1;
                        beginIndex = ci;
                    }
                }
                else if ((inputStatus & 2) > 0)
                {
                    nextNumberStatus = 2;
                    if (numberStatus == 0)
                    {
                        pushTerm(numberBeginIndex, ci - 1, true);
                        LastMatchEnd = ci - 1;
                        beginIndex = ci;
                    }
                }
                else if (inputStatus == Dictionary.BASECHARTYPE_NUMBER)
                    nextNumberStatus = 0;
                if ((inputStatus & Dictionary.BASECHARTYPE_CJK) > 0)
                    nextContextStatus |= 0x100;
                return ci;
            }
            if ((inputStatus & Dictionary.BASECHARTYPE_CJK) > 0)
            {
                nextContextStatus |= 0x100;
                LastMatchEnd = ci - 1;
                pushNumber(numberBeginIndex, ci - 1);
                if ((contextStatus & 0x100) == 0)
                    beginIndex = ci;
                procCJK(ci, ci);
                numberBeginIndex = -1;
                return ci;
            }
            if ((inputStatus & Dictionary.BASECHARTYPE_OTHER) > 0)
                if ((inputStatus & 8) > 0 && numberStatus == 0)
                {
                    nextContextStatus |= 0x10;
                    numberStatus = 8;
                }
                else
                {
                    nextContextStatus = 0;
                    pushTerm(numberBeginIndex, ci - 1, true);
                    if ((contextStatus & 0x100) > 0)
                        if (unmatchFlag)
                        {
                            pushTerm(unmatchBegin, numberBeginIndex - 1);
                            LastMatchEnd = ci - 1;
                            unmatchFlag = false;
                        }
                        else
                        {
                            pushTerm(beginIndex, numberBeginIndex - 1);
                            LastMatchEnd = ci - 1;
                        }
                    LastMatchEnd = ci - 1;
                    numberBeginIndex = -1;
                    beginIndex = ci + 1;
                }
            return ci;
        }

        private int procCJKState(int ci)
        {
            if ((inputStatus & Dictionary.BASECHARTYPE_LETTER) > 0)
            {
                nextContextStatus |= 1;
                if ((inputStatus & Dictionary.BASECHARTYPE_NUMBER) > 0)
                {
                    nextContextStatus |= 0x10;
                    nextNumberStatus = 2;
                    if (numberBeginIndex < 0)
                        numberBeginIndex = ci;
                }
                if (lastMatchEnd != ci - 1)
                    if (unmatchFlag)
                    {
                        procSplitSeg(unmatchBegin, ci - 1);
                        LastMatchEnd = ci - 1;
                        unmatchFlag = false;
                    }
                    else
                    {
                        procSplitSeg(beginIndex, ci - 1);
                        LastMatchEnd = ci - 1;
                    }
                beginIndex = ci;
                return ci;
            }
            if ((inputStatus & Dictionary.BASECHARTYPE_NUMBER) > 0)
            {
                nextContextStatus |= 0x10;
                if (numberBeginIndex < 0)
                    numberBeginIndex = ci;
                if ((inputStatus & Dictionary.BASECHARTYPE_CJK) > 0)
                {
                    if ((inputStatus & 1) > 0)
                        nextNumberStatus = 1;
                    else if ((inputStatus & 2) > 0)
                        nextNumberStatus = 2;
                }
                else
                {
                    if ((inputStatus & 2) > 0)
                        nextNumberStatus = 2;
                    else
                        nextNumberStatus = 0;
                    if (lastMatchEnd != ci - 1)
                        if (unmatchFlag)
                        {
                            procSplitSeg(unmatchBegin, ci - 1);
                            LastMatchEnd = ci - 1;
                            unmatchFlag = false;
                        }
                        else
                        {
                            procSplitSeg(beginIndex, ci - 1);
                            LastMatchEnd = ci - 1;
                        }
                    beginIndex = ci;
                    return ci;
                }
            }
            if ((inputStatus & Dictionary.BASECHARTYPE_CJK) > 0)
            {
                nextContextStatus |= 0x100;
                int move = procCJK(beginIndex, ci);
                ci += move;
                return ci;
            }
            if ((inputStatus & Dictionary.BASECHARTYPE_OTHER) > 0)
            {
                nextContextStatus = 0;
                outputNunmber();
                if (unmatchFlag)
                {
                    procSplitSeg(unmatchBegin, ci - 1);
                    LastMatchEnd = ci - 1;
                    unmatchFlag = false;
                }
                else if (beginIndex < ci)
                {
                    procSplitSeg(beginIndex, ci - 1);
                    LastMatchEnd = ci - 1;
                }
                beginIndex = ci + 1;
                return ci;
            }
            else
            {
                return ci;
            }
        }

        private int procCJK(int begin, int end)
        {
            Hit hit = dict.search(segmentBuff, begin, end);
            if (hit.MatchAndContinue)
            {
                if (unmatchFlag)
                {
                    if (hit.WordType.Suffix)
                        pushTerm(unmatchBegin, end);
                    else if (unmatchBegin == begin - 1 && !dict.isNoiseWord(Convert.ToString(segmentBuff[unmatchBegin])))
                    {
                        pushTerm(unmatchBegin, begin - 1, true);
                        pushTerm(unmatchBegin, begin, true);
                    }
                    else
                    {
                        cjkCut(unmatchBegin, begin - 1);
                        pushTerm(unmatchBegin, begin - 1);
                    }
                    LastMatchEnd = end;
                    pushTerm(begin, end);
                    unmatchFlag = false;
                }
                else if (hit.WordType.NormWord)
                {
                    LastMatchEnd = end;
                    pushTerm(begin, end);
                }
                if (!(numberSet.Count == 0))
                {
                    for (IEnumerator it = numberSet.GetEnumerator(); it.MoveNext(); pushTerm((TokenDelegate)it.Current))
                        ;
                    if (hit.WordType.Count)
                    {
                        TokenDelegate number = numberSet.GetLast();
                        pushTerm(number.Begin, end, true);
                        LastMatchEnd = end;
                    }
                    else
                    {
                        numberSet.Clear();
                    }
                }
                lastHitState = 1;
                return 0;
            }
            if (hit.isMatch())
            {
                if (unmatchFlag)
                {
                    if (hit.WordType.Suffix)
                        pushTerm(unmatchBegin, end);
                    else if (unmatchBegin == begin - 1 && !dict.isNoiseWord(Convert.ToString(segmentBuff[unmatchBegin])))
                    {
                        pushTerm(unmatchBegin, begin - 1, true);
                        pushTerm(unmatchBegin, begin, true);
                    }
                    else
                    {
                        cjkCut(unmatchBegin, begin - 1);
                        pushTerm(unmatchBegin, begin - 1);
                    }
                    LastMatchEnd = end;
                    pushTerm(begin, end);
                    unmatchFlag = false;
                }
                else if (hit.WordType.NormWord)
                {
                    LastMatchEnd = end;
                    pushTerm(begin, end);
                }
                else
                {
                    unmatchFlag = true;
                    unmatchBegin = begin;
                }
                if (!(numberSet.Count == 0))
                {

                    for (IEnumerator it = numberSet.GetEnumerator(); it.MoveNext(); pushTerm((TokenDelegate)it.Current))
                        ;
                    if (hit.WordType.Count)
                    {
                        TokenDelegate number = numberSet.GetLast();
                        pushTerm(number.Begin, end, true);
                        LastMatchEnd = end;
                        unmatchFlag = false;
                    }
                    numberSet.Clear();
                }
                beginIndex++;
                lastHitState = 2;
                return beginIndex - end;
            }
            if (hit.isPrefixMatch())
            {
                lastHitState = 3;
                return 0;
            }
            if (hit.isUnmatch())
            {
                if (lastHitState == 3 && unmatchFlag)
                    if (unmatchBegin == begin - 1 && !dict.isNoiseWord(Convert.ToString(segmentBuff[unmatchBegin])))
                    {
                        pushTerm(unmatchBegin, begin - 1, true);
                        pushTerm(unmatchBegin, begin, true);
                    }
                    else
                    {
                        cjkCut(unmatchBegin, begin - 1);
                        pushTerm(unmatchBegin, begin - 1);
                    }
                if (begin > lastMatchEnd)
                {
                    if (!unmatchFlag)
                    {
                        unmatchFlag = true;
                        unmatchBegin = begin;
                    }
                    Hit h = dict.search(segmentBuff, end, end);
                    if (dict.isNoiseWord(Convert.ToString(segmentBuff[end])) && end - begin == 1)
                    {
                        cjkCut(unmatchBegin, begin);
                        pushTerm(unmatchBegin, begin);
                        unmatchFlag = false;
                        LastMatchEnd = begin;
                    }
                    else if (h.isMatch() && h.WordType.Suffix)
                    {
                        if (unmatchBegin < end)
                        {
                            pushTerm(unmatchBegin, end - 1);
                            pushTerm(unmatchBegin, end);
                            cjkCut(unmatchBegin, end);
                            unmatchFlag = false;
                            LastMatchEnd = end;
                            beginIndex = end;
                            return 0;
                        }
                    }
                    else
                    {
                        h = dict.search(segmentBuff, begin, begin);
                        if (h.isMatch() && h.WordType.Suffix && unmatchBegin != begin)
                        {
                            pushTerm(unmatchBegin, begin);
                            unmatchFlag = false;
                            LastMatchEnd = begin;
                        }
                    }
                }
                beginIndex++;
                lastHitState = 4;
                return beginIndex - end;
            }
            else
            {
                return 0;
            }
        }

        private void procSplitSeg(int begin, int end)
        {
            unmatchFlag = false;
            pushTerm(begin, end);
            int localMatch = begin - 1;
            bool localUnmatch = false;
            int localUnmatchBegin = begin - 1;
            for (int i = begin; i <= end; i++)
            {
                for (int j = i; j <= end; j++)
                {
                    Hit hit = dict.search(segmentBuff, i, j);
                    if (hit.MatchAndContinue)
                    {
                        if (localUnmatch)
                        {
                            pushTerm(localUnmatchBegin, i - 1);
                            localUnmatch = false;
                        }
                        pushTerm(i, j);
                        localMatch = j;
                        continue;
                    }
                    if (hit.isMatch())
                    {
                        if (localUnmatch)
                        {
                            pushTerm(localUnmatchBegin, i - 1);
                            localUnmatch = false;
                        }
                        pushTerm(i, j);
                        localMatch = j;
                        break;
                    }
                    if (hit.isPrefixMatch() || !hit.isUnmatch())
                        continue;
                    if (!localUnmatch)
                    {
                        localUnmatch = true;
                        localUnmatchBegin = i;
                    }
                    break;
                }
            }

            if (localMatch != end)
            {
                pushTerm(localMatch + 1, end);
                cjkCut(localMatch + 1, end);
            }
        }

        private void pushTerm(int begin, int end, bool directOutput)
        {
            if (begin > end)
                return;
            if (!directOutput)
            {
                Hit h = dict.search(segmentBuff, begin, end);
                if (!h.isMatch() && begin != end && dict.isNoiseWord(Convert.ToString(segmentBuff[begin])))
                    begin++;
                if (dict.isNbSign(segmentBuff[end]) || dict.isConnector(segmentBuff[end]))
                    end--;
            }
            TokenDelegate td = new TokenDelegate(offset, begin, end);
            if (mircoSupported)
            {
                String w = (new String(segmentBuff, begin, (end - begin) + 1)).ToLower();
                if (!dict.isUselessWord(w))
                {
                    td.Term = w;
                    tokens.Add(td);
                }
            }
            else
            {
                tokens.Add(td);
            }
        }

        private void pushTerm(int begin, int end)
        {
            pushTerm(begin, end, false);
        }

        private void pushTerm(TokenDelegate td)
        {
            if (mircoSupported)
            {
                String w = new String(segmentBuff, td.Begin, (td.End - td.Begin) + 1);
                if (!dict.isUselessWord(w))
                {
                    td.Term = w;
                    tokens.Add(td);
                }
            }
            else
            {
                tokens.Add(td);
            }
        }

        private void pushNumber(int begin, int end)
        {
            if (dict.isNbSign(segmentBuff[end]) || dict.isConnector(segmentBuff[end]))
            {
                pushTerm(begin, end - 1);
                return;
            }
            else
            {
                TokenDelegate td = new TokenDelegate(offset, begin, end);
                numberSet.Add(td);
                return;
            }
        }

        private void outputNunmber()
        {
            for (IEnumerator it = numberSet.GetEnumerator(); it.MoveNext(); pushTerm((TokenDelegate)it.Current))
                ;
            numberSet.Clear();
        }

        private void cjkCut(int begin, int end)
        {
            if (end - begin <= 1)
                return;
            for (int i = begin; i < end; i++)
                pushTerm(i, i + 1, true);
        }

        public char toDBC(char input)
        {
            if (input == '\u3000')
                input = ' ';
            if (input > '\uFF00' && input < '\uFF5F')
                input -= '\uFEE0';
            return input;
        }

        private Dictionary dict;
        private char[] segmentBuff;
        private bool mircoSupported;
        private int offset;
        private ExtendOrderedSet<TokenDelegate> tokens;
        private ExtendOrderedSet<TokenDelegate> numberSet;
        private int contextStatus;
        private int nextContextStatus;
        private int inputStatus;
        private int beginIndex;
        private int lastMatchEnd;
        private int numberBeginIndex;
        private int numberStatus;
        private int nextNumberStatus;
        private bool unmatchFlag;
        private int unmatchBegin;
        private bool breakAndRead;
        private int bAr_offset;
        internal int lastHitState;

    }

    internal class ExtendOrderedSet<T> : OrderedSet<T>
    {

        public new bool Add(T term)
        {
            if (base.Contains(term))
            {
                return true;
            }
            else
            {
                return base.Add(term);
            }
        }
    }

}