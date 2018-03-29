using System;
using System.Collections.Generic;
using System.Text;

using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using System.IO;

namespace FxtCommonLibrary.LibraryLucene
{
    /**//// <summary>
    /// 
    /// </summary>
    public class ChineseAnalyzer:Analyzer
    {
        //private System.Collections.Hashtable stopSet;
        public static readonly System.String[] CHINESE_ENGLISH_STOP_WORDS = new System.String[] { "a", "an", "and", "are", "as", "at", "be", "but", "by", "for", "if", "in", "into", "is", "it", "no", "not", "of", "on", "or", "s", "such", "t", "that", "the", "their", "then", "there", "these", "they", "this", "to", "was", "will", "with", "我", "我们" };

      
        /**//// <summary>Constructs a {@link StandardTokenizer} filtered by a {@link
        /// StandardFilter}, a {@link LowerCaseFilter} and a {@link StopFilter}. 
        /// </summary>
        public override TokenStream TokenStream(System.String fieldName, System.IO.TextReader reader)
        {
            TokenStream result = new ChineseTokenizer(reader);
            result = new StandardFilter(result);
            result = new LowerCaseFilter(result);
            result = new StopFilter(result, CHINESE_ENGLISH_STOP_WORDS);
            return result;
        }
        public List<string> GetTokenList(string strVal) {
            string readWord = string.Empty, strToken = string.Empty;
            List<string> list = new List<string>();
            using (TextReader reader = new StringReader(strVal))
            {
                Analyzer analyzer = new ChineseAnalyzer();
                TokenStream stream = analyzer.TokenStream(null, reader);
                int i = 0;
                for (Token t = stream.Next(); t != null; t = stream.Next())
                {
                    strToken = t.ToString();   //显示格式： (关键词,0,2) ，需要处理
                    strToken = strToken.Replace("(", "");
                    char[] separator = { ',' };
                    readWord = strToken.Split(separator)[0];
                    list.Add(readWord);
                    i++;
                }
                stream.Close();
            }
            return list;
        }

    }
}