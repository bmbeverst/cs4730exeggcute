using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.graphics;
using Microsoft.Xna.Framework.Graphics;
using Exeggcute.src.assets;
using Microsoft.Xna.Framework;

namespace Exeggcute.src
{
    class WangMesh
    {
        private WangArray wangGrid;
        private int cols;
        private int rows;
        private Quad[,] Quads;
        private QuadBatch quadBatch;
        Random random = new Random();
        public float Width { get; protected set; }
        public float Height { get; protected set; }
        public WangMesh(GraphicsDevice graphics, TextureName texName, int rows, int cols, float size)
        {

            Texture2D texture = TextureBank.Get(texName);
            int texWidth = texture.Width;
            int texHeight = texture.Height;
            this.quadBatch = new QuadBatch(graphics, texName);
            this.cols = cols;
            this.rows = rows;
            Width = (cols - 1) * size;
            Height = rows * size;
            wangGrid = new WangArray(cols, rows);
            Quads = new Quad[cols, rows];
            int heightOffset = 50;
            Texture2D wang = TextureBank.Get(TextureName.wang8);
            for (int i = 0; i < cols; i += 1)
            {
                for (int j = 0; j < rows; j += 1)
                {
                    int index = wangGrid[i, j];
                    //HARDCODE
                    int tilesize = 32;
                    float height = random.Next() * 16 - 8.0f;
                    Quads[i, j] = new Quad(index, new Vector3(i * size - Width / 2, j * size - heightOffset, height), new Vector3(0, 0, 1), size, size, tilesize, tilesize, texWidth, texHeight);
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
        float yProgress;
        public void Draw(GraphicsDevice graphics, Matrix view, Matrix projection)
        {
            yProgress -= 0.01f;
            view *= Matrix.CreateTranslation(0, yProgress, 0);
            quadBatch.SetCamera(view, projection);
            for (int j = 0; j < rows; j += 1)
            {
                for (int i = 0 ; i < cols; i += 1)
                {
                
                    Quad quad = Quads[i, j];
                    quadBatch.Draw(graphics, quad);
                    //return;
                }
            }
        }
    }
}
