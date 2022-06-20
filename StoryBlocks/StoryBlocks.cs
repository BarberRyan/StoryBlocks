using SBL = StoryBlocks.SBLib;
using SBM = StoryBlocks.SBMenu;
using SBDS = StoryBlocks.SBDefaultStory;
namespace StoryBlocks
{
    public class StoryBlocksMain
    {
        public static void Main(string[] args)
        {
            Console.CursorVisible = false;

            SBDS.GenerateDefaultStory();

            string storyPath = Directory.GetCurrentDirectory() + "/Stories/default.txt";

            //get file path from command line arguments (or dragging a .txt file onto the .exe).
            if (args.Length != 0)
            {
                storyPath = args[0];
                LoadStory(storyPath);
            }

            SBM.FileMenu();
        }

        //Sets the story up, also reloads the file if currently running.
        //story: file path of the story file to load

        public static void LoadStory(string story)
        {
            Console.Clear();
            SBL.StoryBlocks.Clear();
            SBL.ClearDicts();
            SBL.CreateBlocks(story);
            SBL.LoadConfig();
            Console.Title = $"StoryBlocks: {SBL.title}";
            SBM.CreateMenu("MAIN MENU");
        }
    }
}