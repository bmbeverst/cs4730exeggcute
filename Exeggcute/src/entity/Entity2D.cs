﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Exeggcute.src.graphics;
using Exeggcute.src.input;
using Exeggcute.src.assets;

namespace Exeggcute.src.entity
{
    class Entity2D
    {

        public virtual Sprite Sprite { get; protected set; }
        public virtual Vector2 Position { get; protected set; }
        public virtual float X 
        { 
            get { return Position.X; } 
            set { Position = new Vector2(value, Position.Y); } 
        }
        public virtual float Y
        {
            get { return Position.Y; }
            set { Position = new Vector2(Position.X, value); }
        }

        public Entity2D(TextureName textureName, Vector2 pos)
        {
            Sprite = new StaticSprite(textureName, new Point(0, 0), 16, 16);
            Position = pos;
        }

        public virtual void Update()
        {

        }

        public virtual void Draw(SpriteBatch batch)
        {
            Sprite.Draw(batch, Position);
        }


    }
}
