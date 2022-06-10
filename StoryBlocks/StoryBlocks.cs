using SBL = StoryBlocks.SBLib;
using SBM = StoryBlocks.SBMenu;
namespace StoryBlocks
{
    public class StoryBlocksMain
    {
        public static void Main(string[] args)
        {
            Console.CursorVisible = false;

            //file path for default story file.

            string filePath = System.IO.Directory.GetCurrentDirectory() + "/Stories/default.txt";

            //get file path from command line arguments (or dragging a .txt file onto the .exe).

            if (args.Length != 0)
            {
                filePath = args[0];

            }
            LoadStory(filePath);
        }

        //Sets the story up, also reloads the file if currently running.
        //story: file path of the story file to load

        public static void LoadStory(string story)
        {
            SBL.storyBlocks.Clear();
            SBL.ClearDicts();
            SBL.CreateBlocks(story);
            SBL.LoadConfig();
            Console.Title = "Story Blocks: " + SBL.title;
            SBM.CreateMenu("MAIN MENU");
        }
    }
}