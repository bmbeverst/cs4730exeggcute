using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Exeggcute.src.assets;

namespace Exeggcute.src.graphics
{
    class QuadBatch
    {
        public BasicEffect Shader { get; set; }
        public QuadBatch(GraphicsDevice graphics, Texture2D texture)
        {
            //Shader = EffectBank.Get(EffectName.terrain);
            Shader = new BasicEffect(graphics);
            //Shader.EnableDefaultLighting();
            Shader.World = Matrix.Identity;
            Shader.AmbientLightColor = new Vector3(0.5f, 0.5f, 0.9f);

            Shader.TextureEnabled = true;
            Shader.Texture = texture;
        }
        public void SetCamera(Matrix view, Matrix projection)
        {
            
            Shader.View = view;
            Shader.Projection = projection;
        }
        public void Draw(GraphicsDevice graphics, Quad quad, float centerDepth, float rotation)
        {
            Shader.World = 
                  Matrix.CreateTranslation(0, 0, -centerDepth)
                * Matrix.CreateRotationX(-rotation)
                * Matrix.CreateTranslation(0, 0, centerDepth);
            foreach (EffectPass pass in Shader.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphics.DrawUserIndexedPrimitives
                <VertexPositionNormalTexture>(
                    PrimitiveType.TriangleList,
                    quad.Vertices, 0, 4,
                    quad.Indexes, 0, 2);
            }
        }
    }
}
