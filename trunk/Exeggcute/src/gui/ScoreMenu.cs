using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Exeggcute.src.assets;
using Microsoft.Xna.Framework;
using Exeggcute.src.contexts;
using Exeggcute.src.graphics;

namespace Exeggcute.src.gui
{
    class ScoreMenu : Menu
    {
        protected ScoreSet scores;
        protected SpriteFont font;
        protected Rectangle buttonBox;
        protected Doodad buttonBoxOutline;
        protected List<Vector2> drawPositions = new List<Vector2>();
        protected int vertButtonSpacing;
        protected Sprite cursorSprite;
        public ScoreMenu(FontName fontName)
            : base(null, false)
        {
            
            this.font = FontBank.Get(fontName);
            this.buttons = makeButtons();
            this.scores = new ScoreSet();
            this.cursorSprite = new StaticSprite(TextureName.cursor, new Point(0, 0), 16, 16);
            
            loadScores();

            int width = 100;
            int height = 100;
            int bottomOffset = Engine.YRes / 4 + height;
            buttonBox = new Rectangle((Engine.XRes - width) / 2, Engine.YRes - bottomOffset, width, height);
            buttonBoxOutline = new Doodad(buttonBox, Color.Black, false);
            vertButtonSpacing = buttonBox.Height / buttons.Count;
            for (int i = 0; i < buttons.Count; i += 1)
            {
                drawPositions.Add(new Vector2(buttonBox.Left, buttonBox.Top + vertButtonSpacing * i));
            }
        }
        
        private void loadScores()
        {
            scores.LoadLocal();
        }

        private static List<Button> getButtons()
        {
            return null;
        }

        public void ShowLocal()
        {
            scores.ViewingNetwork = false;
        }

        public void ShowNetwork()
        {
            scores.ViewingNetwork = true;
            scores.LoadNetwork();
        }

        public void SyncNetwork()
        {
            scores.LoadNetwork();
            // TODO 
            // Mike suggests the following:
            // 1) Hard code a list of high scores on the network in the database.
            // 2) Write a way to merge two ScoreEntry[], taking the top ten
            //    scores from both.
            // 3) Request the data from the network, then convert it to a ScoreEntry[].
            // 4) Merge the ScoreEntry[]'s for the network data and our networkScores.networkScores
            // 5) If we have a value that should be added to the network, send the data back to the
            //    server.
        }

        public override void Draw(GraphicsDevice graphics, SpriteBatch batch)
        {
            batch.Begin();
            scores.Draw(batch, font, new Vector2(0,0), Color.Black);
            for (int i = 0; i < buttons.Count; i += 1)
            {
                buttons[i].Draw(batch, drawPositions[i]);
            }
            cursorSprite.Draw(batch, new Vector2(buttonBox.Left - cursorSprite.Width, buttonBox.Top + vertButtonSpacing * cursor));
            
            batch.End();
        }
        public override void Move(Direction dir)
        {
            if (dir == Direction.Up)
            {
                cursor -= 1;
            }
            else if (dir == Direction.Down)
            {
                cursor += 1;
            }
            resolveCursor();
        }
        protected List<Button> makeButtons()
        {
            Color fontColor = Color.Black;
            List<Button> buttons = new List<Button> {
                new ListButton(new ScoreEvent(ScoreEventType.SeeLocal), new SpriteText(font, "View Local", fontColor)),
                new ListButton(new ScoreEvent(ScoreEventType.SeeNetwork), new SpriteText(font, "View Network", fontColor)),
                new ListButton(new ScoreEvent(ScoreEventType.Submit), new SpriteText(font, "Submit", fontColor)),

                new ListButton(new BackEvent(), new SpriteText(font, "Back", fontColor)),



            };
            
            return buttons;
        }

        public override void Unload()
        {
            cursor = 0;
            scores.ViewingNetwork = false;
        }
        
    }


}
