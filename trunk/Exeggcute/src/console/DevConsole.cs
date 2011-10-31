using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Exeggcute.src.assets;
using Exeggcute.src.console.commands;
using Exeggcute.src.entities;
using Exeggcute.src.graphics;
using Exeggcute.src.text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Exeggcute.src.console
{

    class DevConsole : ConsoleContext
    {
        private static string welcomeMessage =
@"Welcome! Enter 'help' to get a list of commands.

Keyboard controls:
    ~                   Open/close the dev console.

    tab                 Resize console.

    up/down             Scroll through history of commands.";
        public Dictionary<string, bool> Keywords { get; protected set; }

        protected KeyboardManager kbManager;
        protected ConsoleBuffer textBuffer;

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
        public DevConsole() 
        {
            Keywords = new Dictionary<string, bool>();
            foreach (Keyword type in Enum.GetValues(typeof(Keyword)))
            {
                Keywords[type.ToString().ToLower()] = true;
            }
            Write(welcomeMessage);
            this.font = Assets.Font["consolas"];
            this.kbManager = new KeyboardManager();
            this.textBuffer = new ConsoleBuffer(this, prompt);
            this.lineSpacing = font.LineSpacing;
            Resize();
            //this.promptPos = new Vector2(2, lineSpacing * outputLines + 2);
            //this.bgRect = new RectSprite((int)Engine.XRes, (int)(lineSpacing * (outputLines + 1)), new Color(0, 0, 0, bgAlpha), true);
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

        public void InputCommand(string s)
        {
            string logInput = string.Format("{0}{1}", prompt, s);
            Write(logInput);
            ConsoleCommand command = parser.Parse(this, s);
            if (command == null) return;
            command.AcceptCommand(this);
        }


        public override void Update(ControlManager controls)
        {
            kbManager.Update();
            textBuffer.Update(kbManager);
            int delta = controls.MouseWheelDelta;
            outputPtr = Util.Clamp(outputPtr - delta, 0, output.Count - 8);
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

        public void Write(IEnumerable<string> list)
        {
            foreach (string s in list)
            {
                Write(s);
            }
        }

        public void Write(string message, params object[] args)
        {
            Write(string.Format(message, args));
        }

        public void Write(string message)
        {
            string replaces = Regex.Replace(message, "\r\n", "\n");
            string[] lines = Regex.Split(message, "\n");
            foreach (string msg in lines)
            {
                output.Add(msg);
            }

            if (lines.Length > 0)
            {
                outputPtr = output.Count;
            }
        }

        public override void Unload()
        {

        }

        public override void Dispose()
        {

        }

        public override void AcceptCommand(ConsoleCommand command)
        {
            Write("There is no overloaded method to accept a command of type {0}, i.e. it has\n not yet been implemented", command.GetType().Name);
        }

        public override void AcceptCommand(ExitCommand exit)
        {
            Game.GameHandleDONTUSE.Exit();
        }

        public override void AcceptCommand(ResetCommand reset)
        {
            Game.GameHandleDONTUSE.Reset(null);
        }

        public override void AcceptCommand(GoCommand command)
        {
            Write("Attempting to change contexts to {0}", command.Name);
            World.ContextSwitch(command.Name);
        }

        public override void AcceptCommand(LoadCommand load)
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
                    Write("No valid datasets found... You should probably redownload the game =[");
                    return;
                }
                Write("No data set named {0} exists. Valid values are:");
                Write(validSets);
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
            if (type == FileType.Behavior)
            {
                names = Assets.Behavior.GetLoadedNames();
            }
            else if (type == FileType.Spawn)
            {
                names = Assets.Spawn.GetLoadedNames();
            }
            else if (type == FileType.Trajectory)
            {
                names = Assets.Trajectory.GetLoadedNames();
            }
            else if (type == FileType.Body)
            {
                names = Assets.Body.GetLoadedNames();
            }
            else if (type == FileType.Item)
            {
                names = Assets.Item.GetLoadedNames();
            }
            else if (type == FileType.Boss)
            {
                names = notImplemented;
            }
            else if (type == FileType.Level)
            {
                names = notImplemented;
            }
            else if (type == FileType.Campaign)
            {
                names = notImplemented;
            }
            else if (type == FileType.Enemy)
            {
                names = Assets.Enemy.GetLoadedNames();
            }
            else if (type == FileType.GibBatch)
            {
                names = notImplemented;
            }
            else if (type == FileType.Option)
            {
                names = Assets.Option.GetLoadedNames();
            }
            else if (type == FileType.Font)
            {
                names = Assets.Font.GetLoadedNames();
            }
            else if (type == FileType.Model)
            {
                names = Assets.Model.GetLoadedNames();
            }
            else if (type == FileType.Sfx)
            {
                names = Assets.Sfx.GetLoadedNames();
            }
            else if (type == FileType.Song)
            {
                names = Assets.Song.GetLoadedNames();
            }
            else if (type == FileType.Texture)
            {
                names = Assets.Texture.GetLoadedNames();
            }
            else if (type == FileType.Sprite)
            {
                names = Assets.Sprite.GetLoadedNames();
            }
            else if (type == FileType.Player)
            {
                names = Assets.Player.GetLoadedNames();
            }
            else
            {
                string msg = string.Format("Did not expect type {0}", type);
                names = new List<string> { msg };
            }

            Write(message);
            Write(names);

            
        }

        public override void AcceptCommand(SpawnCommand spawn)
        {
            SpawnType type = spawn.Type;
            string name = spawn.Name;
            Float3 pos = spawn.Position;
            FloatValue angle = spawn.Angle;
            if (type == SpawnType.Player)
            {
                World.InsertPlayer(name);
            }
            else if (type == SpawnType.Boss)
            {
                World.InsertBoss(name);
            }
            else if (type == SpawnType.Enemy)
            {
                World.InsertEnemy(name, pos, angle);
            }
            else
            {
                Write("Did not expect the spawn type {0}.", type);
            }
        }
    }
}
