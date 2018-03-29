using System;
using Lucene.Net.Analysis;
namespace IKAnalyzerNet
{

    /// <summary>
    /// 最大匹配分词器
    /// </summary>
	public class MIKAnalyzer:Analyzer
	{
		
		public MIKAnalyzer()
		{
			mircoSupported = false;
		}
		
		private bool mircoSupported;

        public override TokenStream TokenStream(string fieldName, System.IO.TextReader reader)
        {
            return new IKTokenizer(reader, mircoSupported);
        }
    }
}