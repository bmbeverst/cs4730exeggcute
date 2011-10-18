﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.graphics;
using Microsoft.Xna.Framework.Graphics;
using Exeggcute.src.assets;
using Microsoft.Xna.Framework;
using Exeggcute.src.gui;

namespace Exeggcute.src
{
    /// <summary>
    /// A polygonal mesh tiled aperiodically by Wang tiles used for 
    /// scrolling background.
    /// </summary>
    class WangMesh
    {
        private WangArray wangGrid;
        private int cols;
        private int rows;
        private Quad[,] Quads;
        private QuadBatch quadBatch;
        Random rng = new Random();
        public float Width { get; protected set; }
        public float Height { get; protected set; }
        public float Speed { get; protected set; }
        public float Depth { get; protected set; }
        public float TileSize { get; protected set; }
        public float ProgressY { get; protected set; }

        public WangMesh(GraphicsDevice graphics, TextureName texName, int cols, int rows, float size, float heightVariance, float scrollSpeed)
        {

            Texture2D wangTexture = TextureBank.Get(TextureName.wang8);
            int texWidth = wangTexture.Width;
            int texHeight = wangTexture.Height;
            this.quadBatch = new QuadBatch(graphics, texName);
            this.cols = cols;
            this.rows = rows;
            Speed = scrollSpeed;
            Width = (cols - 1) * size;
            Height = rows * size;
            Depth = 0;
            TileSize = size;
            wangGrid = new WangArray(cols, rows);
            Quads = new Quad[cols, rows];
            ProgressY = -100;
            
            for (int i = 0; i < cols; i += 1)
            {
                for (int j = 0; j < rows; j += 1)
                {
                    int index = wangGrid[i, j];
                    //HARDCODE
                    int tilesize = 32;
                    float height = Depth;// random.Next() * heightVariance - heightVariance / 2;
                    Quads[i, j] = new Quad(index, 0, new Vector3(i * size - Width / 2, j * size, height), new Vector3(0, 0, 1), size, size, tilesize, tilesize, texWidth, texHeight);
                }
            }

            lockEdges();
            
        }

        private void lockEdges()
        {
            for (int i = 0; i < cols; i += 1)
            {
                for (int j = 0; j < rows; j += 1)
                {
                    Quad left;
                    Quad above;

                    if (i <= 0) left = null;
                    else left  = Quads[i - 1, j];

                    if (j <= 0) above = null;
                    else above = Quads[i, j - 1];

                    Quads[i, j].Lock(left, above);
                }
            }
        }

        private void lockLocal(int xIndex, int yIndex)
        {
            int xMin = Math.Max(0, xIndex);
            int xMax = Math.Min(cols - 1, xIndex + 2);

            int yMin = Math.Max(0, yIndex);
            int yMax = Math.Min(rows - 1, yIndex + 2);

            for (int i = xMin; i < xMax; i += 1)
            {
                for (int j = yMin; j < yMax; j += 1)
                {
                    Quad left;
                    Quad above;

                    if (i <= 0) left = null;
                    else left = Quads[i - 1, j];

                    if (j <= 0) above = null;
                    else above = Quads[i, j - 1];

                    Quads[i, j].Lock(left, above);
                }
            }
        }

        int frame = 0;
        public void Update()
        {
            frame += 1;
            if (frame % 10 == 0 || true)
            {
                for (int i = 0; i < cols; i += 1)
                {
                    for (int j = 0; j < rows; j += 1)
                    {
                        Util.Die("d");
                        Quad current = Quads[i, j];
                        float curZ = current.UpperRight.Z;
                        float newZ = Depth + rng.Next() * 8 - 4;
                        //Console.WriteLine("{0},{1}", i, j);
                        float x = current.UpperRight.X;
                        float y = current.UpperRight.Y;
                        Quads[i, j].UpdateVertices(current.UpperLeft, new Vector3(x, y, newZ), current.LowerLeft, current.LowerRight);
                        
                    }
                }
                //Util.Die("");
                  
                
            }
        }

        public void Impact(float x, float y, float mass, float speed)
        {
            int i = Math.Abs((int)((x + Level.HalfWidth*(3f/2f)) / TileSize));
            int j = Math.Abs((int)((y - ProgressY) / TileSize));
            if (i < 0 || i > cols - 1 || j < 0 || j > rows - 1) return;
            Quad current = Quads[i, j];
            float curZ = current.UpperRight.Z;
            float newZ = curZ - rng.Next() * 8;
            //Console.WriteLine("{0},{1}", i, j);
            float xQuad = current.UpperRight.X;
            float yQuad = current.UpperRight.Y;
            Quads[i, j].UpdateVertices(current.UpperLeft, new Vector3(xQuad, yQuad, newZ), current.LowerLeft, current.LowerRight);
            lockLocal(i, j);
        }

        //TODO
        /// <summary>
        /// TODO: Make wrap into a cylinder for automagic infinite scrolling!
        /// </summary>
        public void Draw(GraphicsDevice graphics, Matrix view, Matrix projection)
        {
            ProgressY -= 10*Speed;
            quadBatch.SetCamera(view, projection);
            for (int j = 0; j < rows; j += 1)
            {
                for (int i = 0 ; i < cols; i += 1)
                {
                
                    Quad quad = Quads[i, j];
                    
                    quadBatch.Draw(graphics, quad, ProgressY);
                    //return;
                }
            }
        }

    }
}
