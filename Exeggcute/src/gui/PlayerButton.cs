using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.assets;
using Exeggcute.src.contexts;
using Exeggcute.src.entities;
using Exeggcute.src.input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Exeggcute.src.gui
{
    class PlayerButton : ListButton
    {
        public string RawData { get; protected set; }
        public string Name { get; protected set; }

        public override int Height
        {
            get { return 25; }
        }

        protected static Vector2 dataPos = new Vector2(500, 100);
        protected SpriteFont font;
        protected Color fontColor;
        protected Player player;
        public PlayerButton(Player player, SpriteFont font, Color fontColor)
            : base(null, null)
        {
            
            this.RawData = player.RawData;
            this.Name = player.Name;
            this.font = font;
            this.fontColor = fontColor;
            this.onActivate = new LoadLevelEvent("0", Name);
            this.player = player;
        }

        public override void Update(ControlManager controls)
        {
            if (!IsActive)
            {
                IsActive = true;
                player.DoDemo();
            }
            if (controls[Ctrl.Action].DoEatPress())
            {
                onActivate.Process();
            }
            else if (controls[Ctrl.Cancel].DoEatPress())
            {
                Assets.Sfx["back"].Play();
                parent.Back();
            }
            base.Update(controls);
        }

        public override void Draw(SpriteBatch batch, Vector2 pos)
        {
            if (IsActive)
            {
                
                //batch.DrawString(font, RawData, dataPos, fontColor);
            }
            batch.DrawString(font, Name, pos, fontColor);
        }



        protected override void moveUp()
        {
            World.SendMove(Direction.Up);
            IsActive = false;
            player.ResetFromDemo();
        }

        protected override void moveDown()
        {
            World.SendMove(Direction.Down);
            IsActive = false;
            player.ResetFromDemo();
        }

        protected override void moveLeft()
        {
            //throw new NotImplementedException();
        }

        protected override void moveRight()
        {
            //throw new NotImplementedException();
        }

    }
}
