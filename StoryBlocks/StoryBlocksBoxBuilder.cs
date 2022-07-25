using System;
using SBTH = StoryBlocks.SBTextHandler;

namespace StoryBlocks
{
	public class SBBoxBuilder
	{
		public SBBoxBuilder()
		{
		}
		enum EBoxElements
        {
			TL,
			TR,
			BL,
			BR,
			H,
			V,
			UT,
			DT,
			LT,
			RT,
			X
        }

		static readonly char[] doubleBar = new char[]
		{
			'╔',
			'╗',
			'╚',
            '╝',
			'═',
			'║',
			'╩',
			'╦',
			'╣',
			'╠',
			'╬'
		};

		static readonly char[] singleBar = new char[]
		{
            '┌',
			'┐',
			'└',
			'┘',
			'─',
			'│',
			'┴',
			'┬',
			'┤',
			'├',
			'┼'
		};

		static readonly char[] blocks = new char[]
		{
			'█',
			'█',
			'█',
			'█',
			'█',
			'█',
			'█',
			'█',
			'█',
			'█',
			'█'
		};

		public static int GetLongestLength(List<string> inputList, int pad = 0)
        {
			int longestEntry = 0;

			foreach(string item in inputList)
            {
				if(item.Length > longestEntry)
                {
					longestEntry = item.Length;
                }
            }

			return longestEntry + pad;

        }

		public static void DrawBox(string text, string? title = null, string style = "DOUBLE", int xCoord = 0, int yCoord = 0, string textColor = "WHITE", string bkgColor = "BLACK")
        {
			List<string> menuText = new();
			menuText.Add(text);
			DrawBox(menuText, title, style, xCoord, yCoord, textColor, bkgColor);
		}

		public static void DrawBox(Dictionary<string, int> menu, string? title = null, string style = "DOUBLE", int xCoord = 0, int yCoord = 0, string textColor = "WHITE", string bkgColor = "BLACK")
        {
			List<string> menuText = new();
			foreach(var item in menu)
            {
				menuText.Add($"{item.Key} : {item.Value}");
            }
			DrawBox(menuText, title, style, xCoord, yCoord, textColor, bkgColor);
        }

		public static void DrawBox(Dictionary<string, string> menu, string? title = null, string style = "DOUBLE", int xCoord = 0, int yCoord = 0, string textColor = "WHITE", string bkgColor = "BLACK")
		{
			List<string> menuText = new();
			foreach (var item in menu)
			{
				menuText.Add(item.Value);
			}
			DrawBox(menuText, title, style, xCoord, yCoord, textColor, bkgColor);
		}

		public static void DrawBox(List<string> text, string? title = null, string style = "DOUBLE", int xCoord = 0, int yCoord = 0, string textColor = "WHITE", string bkgColor = "BLACK")
        {
			List<char> boxParts = new();
			int longestLine = GetLongestLength(text, 9);

			if (style.ToUpper() == "SINGLE")
			{
				boxParts.AddRange(singleBar);
			}
			else if (style.ToUpper() == "BLOCK")
            {
				boxParts.AddRange(blocks);
            }
			else
			{
				boxParts.AddRange(doubleBar);
			}

			char TL = boxParts[(int)EBoxElements.TL];
			char TR = boxParts[(int)EBoxElements.TR];
			char BL = boxParts[(int)EBoxElements.BL];
			char BR = boxParts[(int)EBoxElements.BR];
			char H = boxParts[(int)EBoxElements.H];
			char V = boxParts[(int)EBoxElements.V];
			char UT = boxParts[(int)EBoxElements.UT];
			char DT = boxParts[(int)EBoxElements.DT];
			char LT = boxParts[(int)EBoxElements.LT];
			char RT = boxParts[(int)EBoxElements.RT];
			char X = boxParts[(int)EBoxElements.X];

			Console.ForegroundColor = SBTH.getColor(textColor);
			Console.BackgroundColor = SBTH.getColor(bkgColor);
			Console.SetCursorPosition(xCoord, yCoord);

			if (title != null)
            {
				SBTH.PrintText(TL.ToString().PadRight(title.Length + 3, H) + TR);
				yCoord++;
				Console.SetCursorPosition(xCoord, yCoord);
				SBTH.PrintText($"{V} {title} {V}");
				yCoord++;
				Console.SetCursorPosition(xCoord, yCoord);
				SBTH.PrintText((RT.ToString().PadRight(title.Length + 3, H) + UT.ToString()).PadRight(longestLine, H) + TR);
			}
            else
            {
				SBTH.PrintText(TL.ToString().PadRight(longestLine, H) + TR);
			}

			for (int i = 0; i < text.Count; i++)
            {
				Console.SetCursorPosition(xCoord, yCoord + i + 1);
				SBTH.PrintText(($"{V}    {text[i]}    ").PadRight(longestLine, ' ') + V);
            }

			Console.SetCursorPosition(xCoord, yCoord + text.Count + 1);
			SBTH.PrintText(BL.ToString().PadRight(longestLine, H) + BR);

			Console.ResetColor();
		}
	
	
	}
}