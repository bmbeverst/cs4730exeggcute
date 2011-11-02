using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Exeggcute.src.graphics;

namespace Exeggcute.src.gui
{
    class ScoreMenu : Menu
    {
        protected ScoreSet scores;
        public ScoreMenu(List<Button> buttons, WangMesh terrain, Rectangle bounds)
            : base(buttons, bounds, terrain, false)
        {
            this.scores = new ScoreSet();
            loadScores();
        }
        
        private void loadScores()
        {
            scores.LoadLocal();
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
            scores.WriteNetwork();
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

        public override void Draw3D(GraphicsDevice graphics, Camera camera)
        {
            base.Draw3D(graphics, camera);
        }

        public override void Draw2D(SpriteBatch batch)
        {
            scores.Draw(batch, font, Vector2.Zero, Color.White);
            base.Draw2D(batch);
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
            ResolveCursor();
        }

        public override void Unload()
        {
            cursor = 0;
            scores.ViewingNetwork = false;
        }

        public override void Back()
        {
            cursor = 0;
            Worlds.World.Back();
        }
    }


}
