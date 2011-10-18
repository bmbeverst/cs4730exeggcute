using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Exeggcute.src.graphics
{
    class Quad
    {
        public Vector3 Origin;
        public Vector3 UpperLeft;
        public Vector3 LowerLeft;
        public Vector3 UpperRight;
        public Vector3 LowerRight;
        public Vector3 Normal;
        public Vector3 Up;
        public Vector3 Left;
        Vector2 textureUpperLeft;
        Vector2 textureUpperRight;
        Vector2 textureLowerLeft;
        Vector2 textureLowerRight;
        public VertexPositionNormalTexture[] Vertices;
        public short[] Indexes;
        public float Width { get; protected set; }
        public float Height { get; protected set; }

        public Quad(int textureIndex,
                    Vector3 origin,
                    Vector3 back,
                    float width,
                    float height,
                    int tileTexelWidth,
                    int tileTexelHeight,
                    int textureWidth,
                    int textureHeight)
        {
            Width = width;
            Height = height;
            int i = (textureIndex * tileTexelWidth) % textureWidth;
            int j = (textureIndex * tileTexelWidth) / textureWidth;
            float texelWidth = 1.0f / (float)textureWidth;
            float texelHeight = 1.0f / (float)textureHeight;
            Vector2 texUpLeft = new Vector2(i * texelWidth + 0.01f, j * texelHeight);
            Vector2 texLowRight = new Vector2(texUpLeft.X + tileTexelWidth * texelWidth - 0.02f, texUpLeft.Y + tileTexelHeight * texelHeight);
            initQuad(texUpLeft, texLowRight, origin, back);
        }

        public Quad(Vector2 topleft,
            Vector2 bottomRight,
            Vector3 position,
            Vector3 back,
            float width,
            float height)
        {
            Width = width;
            Height = height;
            initQuad(topleft, bottomRight, position, back);
        }
        public void initQuad(Vector2 texUpLeft, Vector2 texLowRight, Vector3 origin, Vector3 back)
        {
            Vertices = new VertexPositionNormalTexture[4];
            Indexes = new short[6]; ;
            Origin = origin;
            Normal = back;
            Up = Vector3.Up;
            textureUpperLeft = texUpLeft;
            textureUpperRight = new Vector2(texLowRight.X, texUpLeft.Y);
            textureLowerLeft = new Vector2(texUpLeft.X, texLowRight.Y);
            textureLowerRight = texLowRight;
            // Calculate the quad corners
            Left = Vector3.Cross(Normal, Up);
            Vector3 uppercenter = (Up * Height / 2) + Origin;
            UpperLeft = uppercenter + (Left * Width / 2);
            UpperRight = uppercenter - (Left * Width / 2);
            LowerLeft = UpperLeft - (Up * Height);
            LowerRight = UpperRight - (Up * Height);
            FillVertices();

        }

        public void UpdateVertices(Vector3 upperleft, Vector3 upperright, Vector3 lowerleft, Vector3 lowerright)
        {
            
            UpperLeft = upperleft;
            UpperRight = upperright;
            LowerLeft = lowerleft;
            LowerRight = lowerright;
            Vector3 right = UpperLeft - UpperRight;
            Vector3 down = UpperRight - LowerRight;
            Normal = Vector3.Cross(down, right);

            Normal.Normalize();
            FillVertices();
        }
        public void UpdateVertices()
        {
            Vector3 right = UpperLeft - UpperRight;
            Vector3 down = UpperRight - LowerRight;
            Normal = Vector3.Cross(down, right);
            Normal.Normalize();
            FillVertices();
        }

        public void Lock(Quad left, Quad below)
        {

            bool lockLeft = left != null;
            bool lockBelow = below != null;

            if (lockLeft)
            {
                UpperLeft = left.UpperRight;
                LowerLeft = left.LowerRight;
                
            }
            if (lockBelow)
            {
                LowerLeft = below.UpperLeft;
                LowerRight = below.UpperRight;
            }
            
            UpdateVertices();
            
        }
        private void FillVertices()
        {
            // Provide a normal for each vertex
            for (int i = 0; i < Vertices.Length; i++)
            {
                Vertices[i].Normal = Normal;
            }

            // Set the position and texture coordinate for each
            // vertex
            Vertices[0].Position = LowerLeft;
            Vertices[0].TextureCoordinate = textureLowerLeft;

            Vertices[1].Position = UpperLeft;
            Vertices[1].TextureCoordinate = textureUpperLeft;

            Vertices[2].Position = LowerRight;
            Vertices[2].TextureCoordinate = textureLowerRight;

            Vertices[3].Position = UpperRight;
            Vertices[3].TextureCoordinate = textureUpperRight;
            // Set the index buffer for each vertex, using
            // clockwise winding
            Indexes[0] = 0;
            Indexes[1] = 1;
            Indexes[2] = 2;
            Indexes[3] = 2;
            Indexes[4] = 1;
            Indexes[5] = 3;

        }


        
    }
}
