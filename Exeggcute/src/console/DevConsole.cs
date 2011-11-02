using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Exeggcute.src.assets;
using Exeggcute.src.console.commands;
using Exeggcute.src.graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Exeggcute.src.loading;
using Exeggcute.src.scripting.task;
using Exeggcute.src.entities;
using Exeggcute.src.console.trackers;
using System.IO;

namespace Exeggcute.src.console
{
    /// <summary>
    /// fixme: go with invalid name crashes game
    /// mouse scroll wheeel does nto scroll history when scroll history is 8 lines or less
    /// make alignment parameter for entities, not for arsenal
    /// </summary>
    class DevConsole : ConsoleContext
    {
        private static string welcomeMessage =
@"Welcome! Enter 'help' to get a list of commands.

Keyboard controls:
    ~                   Open/close the dev console.

    tab                 Resize console.

    up/down             Scroll through history of commands.";
        public Dictionary<string, bool> Keywords { get; protected set; }
        
        public const string HistoryFile = "history";
        public const int HistoryRemembered = 8;

        public const string LogFile = "log";
        public const int LogRemembered = 8;

        public const string InitFile = "init";

        protected KeyboardManager kbManager;
        protected PromptBuffer textBuffer;

        protected SpriteFont font;
        protected CommandParser parser = new CommandParser();

        protected List<string> output = new List<string>();
        protected int outputLines = 4;
        protected int outputPtr = 0;
        protected const string prompt = "|[console]|: ";
        protected Vector2 promptPos;
        protected float lineSpacing;

        protected RectSprite bgRect;
        protected int bgAlpha = 200;

        protected bool inBackground;

        public DevConsole() 
        {
            List<string> history = getHistory();

            Keywords = new Dictionary<string, bool>();
            foreach (Keyword type in Enum.GetValues(typeof(Keyword)))
            {
                Keywords[type.ToString().ToLower()] = true;
            }
            WriteLine(welcomeMessage);
            this.font = Assets.Font["consolas"];
            this.kbManager = new KeyboardManager();
            this.textBuffer = new PromptBuffer(this, prompt, history);
            this.lineSpacing = font.LineSpacing;
            Resize();
            
        }
        private List<string> getHistory()
        {
            List<string> history = new List<string>();
            if (File.Exists(HistoryFile))
            {
                history = Util.ReadLines(HistoryFile);
            }
            return history;
        }


        public void RunInit()
        {
            string filepath = InitFile;
            List<string> lines = new List<string>();
            if (File.Exists(filepath))
            {
                lines = Util.ReadAndStrip(filepath);
            }
            foreach (string line in lines)
            {
                InputCommand(line);
            }
        }

        public bool IsKeyword(string type)
        {
            return Keywords.ContainsKey(type.ToLower());
        }

        public void Resize()
        {
            if (outputLines == 8)
            {
                outputLines = 16;
            }
            else if (outputLines == 16)
            {
                outputLines = 4;
            }
            else
            {
                outputLines = 8;
            }
            this.bgRect = new RectSprite((int)Engine.XRes, (int)(lineSpacing * (outputLines + 1)), new Color(0, 0, 0, bgAlpha), true);
            this.promptPos = new Vector2(2, lineSpacing * outputLines + 2);
        }

        public void AttachParent(IContext parent)
        {
            this.Parent = parent;
        }

        public void DetachParent()
        {
            this.Parent = null;
        }

        public void InputCommand(string cmd)
        {
            string logInput = string.Format("{0}{1}", prompt, cmd);
            WriteLine(logInput);
            ConsoleCommand command = parser.Parse(this, cmd);
            if (command == null) return;
            try
            {
                command.AcceptCommand(this);
            }
            catch (Exception e)
            {
               WriteLine(
@"Execution of the command '{0}' terminated unexpectedly.
    Useless information follows:
{1}", cmd, e.Message);

            }
        }

        public override void Update(ControlManager controls)
        {
            kbManager.Update();
            textBuffer.Update(kbManager);
            if (controls.IsLeftClicking)
            {
                Entity3D clicked = Worlds.World.GetUnderMouse(controls.MousePosition);
                Console.WriteLine(clicked);
                if (clicked != null)
                {
                    textBuffer.AddToBuffer(clicked.ID + " ");
                }
            }
            int delta = controls.MouseWheelDelta;
            outputPtr = Util.Clamp(outputPtr - delta, 0, output.Count - 8);
            controls.EatAll();
            if (inBackground)
            {
                
                Parent.Update(controls);
            }
        }

        public override void Draw3D(GraphicsDevice graphics, Camera camera)
        {
            Parent.Draw3D(graphics, camera);
        }

        public override void Draw2D(SpriteBatch batch)
        {

            Parent.Draw2D(batch);
            bgRect.Draw(batch, new Vector2(0, 0));
            textBuffer.Draw(batch, font, promptPos);
            drawOutput(batch);
        }
        
        protected void drawOutput(SpriteBatch batch)
        {
            int end = Math.Min(output.Count, outputPtr + 8);
            int start = Math.Max(0, end - outputLines);

            int count = 0;
            for (int i = start; i < end; i += 1)
            {
                if (i >= output.Count) continue;
                batch.DrawString(font, output[i], promptPos + new Vector2(0, lineSpacing * count - lineSpacing*outputLines), Color.White);
                count += 1;
            }
        }

        public void WriteLines<T>(IEnumerable<T> list)
        {
            foreach (T obj in list)
            {
                addLine(obj.ToString());
            }
        }

        public void WriteLine(string message, params object[] args)
        {
            string formatted = string.Format(message, args);
            addLine(formatted);
        }

        public void WriteLine(string message)
        {
            addLine(message);
        }

        public void Write(string message)
        {
            if (message.Contains('\n'))
            {
                string replaces = Regex.Replace(message, "\r\n", "\n");
                string[] lines = Regex.Split(message, "\n");
                if (lines.Length == 0)
                {
                    appendToLine(string.Format("failure to write {0}", message));
                    return;
                }
                appendToLine(lines[0]);
                for (int i = 1; i < lines.Length; i += 1)
                {
                    addLine(lines[i]);
                }
            }
            else
            {
                Console.WriteLine("here");
                appendToLine(message);
            }
        }

        public void WriteToBuffer(string message, params object[] args)
        {
            string formatted = string.Format(message, args);
            textBuffer.AddToBuffer(formatted);
        }

        protected void appendToLine(string message)
        {
            output[output.Count - 1] += message;
        }

        protected void addLine(string message)
        {

            string replaces = Regex.Replace(message, "\r\n", "\n");
            string[] lines = Regex.Split(message, "\n");
            foreach (string line in lines)
            {
                output.Add(line);
            }

            outputPtr = output.Count;
        }

        public override void Unload()
        {

        }

        public override void Dispose()
        {
            WriteHistory();
        }

        public void WriteHistory()
        {
            Util.WriteFile(HistoryFile, textBuffer.GetHistory(HistoryRemembered));
            Util.WriteFile(LogFile, GetOutput(LogRemembered));
        }

        public List<string> GetOutput(int count)
        {
            List<string> result = new List<string>();
            int written = 0;
            for (int i = output.Count - 1; i >= 0 && written < count; i -= 1)
            {
                result.Insert(0, output[i]);
            }

            return result;
        }

        public override void AcceptCommand(ConsoleCommand command)
        {
            WriteLine("There is no overloaded method to accept a command of type {0}, i.e. it has\n not yet been implemented", command.GetType().Name);
        }

        public virtual void AcceptCommand(SetGlobalCommand global)
        {
            WriteLine("First implement global settings, then you can set them!");
        }

        public override void AcceptCommand(BuiltinCommand cmd)
        {
            if (cmd.Command == Builtin.bg)
            {
                inBackground = true;
            }
            else if (cmd.Command == Builtin.fg)
            {
                inBackground = false;
            }
            else
            {
                WriteLine("Do not know how to handle {0}", cmd.Command);
            }
        }

        public override void AcceptCommand(TrackCommand track)
        {
            if (!Worlds.World.Entities.ContainsKey(track.ID))
            {
                WriteLine("No entity found with ID {0}", track.ID);
                return;
            }
            Entity3D entity = Worlds.World.Entities[track.ID];
            Tracker tracker;
            if (Util.StrEq(track.Type, "console"))
            {
                tracker = new ConsoleTracker(entity, track.Format, track.Indices, track.Frequency);
            }
            else if (Util.StrEq(track.Type, "hud"))
            {
                tracker = new HudTracker(entity, track.Format, track.Indices, track.Frequency);
            }
            else if (Util.StrEq(track.Type, "entity"))
            {
                tracker = new EntityTracker(entity, track.Format, track.Indices, track.Frequency);
                Worlds.World.AttachTracker(entity, tracker);
                return;
            }
            else
            {
                WriteLine("Don't know any tracker called {0}", track.Type);
                return;
            }
            Worlds.World.AddTracker(tracker);
        }

        public override void AcceptCommand(ClearCommand clear)
        {
            string thing = clear.Thing;
            if (thing == null || Util.StrEq(thing, "console"))
            {
                output = new List<string>();
            }
            else if (Util.StrEq(thing, "trackers"))
            {
                Worlds.World.ClearTrackers();
            }
            else if (Util.StrEq(thing, "all"))
            {
                //fixme code clones
                output = new List<string>();
                Worlds.World.ClearTrackers();
                
            }
            else
            {
                WriteLine("Don't know how to clear \"{0}\".", thing);
            }
        }

        public override void AcceptCommand(SetParamCommand set)
        {
            Worlds.World.SetParameter(set);
        }

        public override void AcceptCommand(LevelTaskCommand task)
        {
            List<Task> tasks;
            try
            {
                tasks = Loaders.TaskList.ParseElement(task.TaskString);
            }
            catch
            {
                WriteLine("Syntax error.");
                return;
            }
            try
            {
                Worlds.World.SendTask(tasks);
            }
            catch
            {
                WriteLine("Cannot send tasks to this context.");
                return;
            }
        }

        public override void AcceptCommand(ExitCommand exit)
        {
            Game.GameHandleDONTUSE.Exit();
        }

        public override void AcceptCommand(ResetCommand reset)
        {
            throw new ResetException(null);
        }

        public override void AcceptCommand(GoCommand command)
        {
            bool isSandbox = Sandbox.IsName(command.Name);
            if (!isSandbox && !Assets.Level.ContainsKey(command.Name))
            {
                WriteLine("No context called \"{0}\" could be found. Valid contexts are:\nsandbox", command.Name);
                WriteLines(Assets.Level.GetLoadedNames());
                return;
            }
            WriteLine("Attempting to change contexts to {0}", command.Name);
            Worlds.World.ContextSwitch(command.Name, isSandbox);
        }

        public override void AcceptCommand(LoadSetCommand load)
        {
            string name = load.Name;
            if (Manifest.VerifyExists(name))
            {
                //This is pretty much the only place you SHOULD use it
                Game.GameHandleDONTUSE.Reset(name);
            }
            else
            {
                List<string> validSets = Manifest.GetValidSets();
                if (validSets.Count == 0)
                {
                    WriteLine("No valid datasets found... You should probably redownload the game =[");
                    return;
                }
                WriteLine("No data set named {0} exists. Valid values are:");
                WriteLines(validSets);
            }
            
        }

        public override void AcceptCommand(PackageCommand package)
        {
            string packageName = package.Name;
            DataSet set = new DataSet(packageName);
            set.Save();
        }

        // FIXME monstrosity
        public override void AcceptCommand(ListCommand list)
        {
            FileType type = list.Type;
            string message = "Loaded objects of that type include:\n";
            List<string> names;

            List<string> notImplemented = new List<string> { "not implemented/dynamically loaded" };
            Dictionary<FileType, List<string>> usages = new Dictionary<FileType, List<string>> 
            {
                { FileType.Behavior, Assets.Behavior.GetLoadedNames() },
                { FileType.Spawn, Assets.Spawn.GetLoadedNames() },
                { FileType.Trajectory, Assets.Trajectory.GetLoadedNames() },
                { FileType.Body, Assets.Body.GetLoadedNames() },
                { FileType.Item, Assets.Item.GetLoadedNames() },
                { FileType.Boss, notImplemented },
                { FileType.Level, Assets.Level.GetLoadedNames() },
                { FileType.Enemy, Assets.Enemy.GetLoadedNames() },
                { FileType.GibBatch, notImplemented },
                { FileType.Option, Assets.Option.GetLoadedNames() },
                { FileType.Font, Assets.Font.GetLoadedNames() },
                { FileType.Model, Assets.Model.GetLoadedNames() },
                { FileType.Sfx, Assets.Sfx.GetLoadedNames() },
                { FileType.Song, Assets.Song.GetLoadedNames() },
                { FileType.Texture, Assets.Texture.GetLoadedNames() },
                { FileType.Sprite, Assets.Sprite.GetLoadedNames() },
                { FileType.Player, Assets.Player.GetLoadedNames() }
            };

            if (!usages.ContainsKey(type))
            {
                string msg = string.Format("Did not expect type {0}", type);
                names = new List<string> { msg };
            }
            else
            {
                names = usages[type];
            }

            WriteLine(message);
            WriteLines(names);
        }

        public override void AcceptCommand(SpawnCommand spawn)
        {
            SpawnType type = spawn.Type;
            string name = spawn.Name;
            Float3 pos = spawn.Position;
            FloatValue angle = spawn.Angle;
            if (type == SpawnType.Player)
            {
                Worlds.World.InsertPlayer(name);
            }
            else if (type == SpawnType.Boss)
            {
                Worlds.World.InsertBoss(name);
            }
            else if (type == SpawnType.Enemy)
            {
                Worlds.World.InsertEnemy(name, pos, angle);
            }
            else
            {
                WriteLine("Did not expect the spawn type {0}.", type);
            }
        }
    }
}
