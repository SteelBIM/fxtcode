using System;
using Lucene.Net.Analysis;
namespace IKAnalyzerNet
{
	
	public class TokenDelegate : IComparable
	{
		virtual public String Term
		{
			set
			{
				token = new Token(value, offset + begin, offset + end + 1);
			}
			
		}
		virtual public int Begin
		{
			get
			{
				return begin;
			}
			
		}
		virtual public int End
		{
			get
			{
				return end;
			}
			
		}
		virtual public Token Token
		{
			get
			{
				return token;
			}
			
		}
		virtual public int Offset
		{
			get
			{
				return offset;
			}
			
		}
		
		internal TokenDelegate(int offset, int begin, int end)
		{
			this.offset = offset;
			this.begin = begin;
			this.end = end;
		}
		
		public  override bool Equals(System.Object o)
		{
			if (o is TokenDelegate)
			{
				TokenDelegate ntd = (TokenDelegate) o;
				if (begin == ntd.Begin && end == ntd.End)
					return true;
			}
			return false;
		}
		
		public override int GetHashCode()
		{
			return begin * 17 + end * 23;
		}
		
		public virtual int CompareTo(System.Object o)
		{
			TokenDelegate ntd = (TokenDelegate) o;
			if (begin < ntd.begin)
				return - 1;
			if (begin == ntd.begin)
			{
				if (end > ntd.end)
					return -1;
				if (end == ntd.end)
					return 0;
				if (end < ntd.end)
					return 1;
			}
			return 1;
		}
		
		private int offset;
		private int begin;
		private int end;
		private Token token;
	}
}