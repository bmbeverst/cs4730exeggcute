using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Exeggcute.src.contexts;
using Exeggcute.src.input;

namespace Exeggcute.src.gui
{
    class PlayerButton : Button
    {
        public string RawData { get; protected set; }
        public string Name { get; protected set; }
        public bool IsCustom { get; protected set; }

        public override int Height
        {
            get { return 25; }
        }

        protected static Vector2 dataPos = new Vector2(500, 100);
        protected SpriteFont font;
        protected Color fontColor;
        public PlayerButton(Player player, SpriteFont font, Color fontColor)
            : base(null, null, null, null)
        {
            
            this.RawData = player.RawData;
            this.Name = player.Name;
            this.font = font;
            this.fontColor = fontColor;
            this.IsCustom = player.IsCustom;
            this.onActivate = new LoadLevelEvent("0", Name, IsCustom);
        }

        public override void Update(ControlManager controls)
        {
            IsActive = true;
            if (controls[Ctrl.Action].DoEatPress())
            {
                onActivate.Process();
            }
            else if (controls[Ctrl.Cancel].DoEatPress())
            {
                World.Back();
            }
            base.Update(controls);
        }

        public override void Draw(SpriteBatch batch, Vector2 pos)
        {
            if (IsActive)
            {
                
                batch.DrawString(font, RawData, dataPos, fontColor);
            }
            batch.DrawString(font, Name, pos, fontColor);
            //Console.WriteLine("{0} {1}", pos, dataPos);
        }



        protected override void moveUp()
        {
            World.SendMove(Direction.Up);
            IsActive = false;
        }

        protected override void moveDown()
        {
            World.SendMove(Direction.Down);
            IsActive = false;
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
