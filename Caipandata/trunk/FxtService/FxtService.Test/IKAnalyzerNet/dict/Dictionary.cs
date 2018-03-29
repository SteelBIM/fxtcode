using System;
using Wintellect.PowerCollections;
using System.IO;
using System.Reflection;
using System.Text;
namespace IKAnalyzerNet.dict
{
    public class Dictionary
    {
        Assembly assembly;
        private Dictionary()
        {
            assembly = typeof(Dictionary).Assembly;
            hsNoise = null;
            hsStop = null;
            hsOtherDigit = null;
            hsCHNNumber = null;
            hsNbSign = null;
            hsConnector = null;
            dictSeg = null;
            dictSeg = new DictSegment();
            initDictionary();
        }

        public static Dictionary load()
        {
            if (singleton == null)
                singleton = new Dictionary();
            return singleton;
        }

        private void initDictionary()
        {
            //loadNoiseWords();
            //loadStopWords();
            //loadOtherDigit();
            //loadCHNNumber();
            //loadNbSign();
            //loadConnector();
            //loadSuffixWords();
            //loadCountWords();
            loadWords();
            //loadLocalWords();
        }

        public virtual bool isNumber(char onechar)
        {
            return false;// Char.IsDigit(onechar) || isOtherDigit(onechar) || isCHNNumber(onechar);
        }

        public virtual bool isConnector(char onechar)
        {
            return false;// singleton.hsConnector.Contains(Convert.ToString(onechar));
        }

        private void loadConnector()
        {
            Stream is_Renamed;
            is_Renamed = assembly.GetManifestResourceStream(file_Connector);
            hsConnector = new Set<String>();

            try
            {
                using (StreamReader br = new StreamReader(is_Renamed, Encoding.UTF8, false, 512))
                {
                    string theWord;
                    while ((theWord = br.ReadLine()) != null)
                    {
                        hsConnector.Add(theWord.Trim());
                    }
                }
                Console.WriteLine("加载" + file_Connector + ":" + hsConnector.Count);
            }
            catch (Exception e)
            {
                Console.WriteLine("字典文件读取错误！" + file_Connector);
                throw e;
            }

        }

        public virtual bool isNbSign(char onechar)
        {
            return false;// singleton.hsNbSign.Contains(System.Convert.ToString(onechar));
        }

        private void loadNbSign()
        {
            Stream is_Renamed;
            is_Renamed = assembly.GetManifestResourceStream(file_NbSign);
            hsNbSign = new Set<String>();

            try
            {
                using (StreamReader br = new StreamReader(is_Renamed, Encoding.UTF8, false, 512))
                {
                    string theWord;
                    while ((theWord = br.ReadLine()) != null)
                    {
                        hsNbSign.Add(theWord.Trim());
                    }
                }
                Console.WriteLine("加载" + file_NbSign + ":" + hsNbSign.Count);
            }
            catch (Exception e)
            {
                Console.WriteLine("字典文件读取错误！" + file_NbSign);
                throw e;
            }

        }

        public virtual bool isCHNNumber(char onechar)
        {
            return false;// singleton.hsCHNNumber.Contains(System.Convert.ToString(onechar));
        }

        private void loadCHNNumber()
        {
            Stream is_Renamed;
            is_Renamed = assembly.GetManifestResourceStream(file_CHNNumber);
            hsCHNNumber = new Set<String>();
            try
            {
                using (StreamReader br = new StreamReader(is_Renamed, Encoding.UTF8, false, 512))
                {
                    string theWord;
                    while ((theWord = br.ReadLine()) != null)
                    {
                        hsCHNNumber.Add(theWord.Trim());
                    }
                }
                Console.WriteLine("加载" + file_CHNNumber + ":" + hsCHNNumber.Count);
            }
            catch (Exception e)
            {
                Console.WriteLine("字典文件读取错误！" + file_CHNNumber);
                throw e;
            }
        }

        public virtual bool isOtherDigit(char onechar)
        {
            return false;// singleton.hsOtherDigit.Contains(System.Convert.ToString(onechar));
        }

        private void loadOtherDigit()
        {
            Stream is_Renamed;
            is_Renamed = assembly.GetManifestResourceStream(file_OtherDigit);
            hsOtherDigit = new Set<String>();
            try
            {
                using (StreamReader br = new StreamReader(is_Renamed, Encoding.UTF8, false, 512))
                {
                    string theWord;
                    while ((theWord = br.ReadLine()) != null)
                    {
                        hsOtherDigit.Add(theWord.Trim());
                    }

                }
                Console.WriteLine("加载" + file_OtherDigit + ":" + hsOtherDigit.Count);
            }
            catch (Exception e)
            {
                Console.WriteLine("字典文件读取错误！" + file_OtherDigit);
                throw e;
            }
        }

        private void loadCountWords()
        {
            Stream is_Renamed;
            is_Renamed = assembly.GetManifestResourceStream(file_Count);
            try
            {
                int i = 0;
                using (StreamReader br = new StreamReader(is_Renamed, Encoding.UTF8, false, 512))
                {
                    string theWord;
                    while ((theWord = br.ReadLine()) != null)
                    {
                        theWord = theWord.Trim();
                        dictSeg.addWord(theWord.ToCharArray(), WordType.WT_COUNT);
                    }
                }
                Console.WriteLine("加载" + file_Count + ":" + i);

            }
            catch (Exception e)
            {
                Console.WriteLine("字典文件读取错误！" + file_Count);
                throw e;
            }
        }

        private void loadSuffixWords()
        {
            Stream is_Renamed = assembly.GetManifestResourceStream(file_suffix_Word);
            try
            {
                int i = 0;
                using (StreamReader br = new StreamReader(is_Renamed, Encoding.UTF8, false, 512))
                {
                    string theWord;
                    while ((theWord = br.ReadLine()) != null)
                    {
                        theWord = theWord.Trim();
                        dictSeg.addWord(theWord.ToCharArray(), WordType.WT_SUFFIX);
                        i++;
                    }
                }
                Console.WriteLine("加载" + file_suffix_Word + ":" + i);
            }
            catch (Exception e)
            {
                Console.WriteLine("字典文件读取错误！" + file_suffix_Word);
                throw e;
            }

        }

        public virtual bool isUselessWord(string word)
        {
            return false;// isStopWord(word) || isNoiseWord(word);
        }

        public virtual bool isStopWord(string word)
        {
            return false;// singleton.hsStop.Contains(word);
        }

        private void loadStopWords()
        {
            Stream is_Renamed;
            is_Renamed = assembly.GetManifestResourceStream(file_stop_Word);
            hsStop = new Set<String>();
            try
            {
                using (StreamReader br = new StreamReader(is_Renamed, Encoding.UTF8, false, 512))
                {
                    string theWord;
                    while ((theWord = br.ReadLine()) != null)
                    {
                        hsStop.Add(theWord.Trim());
                    }
                }
                Console.WriteLine("加载" + file_stop_Word + ":" + hsStop.Count);
            }
            catch (Exception e)
            {
                Console.WriteLine("字典文件读取错误！" + file_stop_Word);
                throw e;
            }
        }

        public virtual bool isNoiseWord(string word)
        {
            return false;// singleton.hsNoise.Contains(word);
        }

        private void loadNoiseWords()
        {
            Stream is_Renamed = assembly.GetManifestResourceStream(file_noise_char);
            hsNoise = new Set<String>();
            try
            {
                using (StreamReader br = new StreamReader(is_Renamed, Encoding.UTF8, false, 512))
                {
                    string theWord;
                    while ((theWord = br.ReadLine()) != null)
                    {
                        hsNoise.Add(theWord.Trim());
                    }
                }
                Console.WriteLine("加载" + file_noise_char + ":" + hsNoise.Count);

            }
            catch (Exception e)
            {
                Console.WriteLine("字典文件读取错误！" + file_noise_char);
                throw e;
            }
        }

        public virtual Hit search(char[] seg)
        {
            return singleton.dictSeg.search(seg, 0, seg.Length - 1);
        }

        public virtual Hit search(char[] seg, int beginIndex, int endIndex)
        {
            return singleton.dictSeg.search(seg, beginIndex, endIndex);
        }

        public static void loadExtendWords(System.Collections.IList wordList)
        {
            string theWord = null;
            for (int i = 0; i < wordList.Count; i++)
            {
                theWord = ((string)wordList[i]);
                load().dictSeg.addWord(theWord.ToCharArray(), WordType.WT_NORMWORD);
            }
        }

        public static void loadExtendWords(string theWord)
        {
            load().dictSeg.addWord(theWord.ToCharArray(), WordType.WT_NORMWORD);
        }

        private void loadWords()
        {
            Stream is_Renamed = assembly.GetManifestResourceStream(file_Word);
            try
            {
                int i = 0;
                using (StreamReader br = new StreamReader(is_Renamed, Encoding.UTF8, false, 512))
                {
                    string theWord;
                    while ((theWord = br.ReadLine()) != null)
                    {
                        theWord = theWord.Trim();
                        dictSeg.addWord(theWord.ToCharArray(), WordType.WT_NORMWORD);
                        i++;
                    }
                }
                Console.WriteLine("加载" + file_Word + ":" + i);
            }
            catch (Exception e)
            {
                Console.WriteLine("字典文件读取错误！" + file_Word);
                throw e;
            }

        }

        private void loadLocalWords()
        {
            Stream is_Renamed = assembly.GetManifestResourceStream(file_local_Word);
            try
            {
                int i = 0;
                using (StreamReader br = new StreamReader(is_Renamed, Encoding.UTF8, false, 512))
                {
                    string theWord;
                    while ((theWord = br.ReadLine()) != null)
                    {
                        theWord = theWord.Trim();
                        dictSeg.addWord(theWord.ToCharArray(), WordType.WT_NORMWORD);
                        i++;
                    }
                }

                Console.WriteLine("加载" + file_local_Word + ":" + i);
            }
            catch (Exception e)
            {
                Console.WriteLine("字典文件读取错误！" + file_local_Word);
                throw e;
            }

        }



        /// <summary>
        /// 区分字符类型
        /// </summary>
        /// <param name="onechar">The onechar.</param>
        /// <returns></returns>
        /// 时间：2008-2-15 9:37
        /// 作者：dyx
        public virtual int identify(char onechar)
        {
            int charType = BASECHARTYPE_NULL;
            if (onechar >= 'a' && onechar <= 'z' || onechar >= 'A' && onechar <= 'Z')
                charType |= BASECHARTYPE_LETTER;//英文
            if (isNumber(onechar))
            {
                charType |= BASECHARTYPE_NUMBER;//数字
                if (isCHNNumber(onechar))
                    charType |= NUMBER_CHN;//中文数字
                if (isOtherDigit(onechar))
                    charType |= NUMBER_OTHER;//其他数字，如罗马数字
            }
            if (onechar >= '\u4e00' && onechar <= '\u9fff')
                charType |= BASECHARTYPE_CJK;//中文
            if (charType == BASECHARTYPE_NULL)
            {
                charType |= BASECHARTYPE_OTHER;
                if (isNbSign(onechar))
                    charType |= OTHER_NUMSIGN;//数学运算符
                if (isConnector(onechar))
                    charType |= OTHER_CONNECTOR;//链接字符
            }
            return charType;
        }

        public static bool IsChineseLetter(char onechar)
        {
            return onechar >= '\u4e00' && onechar <= '\u9fff';
        }



        public const int BASECHARTYPE_NULL = 0;
        public const int BASECHARTYPE_LETTER = 0x10000;
        public const int BASECHARTYPE_NUMBER = 0x100000;
        public const int NUMBER_CHN = 1;
        public const int NUMBER_OTHER = 2;
        public const int BASECHARTYPE_CJK = 0x1000000;
        public const int BASECHARTYPE_OTHER = 0x10000000;
        public const int OTHER_CONNECTOR = 4;
        public const int OTHER_NUMSIGN = 8;
        private static Dictionary singleton = null;
        private static string file_Word = "FxtService.Test.dict.wordbase.dic";
        private static string file_stop_Word = "FxtService.Test.dict.stopword.dic";
        private static string file_suffix_Word = "FxtService.Test.dict.suffix.dic";
        private static string file_local_Word = "FxtService.Test.dict.local.dic";
        private static string file_OtherDigit = "FxtService.Test.dict.other_digit.dic";
        private static string file_CHNNumber = "FxtService.Test.dict.c_number.dic";
        private static string file_NbSign = "FxtService.Test.dict.number_sign.dic";
        private static string file_Connector = "FxtService.Test.dict.connector.dic";
        private static string file_Count = "FxtService.Test.dict.count.dic";
        private static string file_noise_char = "FxtService.Test.dict.noisechar.dic";

        //private static string file_Word = "wordbase.dic";
        //private static string file_stop_Word = "stopword.dic";
        //private static string file_suffix_Word = "suffix.dic";
        //private static string file_local_Word = "local.dic";
        //private static string file_OtherDigit = "other_digit.dic";
        //private static string file_CHNNumber = "c_number.dic";
        //private static string file_NbSign = "number_sign.dic";
        //private static string file_Connector = "connector.dic";
        //private static string file_Count = "count.dic";
        //private static string file_noise_char = "noisechar.dic";



        private Set<String> hsNoise;
        private Set<String> hsStop;
        private Set<String> hsOtherDigit;
        private Set<String> hsCHNNumber;
        private Set<String> hsNbSign;
        private Set<String> hsConnector;
        private DictSegment dictSeg;
    }
}