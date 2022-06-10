using SBL = StoryBlocks.SBLib;
using SBM = StoryBlocks.SBMenu;
namespace StoryBlocks
{
    public class StoryBlocksMain
    {
        public static void Main(string[] args)
        {
            Console.CursorVisible = false;

            string filePath = "FormatTest.txt";

            if (args.Length != 0)
            {
                filePath = args[0];

            }
            LoadStory(filePath);
        }
        public static void LoadStory(string story)
        {
            SBL.storyBlocks.Clear();
            SBL.ClearDicts();
            SBL.CreateBlocks(story);
            SBL.LoadConfig();
            Console.Title = "Story Blocks: " + SBM.GetMenuStory("MAIN MENU");
            SBM.CreateMenu("MAIN MENU");
        }
    }
}