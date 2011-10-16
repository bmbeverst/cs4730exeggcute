using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Exeggcute.src.assets;
using Exeggcute.src.input;

namespace Exeggcute.src.graphics
{

    /// <summary>
    /// Class for procedurally generating background meshes
    /// </summary>
    class TerrainMesh
    {

        private float cellSize;
        //private VertexPositionColor[] vertices;
        //private VertexPositionNormalTexture[][] vertices;
        private VertexPositionNormalTexture[] vertices;

        private int[] indices;
        private Texture2D texture;
        private Effect effect;
        private int vertexCols;
        private int vertexRows;
        private float meshWidth;
        private float meshHeight;


        private WangArray wangGrid;

        // based off http://www.toymaker.info/Games/XNA/html/xna_terrain.html#create
        public TerrainMesh(GraphicsDevice graphics, EffectName effectName, float cellSize, int cols, int rows)
        {
            this.effect = EffectBank.Get(effectName);
            this.texture = TextureBank.Get(TextureName.bg);
            vertexCols = cols;
            vertexRows = rows;
            wangGrid = new WangArray(vertexCols - 1, vertexRows - 1);
            this.cellSize = cellSize;
            meshWidth = (vertexCols - 1) * cellSize;
            meshHeight = (vertexRows - 1) * cellSize;
            if (effect.CurrentTechnique.Passes.Count <= 0)
            {
                throw new ExeggcuteError("terrain mesh effect must have at least one pass");
            }
            //vertices = new VertexPositionNormalTexture[8][];


            SetUpVertices();
            SetUpIndices();
        }
        private void SetUpVertices()
        {
            vertices = new VertexPositionNormalTexture[vertexCols * vertexRows];
            /*for (int i = 0; i < wangGrid.WangCount; i += 1)
            {
                vertices[i] = new VertexPositionNormalTexture[vertexCols * vertexRows];
            }*/
            int count = 0;
            for (int i = 0; i < vertexCols; i++)
            {
                for (int j = 0; j < vertexRows; j++)
                {
                    vertices[count].Position = new Vector3(i * cellSize - meshWidth / 2, -j * cellSize, 0);
                    vertices[count].Normal = Vector3.Backward;
                    vertices[count].TextureCoordinate = new Vector2(
                        (float)(i*32) / (vertexCols - 1),
                        (float)(j*32) / (vertexRows - 1));
                    
                    count += 1;
                }
            }
        }

        private void SetUpIndices()
        {
            indices = new int[(vertexCols - 1) * (vertexRows - 1) * 6];
            int counter = 0;
            for (int j = 0; j < vertexRows - 1; j += 1)
            {
                for (int i = 0; i < vertexCols - 1; i += 1)
                {
                    int lowerLeft = i + j * vertexCols;
                    int lowerRight = (i + 1) + j * vertexCols;
                    int topLeft = i + (j + 1) * vertexCols;
                    int topRight = (i + 1) + (j + 1) * vertexCols;

                    indices[counter] = topLeft;
                    indices[counter+1] = lowerRight;
                    indices[counter+2] = lowerLeft;
                    counter += 3;

                    indices[counter] = topLeft;
                    indices[counter+1] = topRight;
                    indices[counter+2] = lowerRight;
                    counter += 3;

                }
            }
        }

        float t1;
        float t2;
        float t3;
        public void Update(ControlManager controls)
        {
            if (controls[Ctrl.LShoulder].IsPressed)
            {
                t1 += 0.1f;
            }
            else if (controls[Ctrl.RShoulder].IsPressed)
            {
                t1 -= 0.1f;
            }

            if (controls[Ctrl.Left].IsPressed)
            {
                t2 += 0.1f;
            }
            else if (controls[Ctrl.Right].IsPressed)
            {
                t2 -= 0.1f;
            }

            if (controls[Ctrl.Up].IsPressed)
            {
                t3 += 0.1f;
            }
            else if (controls[Ctrl.Down].IsPressed)
            {
                t3 -= 0.1f;
            }
        }
        public void Draw(GraphicsDevice graphics, Matrix view, Matrix projection)
        {

            RasterizerState rs = new RasterizerState();
            rs.CullMode = CullMode.None;
            //rs.FillMode = FillMode.WireFrame;
            graphics.RasterizerState = rs;

            effect.CurrentTechnique = effect.Techniques["PointSprites"];
            effect.Parameters["xView"].SetValue(view);
            effect.Parameters["xProjection"].SetValue(projection);
            Matrix worldMatrix = Matrix.CreateRotationZ(t3) *
                Matrix.CreateRotationX(t1) *
                Matrix.CreateRotationY(t2) *
                Matrix.CreateTranslation(new Vector3(0, 0, 0));
            
            effect.Parameters["xWorld"].SetValue(worldMatrix);
            int count = 0;
            for (int i = 0; i < wangGrid.Cols; i += 1)
            {
                for (int j = 0; j < wangGrid.Rows; j += 1)
                {
                    
                    effect.Parameters["xTexture"].SetValue(texture);
                    foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                    {
                        pass.Apply();
                        graphics.DrawUserIndexedPrimitives(
                            PrimitiveType.TriangleList,
                            vertices, count, 6,//vertices.Length,
                            indices, count*6, 2);//indices.Length / 3);

                    }
                    count += 1;
                }
            }

            
        }
    }
}
