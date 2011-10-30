using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.text;
using Microsoft.Xna.Framework.Graphics;
using Exeggcute.src.assets;
using Microsoft.Xna.Framework;
using Exeggcute.src.console.commands;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework.Input;
using Exeggcute.src.graphics;
using Exeggcute.src.entities;

namespace Exeggcute.src.console
{

    class DevConsole : ConsoleContext
    {
        protected KeyboardManager kbManager;
        protected ConsoleBuffer textBuffer;

        protected SpriteFont font;
        protected CommandParser parser = new CommandParser();

        protected List<string> output = new List<string>();
        protected int outputLines = 8;
        protected int outputPtr = 0;
        protected const string prompt = "|[console]|: ";
        protected Vector2 promptPos;
        protected float lineSpacing;

        protected RectSprite bgRect;
        protected int bgAlpha = 200;
        public DevConsole() 
        {
            Write("Welcome! Enter 'help' to get a list of commands.");
            this.font = FontBank.Get("consolas");
            this.kbManager = new KeyboardManager();
            this.textBuffer = new ConsoleBuffer(this, prompt);
            this.lineSpacing = font.LineSpacing;
            this.promptPos = new Vector2(2, lineSpacing * outputLines + 2);
            this.bgRect = new RectSprite((int)Engine.XRes, (int)(lineSpacing * (outputLines + 1)), new Color(0, 0, 0, bgAlpha), true);
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

        public override void AcceptCommand(ContextCommand command)
        {
            Write("Attempting to change contexts to {0}", command.Name);
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
                names = ScriptBank.GetLoadedBehaviors();
            }
            else if (type == FileType.Spawn)
            {
                names = ScriptBank.GetLoadedSpawns();
            }
            else if (type == FileType.Trajectory)
            {
                names = ScriptBank.GetLoadedTrajectories();
            }
            else if (type == FileType.Body)
            {
                names = BodyBank.GetLoaded();
            }
            else if (type == FileType.Item)
            {
                names = ItemBank.GetLoaded();
            }
            else if (type == FileType.ItemBatch)
            {
                names = ItemBatchBank.GetLoaded();
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
                names = notImplemented;
            }
            else if (type == FileType.GibBatch)
            {
                names = notImplemented;
            }
            else if (type == FileType.Option)
            {
                names = OptionBank.GetLoaded();
            }
            else if (type == FileType.Font)
            {
                names = FontBank.GetLoaded();
            }
            else if (type == FileType.Model)
            {
                names = ModelBank.GetLoaded();
            }
            else if (type == FileType.Sfx)
            {
                names = SfxBank.GetLoaded();
            }
            else if (type == FileType.Song)
            {
                names = SongBank.GetLoaded();
            }
            else if (type == FileType.Texture)
            {
                names = TextureBank.GetLoaded();
            }
            else if (type == FileType.Sprite)
            {
                names = SpriteBank.GetLoaded();
            }
            else if (type == FileType.Player)
            {
                List<Player> customs = PlayerBank.GetAll(true);
                List<Player> standard = PlayerBank.GetAll(false);
                names = new List<string>();
                customs.ForEach(player => names.Add(string.Format("custom: {0}", player.Name)));
                standard.ForEach(player => names.Add(string.Format("standard: {0}", player.Name)));

            }
            else
            {
                string msg = string.Format("Did not expect type {0}", type);
                names = new List<string> { msg };
            }
            

            names.ForEach(name => message += name + "\n");
            Write(message);
        }

        public override void AcceptCommand(SpawnCommand spawn)
        {
            SpawnType type = spawn.Type;
            string name = spawn.Name;
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
                World.InsertEnemy(name);
            }
            else
            {
                Write("Did not expect the type {0}", type);
            }
        }
    }
}
