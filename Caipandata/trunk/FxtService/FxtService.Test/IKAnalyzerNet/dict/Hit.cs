using System;
namespace IKAnalyzerNet.dict
{
	
	public class Hit
	{
		virtual public bool MatchAndContinue
		{
			get
			{
				return (hitState & 1) > 0 && (hitState & 0x10) > 0;
			}
			
		}
		virtual public WordType WordType
		{
			get
			{
				return wordType;
			}
			
			set
			{
				this.wordType = value;
			}
			
		}
		virtual public int HitState
		{
			get
			{
				return hitState;
			}
			
		}
		
		public Hit()
		{
			hitState = 0;
		}
		
		public virtual bool isMatch()
		{
			return (hitState & 1) > 0;
		}
		
		public virtual void  setMatch()
		{
			hitState = hitState | 1;
		}
		
		public virtual bool isPrefixMatch()
		{
			return (hitState & 0x10) > 0;
		}
		
		public virtual void  setPrefixMatch()
		{
			hitState = hitState | 0x10;
		}
		
		public virtual bool isUnmatch()
		{
			return (hitState & 0x100) > 0;
		}
		
		public virtual void  setUnmatch()
		{
			hitState = 256;
		}
		private int hitState;
		private WordType wordType;
	}
}