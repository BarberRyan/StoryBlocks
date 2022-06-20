using System;
using SBL = StoryBlocks.SBLib;
namespace StoryBlocks
{
	public static class SBPrefixHandler
	{
		static SBPrefixHandler()
		{
		}

		//Array of prefix codes in string form to give an index
        static readonly string[] PrefixList = new string[]
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
			menuChoice,
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
			return line[..(line.IndexOf(':') + 1)];
        }

		public static string GetSubPrefix(string line)
        {
			string mainPrefix = GetPrefix(line);
			string subPrefix = line[mainPrefix.Length..];
			return subPrefix;
        }
		public static string GetNextPrefix(string line)
        {
			string subPrefix = line[GetPrefix(line).Length..];
			string[] elements = subPrefix.Split(new string[] { ":" }, StringSplitOptions.None);
			for(int i = 0; i < elements.Length; i++)
            {
				string blip = elements[i] + ":";
				if (PrefixList.Contains(blip))
                {
					return subPrefix[subPrefix.IndexOf(blip)..];
				}
				
			}
			return subPrefix;
        }
	}
}