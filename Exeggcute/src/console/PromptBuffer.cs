using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Exeggcute.src.console
{
    class PromptBuffer
    {
        public bool HasFocus { get; protected set; }

        protected string buffer = "";
        protected List<string> history = new List<string>();
        protected int historyPtr = -1;
        protected int cursor = 0;
        public IContext Parent { get; protected set; }
        protected Keyflag backFlag = new Keyflag();
        protected Keyflag leftFlag = new Keyflag();
        protected Keyflag rightFlag = new Keyflag();
        protected Keyflag deleteFlag = new Keyflag();
        protected DevConsole console;

        protected string prompt;
        public PromptBuffer(DevConsole console, string prompt)
        {
            this.console = console;
            this.prompt = prompt;
        }

        public void Update(ControlManager controls)
        {

        }
        private string tryDeleteAt(string s, int i)
        {
            return s.Remove(i, 1);
        }

        protected void processBack(KeyboardManager kb)
        {
            backFlag.Update(kb.IsKeyPressed(Keys.Back));
            leftFlag.Update(kb.IsKeyPressed(Keys.Left));
            rightFlag.Update(kb.IsKeyPressed(Keys.Right));
            deleteFlag.Update(kb.IsKeyPressed(Keys.Delete));

        }

        private void moveCursor()
        {
            isZipping = true;
            if (backFlag.CheckZip(zipSpeed))
            {
                if (buffer.Length != 0)
                {
                    cursor -= 1;
                    buffer = tryDeleteAt(buffer, cursor);
                }
            }
            else if (deleteFlag.CheckZip(zipSpeed))
            {
                if (cursor < buffer.Length)
                {
                    buffer = tryDeleteAt(buffer, cursor);
                }
            }
            else if (rightFlag.CheckZip(zipSpeed))
            {
                cursor += 1;
                if (cursor > buffer.Length) cursor = buffer.Length;
            }
            else if (leftFlag.CheckZip(zipSpeed))
            {
                cursor -= 1;
                if (cursor < 0) cursor = 0;
            }
            else
            {
                isZipping = false;
            }

        }
        int cursorBlink = 0;
        bool drawCursor;
        int zipSpeed = 30;
        bool isZipping;
        public void Update(KeyboardManager kb)
        {
            cursorBlink += 1;
            if (isZipping) cursorBlink = 0;
            drawCursor = (cursorBlink % 60) < 30;
            processBack(kb);
            moveCursor();
            if (kb.PressedThisFrame.Length <= 0) return;

            Keys key = kb.PressedThisFrame[0];
            if (key == Keys.Enter)
            {
                if (Util.IsWhitespace(buffer))
                {
                    buffer = "";
                    
                }
                else
                {
                    console.InputCommand(buffer);
                    history.Insert(0, buffer);
                }
                historyPtr = -1;
                buffer = "";
                cursor = 0;
            }
            else if (key == Keys.Up)
            {
                if (history.Count == 0) return;
                if (history.Count > historyPtr)
                {
                    historyPtr += 1;
                    if (historyPtr == history.Count) historyPtr -= 1;
                    buffer = history[historyPtr];
                }
                else
                {
                    buffer = history[historyPtr];
                    historyPtr = history.Count - 1;
                }
                cursor = buffer.Length;
            }
            else if (key == Keys.Down)
            {
                if (history.Count == 0) return;
                if (historyPtr > 0)
                {
                    historyPtr -= 1;
                    buffer = history[historyPtr];
                }
                else
                {
                    buffer = "";
                    historyPtr = -1;
                }
                cursor = buffer.Length;
            }
            else if (key == Keys.Tab)
            {
                console.Resize();
            }
            else if (kb.IsPrintable(key))
            {
                char pressed = kb.GetPrintable(key);
                buffer = buffer.Insert(cursor, pressed + "");
                cursor += 1;
            }
        }

        public void AddToBuffer(string message)
        {
            buffer += message;
            cursor += message.Length;
        }

        public void Draw(SpriteBatch batch, SpriteFont font, Vector2 pos)
        {
            float charWidth = font.MeasureString("a").X;
            string combined = string.Format("{0}{1}", prompt, buffer);
            batch.DrawString(font, combined, pos, Color.White);
            /*for(int i = 0 ; i < history.Count; i += 1)
            {
                string message = string.Format("             {0}", history[i]);
                batch.DrawString(font, message, pos + new Vector2(0, -22*(i+1)), Color.White);
            }*/
            if (drawCursor) batch.DrawString(font, "|", pos + new Vector2(cursor *charWidth - charWidth/2 + charWidth * prompt.Length , 0), Color.White);
        }


    }
}
