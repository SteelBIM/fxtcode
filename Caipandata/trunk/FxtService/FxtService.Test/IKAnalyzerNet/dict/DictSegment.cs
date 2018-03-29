using System;
using System.Collections;
namespace IKAnalyzerNet.dict
{
	public class DictSegment
	{
		virtual public int NodeState
		{
			get
			{
				return nodeState;
			}
			
			set
			{
				this.nodeState = value;
			}
			
		}
		virtual public WordType WordType
		{
			get
			{
				return wordType;
			}
			
		}
		
		public DictSegment()
		{
			nodeState = 0;
			wordType = new WordType();
		}
		
		public virtual void  addWord(char[] seg, WordType wordType)
		{
			addWord(seg, 0, seg.Length - 1, wordType);
		}
		
		public virtual void  addWord(char[] seg, int begin, int end, WordType wordType)
		{
			if (dictTreeNode == null)
			{
				dictTreeNode = new Hashtable(2, 0.8F);
			}
			Char keyChar = seg[begin];
			DictSegment ds = (DictSegment) dictTreeNode[keyChar];
			if (ds == null)
			{
				ds = new DictSegment();
				dictTreeNode[keyChar] = ds;
			}
			if (begin < end)
				ds.addWord(seg, begin + 1, end, wordType);
			else if (begin == end)
			{
				ds.NodeState = 1;
				ds.wordType.addWordType(wordType);
			}
		}
		
		public virtual Hit search(char[] seg, int begin, int end)
		{
			Hit searchHit = new Hit();
			return search(seg, begin, end, searchHit);
		}
		
		private Hit search(char[] seg, int begin, int end, Hit searchHit)
		{
			if (dictTreeNode == null)
			{
				searchHit.setUnmatch();
				return searchHit;
			}
			Char keyChar = seg[begin];
			DictSegment ds = (DictSegment) dictTreeNode[keyChar];
			if (ds != null)
			{
				if (begin < end)
					return ds.search(seg, begin + 1, end, searchHit);
				if (begin == end)
				{
					if (ds.NodeState == 1)
					{
						searchHit.setMatch();
						searchHit.WordType = ds.WordType;
					}
					if (ds.hasNextNode())
						searchHit.setPrefixMatch();
				}
			}
			else
			{
				searchHit.setUnmatch();
			}
			return searchHit;
		}
		
		public virtual bool hasNextNode()
		{
			return dictTreeNode != null && !(dictTreeNode.Count == 0);
		}
		
		private Hashtable dictTreeNode;
		private int nodeState;
		private WordType wordType;
	}
}