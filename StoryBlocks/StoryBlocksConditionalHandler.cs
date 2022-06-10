using System;
using SBM = StoryBlocks.SBMenu;
using SBL = StoryBlocks.SBLib;
namespace StoryBlocks
{
	public static class SBConditionalHandler
	{
		public static Dictionary<string, (string, string, string, string)> globalConditionals = new Dictionary<string, (string, string, string, string)>();
		static SBConditionalHandler()
		{
		}
		public static void clearConds()
        {
			globalConditionals.Clear();
        }

		public static Boolean addGlobalCond(string name, string operation, string stat, string cond, string jump)
        {
			globalConditionals.Add(name, (operation, stat, cond, jump));
			return true;
        }
			
	}
}