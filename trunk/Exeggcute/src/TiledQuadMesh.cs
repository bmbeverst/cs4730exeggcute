using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Exeggcute.src.graphics;
using Exeggcute.src.assets;
using Microsoft.Xna.Framework;

namespace Exeggcute.src
{
    class TiledQuadMesh
    {
        Texture2D tileTextures;
        VertexPositionNormalTexture[][] listOfVertices;
        short[][] listOfIndices;
        Dictionary<int, int> frequencies = new Dictionary<int,int>();

        Quad[,] quads;
        QuadBatch quadBatch;
        float meshWidth;
        float meshHeight;
        float quadWidth;
        float quadHeight;
        int numDistinctTiles;

        float yOffset;
        float yScrollSpeed;
        float depth;

        public TiledQuadMesh(GraphicsDevice graphics,
            TextureName textureName,
            byte[,] textureIndices, 
            int cols, 
            int rows,  
            int numDistinctTiles,
            float quadWidth,
            float quadHeight )
        {
            this.quadBatch = new QuadBatch(graphics, textureName);
            this.tileTextures = TextureBank.Get(textureName);
            int textureWidth = tileTextures.Width;
            int textureHeight = tileTextures.Height;
            this.listOfVertices = new VertexPositionNormalTexture[numDistinctTiles][];
            this.listOfIndices = new short[numDistinctTiles][];
            this.quads = new Quad[cols, rows];

            this.meshWidth = cols*quadWidth;
            this.meshHeight = rows*quadHeight;
            this.quadWidth = quadWidth;
            this.quadHeight = quadHeight;
            this.numDistinctTiles = numDistinctTiles;

            for (int k = 0; k < numDistinctTiles; k += 1)
            {
                frequencies[k] = 0;
            }
            for (int i = 0; i < cols; i += 1)
            {
                for (int j = 0; j < rows; j += 1)
                {
                    int index = textureIndices[i,j];
                    frequencies[index] += 1;
                }
            }
            for (int k = 0; k < numDistinctTiles; k += 1)
            {
                int freq = frequencies[k];
                if (freq == 0) throw new ExeggcuteError("For now, all tiles must be represented");
                listOfVertices[k] = new VertexPositionNormalTexture[freq*4];
                listOfIndices[k] = new short[freq*6];
            }

            Dictionary<int,int> counts = new Dictionary<int,int>();
            for (int k = 0; k < numDistinctTiles; k += 1)
            {
                counts[k] = 0;
            }

            for (int i = 0; i < cols; i += 1)
            {
                for (int j = 0; j < rows; j += 1)
                {
                    int tIndex = textureIndices[i,j];
                    int count = counts[tIndex];
                    VertexPositionNormalTexture[] vertexHandle = listOfVertices[tIndex];
                    short[] indexHandle = listOfIndices[tIndex];


                    Vector3 pos = getPosition(i, j);
                    //quads[i, j] = new Quad(tIndex, count, pos, new Vector3(0, 0, 1), quadWidth, quadHeight, 32, 32, textureWidth, textureHeight, vertexHandle, indexHandle);


                    counts[tIndex] += 1;
                }
            }
            
        }

        private Vector3 getPosition(int i, int j)
        {
            float x = i * quadWidth - meshWidth / 2;
            float y = j * quadHeight;
            float z = getHeight(i, j);
            return new Vector3(x, y, z);
        }
        private float getHeight(int i, int j)
        {
            return 0.0f;
        }

        public void Update()
        {
            yOffset += yScrollSpeed;
        }

        public void Draw(GraphicsDevice graphics, Matrix view, Matrix projection)
        {

            quadBatch.Shader.World = Matrix.CreateTranslation(0, yOffset, depth);
            foreach (EffectPass pass in quadBatch.Shader.CurrentTechnique.Passes)
            {
                pass.Apply();
                for (int k = 0; k < numDistinctTiles; k += 1)
                {
                    VertexPositionNormalTexture[] vertices = listOfVertices[k];
                    short[] indices = listOfIndices[k];
                    graphics.DrawUserIndexedPrimitives
                    <VertexPositionNormalTexture>(
                        PrimitiveType.TriangleList,
                        vertices, 0, vertices.Length,
                        indices, 0, vertices.Length/2);
                }
            }
        }



        
    }
}
