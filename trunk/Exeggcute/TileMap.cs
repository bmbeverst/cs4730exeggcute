using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Exeggcute.src.assets;
using Exeggcute.src.graphics;

namespace Exeggcute
{
    class TileMap
    {
        public Texture2D Texture { get; protected set; }
        public int TileWidth { get; protected set; }
        public int TileHeight { get; protected set; }
        public int TextureWidth 
        {
            get { return Texture.Width; }
        }
        public int TextureHeight
        {
            get { return Texture.Height; }
        }
        public float TexelWidth
        {
            get { return 1.0f / TextureWidth; }
        }
        public float TexelHeight
        {
            get { return 1.0f / TextureHeight; }
        }

        public TileMap(TextureName textureName, int tileHeight, int tileWidth)
        {
            Texture = TextureBank.Get(textureName);
            TileWidth = tileWidth;
            TileHeight = tileHeight;
        }

        public Point GetPoint(int index)
        {
            int i = (index * TileWidth) % TextureWidth;
            int j = (index * TileWidth) / TextureWidth;
            return new Point(i, j);
        }

        public Vector2 GetUpperLeft(int i, int j)
        {
            return new Vector2(i * TexelWidth + 0.01f, j * TexelHeight);
        }

        public Vector2 GetLowerRight(Vector2 upperLeft)
        {
            return new Vector2(upperLeft.X + TileWidth * TexelWidth - 0.02f, upperLeft.Y + TileHeight * TexelHeight);
        }

        public Quad CreateQuad(int index, Vector3 position, Vector3 back, float quadWidth, float quadHeight)
        {
            Point indexPoint = GetPoint(index);
            int i = indexPoint.X;
            int j = indexPoint.Y;
            Vector2 upperLeft = GetUpperLeft(i, j);
            Vector2 lowerRight = GetLowerRight(upperLeft);
            return new Quad(upperLeft, lowerRight, position, back, quadWidth, quadHeight);
        }
    }
}
