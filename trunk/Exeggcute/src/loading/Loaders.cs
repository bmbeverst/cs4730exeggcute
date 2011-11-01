using Exeggcute.src.graphics;
using Exeggcute.src.scripting.action;
using Exeggcute.src.scripting.task;

namespace Exeggcute.src.loading
{
    /// <summary>
    /// Singleton accessor for loaders
    /// </summary>
    static class Loaders
    {
        public static ScriptLoader Script;
        public static ConversationLoader Conversation;
        public static LevelLoader Level;
        public static BossLoader Boss;
        public static PlayerLoader Player;
        public static EnemyLoader Enemy;
        public static SpriteLoader Sprite;
        public static OptionLoader Option;
        public static ItemBatchLoader ItemBatch;
        public static TaskListLoader TaskList;

        public static void Reset()
        {
            Script = new ScriptLoader();
            Conversation = new ConversationLoader();
            Level = new LevelLoader();
            Boss = new BossLoader();
            Player = new PlayerLoader();
            Enemy = new EnemyLoader();
            Sprite = new SpriteLoader();
            Option = new OptionLoader();
            ItemBatch = new ItemBatchLoader();
            TaskList = new TaskListLoader();
        }
    }
}
