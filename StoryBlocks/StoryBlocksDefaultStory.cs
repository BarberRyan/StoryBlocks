using System;
namespace StoryBlocks
{
	public class SBDefaultStory
	{
		public SBDefaultStory()
		{
		}

		//Creates the Stories folder for the program and populates a sample file
		public static void generateDefaultStory()
        {
			string defaultStoryPath = System.IO.Directory.GetCurrentDirectory() + "/Stories";
			string defaultFilePath = defaultStoryPath + "/default.txt";

			if (!System.IO.Directory.Exists(defaultStoryPath))
			{
				System.IO.Directory.CreateDirectory(defaultStoryPath);
			}

			if (!File.Exists(defaultFilePath))
			{
				using (StreamWriter writer = File.CreateText(defaultFilePath))
				{
					writer.WriteLine("CONFIG::\n" +
									 "1:Beginning\n" +
                                     "T:Title Goes Here!\n" +
                                     "!I:Money:0\n" +
                                     "!S:Name: \n" +
                                     "::\n");

					writer.WriteLine("MAIN MENU::\n" +
									 "S:░██████╗░█████╗░███╗░░░███╗██████╗░██╗░░░░░███████╗\n" +
                                     "S:██╔════╝██╔══██╗████╗░████║██╔══██╗██║░░░░░██╔════╝\n" +
                                     "S:╚█████╗░███████║██╔████╔██║██████╔╝██║░░░░░█████╗░░\n" +
                                     "S:░╚═══██╗██╔══██║██║╚██╔╝██║██╔═══╝░██║░░░░░██╔══╝░░\n" +
                                     "S:██████╔╝██║░░██║██║░╚═╝░██║██║░░░░░███████╗███████╗\n" +
                                     "S:╚═════╝░╚═╝░░╚═╝╚═╝░░░░░╚═╝╚═╝░░░░░╚══════╝╚══════╝:dark green\n" +
                                     ">:START:Start the story!\n" +
                                     ">:ABOUT:About this story!\n" +
                                     ">:RELOAD:Reload story file!\n" +
                                     ">:EXIT:Exit the program!\n" +
                                     "::\n");

					writer.WriteLine("ABOUT::\n" +
									 "S:This \"story\" was created by Ryan Barber as an example of the StoryBlocks system in 2022!\n" +
                                     ">:BACK:Back to main menu\n" +
                                     "::\n");

					writer.WriteLine("Beginning::\n" +
									 "S:Hello there, This is a sample file to show how formatting works. It is also the default \"story.\"\n" +
                                     "?=:Name: :Enter name:So What's your name?:Green\n" +
                                     ">:Find a dollar:Wait, is that a dollar on the ground?\n" +
                                     "::\n");

					writer.WriteLine("Enter name::\n" +
									 "!=:Name: Enter your name!:red\n" +
                                     "::\n");

					writer.WriteLine("Find a dollar::\n" +
									 "S:It is a dollar! Do you want to pick it up?\n" +
                                     ">:Pick up dollar:yes\n" +
                                     ">:I let this break on purpose!:no\n" +
                                     "::\n");

					writer.WriteLine("Pick up dollar::\n" +
									 "S:Nice! You got a one dollars!\n" +
                                     "+:Money:1\n" +
                                     ">:Beginning:Go back to the beginning with your new dollar!\n" +
                                     "::\n");
				}
			}
		}
	}
}