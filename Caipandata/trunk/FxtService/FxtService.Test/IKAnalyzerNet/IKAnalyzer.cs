using System;
using Lucene.Net.Analysis;
namespace IKAnalyzerNet
{
    /// <summary>
    /// 全分词器
    /// </summary>
	public sealed class IKAnalyzer:Analyzer
	{
		public IKAnalyzer()
		{
			mircoSupported = true;
		}
		
		private bool mircoSupported;

        public override TokenStream TokenStream(string fieldName, System.IO.TextReader reader)
        {
            return new IKTokenizer(reader, mircoSupported);
        }
    }
}