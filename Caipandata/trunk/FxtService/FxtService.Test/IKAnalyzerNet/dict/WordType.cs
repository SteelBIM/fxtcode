using System;
namespace IKAnalyzerNet.dict
{
	
	
	public class WordType
	{
		virtual public bool NormWord
		{
			get
			{
				return (wordTypeValue & 1) > 0;
			}
			
		}
		virtual public bool Suffix
		{
			get
			{
				return (wordTypeValue & 0x10) > 0;
			}
			
		}
		virtual public bool Count
		{
			get
			{
				return (wordTypeValue & 0x100) > 0;
			}
			
		}
		
		public WordType()
		{
			wordTypeValue = 0;
		}
		
		private WordType(int wordTypeValue)
		{
			this.wordTypeValue = 0;
			this.wordTypeValue = wordTypeValue;
		}
		
		public virtual void  addWordType(WordType wordType)
		{
			wordTypeValue = wordTypeValue | wordType.wordTypeValue;
		}
		
		private int wordTypeValue;
		public static readonly WordType WT_NORMWORD = new WordType(1);
		public static readonly WordType WT_SUFFIX = new WordType(16);
		public static readonly WordType WT_COUNT = new WordType(256);
	}
}