﻿using System;
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
            //Shader.AmbientLightColor = new Vector3(0.5f, 0.5f, 0.9f);

            Shader.TextureEnabled = true;
            Shader.Texture = texture;
        }
        public void SetCamera(Matrix view, Matrix projection)
        {
            
            Shader.View = view;
            Shader.Projection = projection;
        }


        public void Draw(GraphicsDevice graphics, Wrapped2DArray<Quad> quads, float centerDepth, float rotation)
        {
            Shader.World = 
                  Matrix.CreateTranslation(0, 0, -centerDepth)
                * Matrix.CreateRotationX(-rotation)
                * Matrix.CreateTranslation(0, 0, centerDepth);
            foreach (EffectPass pass in Shader.CurrentTechnique.Passes)
            {
                pass.Apply();
                drawAll(graphics, quads);
            }
        }

        public void DrawNear(GraphicsDevice graphics, Wrapped2DArray<Quad> quads, float centerDepth, float rotation, int start, int buffer)
        {
            Shader.World =
                  Matrix.CreateTranslation(0, 0, -centerDepth)
                * Matrix.CreateRotationX(-rotation)
                * Matrix.CreateTranslation(0, 0, centerDepth);
            foreach (EffectPass pass in Shader.CurrentTechnique.Passes)
            {
                pass.Apply();
                drawRange(graphics, quads, start, buffer);
            }
            
        }

        protected void drawAll(GraphicsDevice graphics, Wrapped2DArray<Quad> quads)
        {
            for (int j = 0; j < quads.Rows; j += 1)
            {
                for (int i = 0; i < quads.Cols; i += 1)
                {
                    Quad quad = quads[i, j];
                    graphics.DrawUserIndexedPrimitives
                    <VertexPositionNormalTexture>(
                        PrimitiveType.TriangleList,
                        quad.Vertices, 0, 4,
                        quad.Indexes, 0, 2);
                }
            }
        }

        protected void drawRange(GraphicsDevice graphics, Wrapped2DArray<Quad> quads, int start, int buffer)
        {
            for (int j = start - buffer; j <= start + buffer; j += 1)
            {
                for (int i = 0; i < quads.Cols; i += 1)
                {
                    Quad quad = quads[i, j];
                    graphics.DrawUserIndexedPrimitives
                    <VertexPositionNormalTexture>(
                        PrimitiveType.TriangleList,
                        quad.Vertices, 0, 4,
                        quad.Indexes, 0, 2);
                }
                
            }
        }
    }
}