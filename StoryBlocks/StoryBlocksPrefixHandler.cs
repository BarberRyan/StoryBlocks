﻿using System;
using SBL = StoryBlocks.SBLib;
namespace StoryBlocks
{
	public static class SBPrefixHandler
	{
		static SBPrefixHandler()
		{
		}

		//Array of prefix codes in string form to give an index
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
			"?!=:",
			"?>=:",
			"?>:",
			"I+:",
			"I-:",
			"!=:",
			"X=:",
			">>:",
			"!!:",
			"XX:",
			"!X:"
		};

		//enumerator to make remembering each prefix index easier
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
			notEqual,
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

		//Takes in a line from the file, gets the prefix (from GetPrefix), and then returns the corresponding index number.
		//If the prefix does not match any known prefixes, returns -1.
		//line: line string provided by StreamReader.
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

		//Takes in a line and returns its prefix.
		//line: line string provided by StreamReader
		public static string GetPrefix(string line)
        {
			return line.Substring(0, line.IndexOf(':') + 1);
        }
	}
}