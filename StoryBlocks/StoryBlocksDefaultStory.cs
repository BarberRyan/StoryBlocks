using System;
namespace StoryBlocks
{
	public class SBDefaultStory
	{
		public SBDefaultStory()
		{
		}

		//Creates the Stories folder for the program and populates a sample file
		public static void GenerateDefaultStory()
        {
			string defaultStoryPath = Directory.GetCurrentDirectory() + "/Stories";
			string defaultFilePath = defaultStoryPath + "/default.txt";

			if (!Directory.Exists(defaultStoryPath))
			{
				Directory.CreateDirectory(defaultStoryPath);
			}

			if (File.Exists(defaultFilePath))
			{
				File.Delete(defaultFilePath);
			}

            using StreamWriter writer = File.CreateText(defaultFilePath);
            writer.WriteLine("CONFIG::\n" +
                              "1:Beginning\n" +
                             "T:Sample Story\n" +
                             "D:This is the description of the story\n" +
                             "!I:Money:0\n" +
                             "XS:Name:player\n" +
                             "::\n");

            writer.WriteLine("MAIN MENU::\n" +
                            "S:{░██████╗░█████╗░███╗░░░███╗██████╗░██╗░░░░░███████╗}[dark green]\n" +
                            "S:{██╔════╝██╔══██╗████╗░████║██╔══██╗██║░░░░░██╔════╝}[dark green]\n" +
                            "S:{╚█████╗░███████║██╔████╔██║██████╔╝██║░░░░░█████╗░░}[dark green]\n" +
                            "S:{░╚═══██╗██╔══██║██║╚██╔╝██║██╔═══╝░██║░░░░░██╔══╝░░}[dark green]\n" +
                            "S:{██████╔╝██║░░██║██║░╚═╝░██║██║░░░░░███████╗███████╗}[dark green]\n" +
                            "S:{╚═════╝░╚═╝░░╚═╝╚═╝░░░░░╚═╝╚═╝░░░░░╚══════╝╚══════╝}[dark green]\n" +
                            ">:START:Start the story!\n" +
                            ">:BLANK\n" +
                            ">:ABOUT:About this story!\n" +
                            ">:RELOAD:Reload story file!\n" +
                            ">:EXIT:Exit the story!\n" +
                            "::\n");

            writer.WriteLine("ABOUT::\n" +
                                "S:This \"story\" was created by {Ryan Barber}[green] as an example of the StoryBlocks system in 2022!\n" +
                                ">:BACK:Back to main menu\n" +
                                "::\n");

            writer.WriteLine("Beginning::\n" +
                                "S:Hello there {@Name@}[yellow], This is a sample file to show how formatting works. It is also the default \"story.\"\n" +
                                "?=:Name:player:>:Enter name:So What's your name?:Green\n" +
                                "?!=:Name:player:>:Enter name:Wait, your name isn't @Name@?:Green\n" +
                                "?>=:Money:5:>:Buy item:Hey, wanna buy something cool for $5?\n" +
                                "?=:Money:0:>:Find a dollar:Wait, is that a dollar on the ground?\n" +
                                "?>:Money:0:>:Find a dollar:Wait, is that another dollar?\n" +
                                "::\n");

            writer.WriteLine("Buy item::\n" +
                                "S:I know you want to buy it, so thanks for the money and here you go!!\n" +
                                "S:\n" +
                                "S:OBTAINED ONE CORN CHIP!\n" +
                                "-:Money:5\n" +
                                "I+:Corn Chip:1\n" +
                                ">:BACK:Go back\n" +
                                "::\n");

            writer.WriteLine("Enter name::\n" +
                                "!!:Name\n" +
                                "!=:Name: Enter your name!:red\n" +
                                "::\n");

            writer.WriteLine("Find a dollar::\n" +
                                "S:It IS a dollar! Do you want to pick it up?\n" +
                                ">:Pick up dollar:yes\n" +
                                ">:Leave dollar:no\n" +
                                "::\n");

            writer.WriteLine("Pick up dollar::\n" +
                                "S:Nice! You got a one dollars!\n" +
                                "+:Money:1\n" +
                                ">:Beginning:Go back to the beginning with your new dollar!\n" +
                                "::\n");

            writer.WriteLine("Leave dollar::\n" +
                                "S:You start to walk away, it's not yours after all...\n" +
                                "S:Then a man approaches you and tells you he was testing your honesty.\n" +
                                "S:He gave you $5!!!\n" +
                                "+:Money:5\n" +
                                ">:Beginning:Go back to the beginning with your money!\n" +
                                "::\n");
        }
	}
}