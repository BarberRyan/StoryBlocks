using System;
using SBL = StoryBlocks.SBLib;
namespace StoryBlocks
{
	public static class SBPrefixHandler
	{
		static SBPrefixHandler()
		{
		}
        static string[] PrefixList = 
		{ 
			"1:",
			"T:",
			"S:",
			">:",
			"!I:",
			"XI:",
			"!S:",
			"XS:",
			"+:", 
			"-:",
			"*:",
			"/:",
			"?<:",
			"?<=:",
			"?=:",
			"?>=:",
			"?>:",
			"I+:",
			"I-:",
			"!I=:",
			"XI=:",
			">>:",
			"!!:",
			"XX:",
			"!X:"
		};

		public enum EPrefix
		{
			startBlock,
			title,
			story,
			loadBlock,
			visibleInt,
			hiddenInt,
			visibleStr,
			hiddenStr,
			add,
			subtract,
			multiply,
			divide,
			lessThan,
			lessOrEqual,
			equal,
			greaterOrEqual,
            greaterThan,
			inventoryAdd,
			inventorySubtract,
			inputVisible,
			inputHidden,
			immediateJump,
			makeVisible,
			makeHidden,
			toggleVisibility
		}
		public static int GetPrefixIndex(string line)
        {
			string prefix = GetPrefix(line);
            if (PrefixList.Contains(prefix))
            {
				return Array.IndexOf(PrefixList, prefix);
            }
            else
            {
				return -1;
            }
        }

		public static string GetPrefix(string line)
        {
			return line.Substring(0, line.IndexOf(':') + 1);
        }
	}
}