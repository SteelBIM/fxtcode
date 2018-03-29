using System;
namespace IKAnalyzerNet
{
	public class MTokenDelegate:TokenDelegate
	{
		internal MTokenDelegate(int offset, int begin, int end):base(offset, begin, end)
		{
		}
		
		public override int CompareTo(Object o)
		{
			MTokenDelegate mtd = (MTokenDelegate) o;
			if (Begin < mtd.Begin)
				return - 1;
			if (Begin == mtd.Begin)
				return 0;
			return End > mtd.End?1:0;
		}
	}
}