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
        public BasicEffect Shader { get; protected set; }
        public QuadBatch(GraphicsDevice graphics, TextureName texName)
        {
            Shader = new BasicEffect(graphics);
            //Shader.EnableDefaultLighting();
            Shader.World = Matrix.Identity;
            Shader.TextureEnabled = true;
            Shader.Texture = TextureBank.Get(texName);
        }
        public void SetCamera(Matrix view, Matrix projection)
        {
            Shader.View = view;
            Shader.Projection = projection;
        }
        public void Draw(GraphicsDevice graphics, Quad quad)
        {
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
