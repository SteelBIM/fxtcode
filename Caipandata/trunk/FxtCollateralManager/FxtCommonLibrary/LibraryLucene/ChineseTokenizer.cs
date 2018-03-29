using System;
using System.Collections.Generic;
using System.Text;
using Lucene.Net.Analysis;
using System.Collections;
using System.Text.RegularExpressions;
using System.IO;

namespace FxtCommonLibrary.LibraryLucene
{
    class ChineseTokenizer : Tokenizer
    {

        private int offset = 0, bufferIndex = 0, dataLen = 0;//偏移量，当前字符的位置，字符长度

        private int start;//开始位置
        /// <summary>
        /// 存在字符内容
        /// </summary>
        private string text;

        /// <summary>
        /// 切词所花费的时间
        /// </summary>
        public double TextSeg_Span = 0;

        /// <summary>Constructs a tokenizer for this Reader. </summary>
        public ChineseTokenizer(System.IO.TextReader reader)
        {
            this.input = reader;
            text = input.ReadToEnd();
            dataLen = text.Length;
        }

        /// <summary>进行切词，返回数据流中下一个token或者数据流为空时返回null
        /// </summary>
        /// 
        public override Token Next()
        {
            Token token = null;
            WordTree tree = new WordTree();
            //读取词库
            tree.LoadDict();
            //初始化词库，为树形
            Hashtable t_chartable = WordTree.chartable;
            string ReWord = "";
            string char_s;
            start = offset;
            bufferIndex = start;

            while (true)
            {
                //开始位置超过字符长度退出循环
                if (start >= dataLen)
                {
                    break;
                }
                //获取一个词
                char_s = text.Substring(start, 1);
                if (string.IsNullOrEmpty(char_s.Trim()))
                {
                    start++;
                    continue;
                }
                //字符不在字典中
                if (!t_chartable.Contains(char_s))
                {
                    if (ReWord == "")
                    {
                        int j = start + 1;
                        switch (tree.GetCharType(char_s))
                        {
                            case 0://中文单词
                                ReWord += char_s;
                                break;
                            case 1://英文单词
                                j = start + 1;
                                while (j < dataLen)
                                {
                                    if (tree.GetCharType(text.Substring(j, 1)) != 1)
                                        break;

                                    j++;
                                }
                                ReWord += text.Substring(start, j - offset);

                                break;
                            case 2://数字
                                j = start + 1;
                                while (j < dataLen)
                                {
                                    if (tree.GetCharType(text.Substring(j, 1)) != 2)
                                        break;

                                    j++;
                                }
                                ReWord += text.Substring(start, j - offset);

                                break;

                            default:
                                ReWord += char_s;//其他字符单词
                                break;
                        }

                        offset = j;//设置取下一个词的开始位置
                    }
                    else
                    {
                        offset = start;//设置取下一个词的开始位置
                    }

                    //返回token对象
                    return new Token(ReWord, bufferIndex, bufferIndex + ReWord.Length - 1);
                }
                //字符在字典中
                ReWord += char_s;
                //取得属于当前字符的词典树
                t_chartable = (Hashtable)t_chartable[char_s];
                //设置下一循环取下一个词的开始位置
                start++;
                if (start == dataLen)
                {
                    offset = dataLen;
                    return new Token(ReWord, bufferIndex, bufferIndex + ReWord.Length - 1);
                }
            }
            return token;
        }

    }
}
