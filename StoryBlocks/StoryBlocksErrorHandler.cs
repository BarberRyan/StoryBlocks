using System;
namespace StoryBlocks
{
	public class SBErrorHandler
	{
		public SBErrorHandler()
		{
		}
		public enum EErrorCode
        {
			noBlock,
			unfinishedBlock
        }

		public static void ThrowError(int errorCode, string option1 = "", string option2 = "")
        {
			string errorText = "";
			Console.Clear();
			Console.ForegroundColor = ConsoleColor.Red;
			
			switch (errorCode)
            {
				case (int)EErrorCode.noBlock:
					errorText = $"ERROR! Block by the name of \"{option1}\" does not exist!\n\nPress any button to return to the main menu.";
					break;
				case (int)EErrorCode.unfinishedBlock:
					errorText = $"ERROR! Block by the name of \"{option1}\" does not end properly!\nBlocks must end with :: on a line by itself to be valid!\n\nPress any button to return to the main menu.";
					break;
            }
			Console.WriteLine(errorText);
			Console.ReadKey(true);
			Console.ResetColor();
			Console.Clear();
		}

	}
}
