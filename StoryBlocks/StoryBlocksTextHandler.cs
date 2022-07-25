using System;
namespace StoryBlocks
{
	public class SBTextHandler
	{
		public SBTextHandler()
		{
		}

		static List<(string, string)> colors = new();

		public static string ExtractColors(string inputString)
		{
			if (inputString.Contains('[') && inputString.Contains(']'))
			{
				int startIndex = 0;
				int colorLength = 0;

				List<string> colorList = new();

				for (int i = 0; i < inputString.Length; i++)
				{
					if (inputString[i] == '[')
					{
						startIndex = i + 1;
					}
					if (inputString[i] == ']')
                    {
                        colorLength = i - startIndex;
						colorList.Add(inputString.Substring(startIndex, colorLength).ToUpper().Replace(" ", String.Empty));
						inputString = inputString.Remove(startIndex - 1, colorLength + 2);
						i -= colorLength + 2;
					}
				}
				foreach (var item in colorList)
				{
					if (item.Contains(','))
					{
						var splits = item.Split(',');
						colors.Add((splits[0], splits[1]));
					}
                    else
                    {
						colors.Add((item, "BLACK"));
                    }
				}
			}
			return inputString;
		}

		public static void PrintText(string inputString)
        {
			inputString = ExtractColors(inputString);
			if (SBLib.OutputOption == "CONSOLE")
			{

				for (int i = 0; i < inputString.Length; i++)
				{
					if (inputString[i] == '{')
					{
						if (colors.Count > 0)
						{
							Console.ForegroundColor = getColor(colors.ElementAt(0).Item1, true);
							Console.BackgroundColor = getColor(colors.ElementAt(0).Item2, false);
							colors.Remove(colors.ElementAt(0));
						}

						continue;
					}
					if (inputString[i] == '}')
					{
						Console.ResetColor();
						continue;
					}
					Console.Write(inputString[i]);
					if (i == inputString.Length - 1)
					{
						Console.Write("\n");
					}
				}
			}
        }

		//Takes in a string and returns a corresponding ConsoleColor.
		//input: string argument for color provided in a line ("{TEXT TO CHANGE}[TEXT COLOR][BACKGROUND COLOR]")
		//text: true = foreground color, false = background color.
		public static ConsoleColor getColor(string input, bool text = true)
		{
			switch (input.ToUpper())
			{
				case "BLACK":
					return ConsoleColor.Black;

				case "BLUE":
					return ConsoleColor.Blue;

				case "CYAN":
				case "LIGHT BLUE":
				case "LIGHTBLUE":
					return ConsoleColor.Cyan;

				case "DARK BLUE":
				case "DARKBLUE":
					return ConsoleColor.DarkBlue;

				case "DARK CYAN":
				case "DARKCYAN":
					return ConsoleColor.DarkCyan;

				case "DARK GREY":
				case "DARKGREY":
				case "DARK GRAY":
				case "DARKGRAY":
					return ConsoleColor.DarkGray;


				case "DARK GREEN":
				case "DARKGREEN":
					return ConsoleColor.DarkGreen;

				case "DARK MAGENTA":
				case "DARKMAGENTA":
				case "PURPLE":
					return ConsoleColor.DarkMagenta;

				case "DARK RED":
				case "DARKRED":
					return ConsoleColor.DarkRed;

				case "DARK YELLOW":
				case "DARKYELLOW":
				case "GOLD":
				case "ORANGE":
					return ConsoleColor.DarkYellow;

				case "GRAY":
				case "GREY":
					return ConsoleColor.Gray;

				case "GREEN":
					return ConsoleColor.Green;

				case "MAGENTA":
				case "PINK":
					return ConsoleColor.Magenta;

				case "RED":
					return ConsoleColor.Red;

				case "WHITE":
					return ConsoleColor.White;

				case "YELLOW":
					return ConsoleColor.Yellow;

				case "RANDOM":
					var consoleColors = Enum.GetValues(typeof(ConsoleColor));
					var r = new Random();
					ConsoleColor randColor = (ConsoleColor)consoleColors.GetValue(r.Next(consoleColors.Length));
					while(randColor == Console.BackgroundColor)
                    {
						randColor = (ConsoleColor)consoleColors.GetValue(r.Next(consoleColors.Length));
					}
					return randColor;

				default:
					if (text)
					{
						return ConsoleColor.White;
					}
					else
					{
						return ConsoleColor.Black;
					}
			}
		}

	}
}