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
        public int Index { get; protected set; }

        public Quad(int textureIndex, 
                    int quadIndex, 
                    Vector3 origin, 
                    Vector3 back, 
                    float width, 
                    float height, 
                    int tileWidth, 
                    int tileHeight, 
                    int texWidth, 
                    int texHeight/*,
                    VertexPositionNormalTexture[] verticesHandle*/)
        {
            Width = width;
            Height = height;
            Index = quadIndex;

            int i = (textureIndex * tileWidth) % texWidth;
            int j = (textureIndex * tileWidth) / texWidth;
            float texelWidth = 1.0f / (float)texWidth;
            float texelHeight = 1.0f / (float)texHeight;
            Vector2 texUpLeft = new Vector2(i*texelWidth + 0.01f, j*texelHeight);
            Vector2 texLowRight = new Vector2(texUpLeft.X + tileWidth * texelWidth - 0.02f, texUpLeft.Y + tileHeight * texelHeight);
            initQuad(texUpLeft, texLowRight, origin, back);
        }
        public void initQuad(Vector2 texUpLeft, Vector2 texLowRight, Vector3 origin, Vector3 back)
        {
            Vertices = new VertexPositionNormalTexture[4];
            Indexes = new short[6];
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

        //***************************************************************
        //
        //
        // magic DO NOT TOUCH UNDER ANY CIRCUMSTANCE
        //
        //
        //***************************************************************
        public void Lock(Quad left, Quad above)
        {
            Vector3 upperleft = UpperLeft;
            Vector3 upperright = UpperRight;
            Vector3 lowerleft = LowerLeft;
            Vector3 lowerright = LowerRight;
            string message = string.Format("Quad:{0} {1}\n     {2} {3}\n", upperleft, upperright, lowerleft, lowerright);
            
            UpdateVertices(upperleft, upperright, lowerleft, lowerright);
            //return;
            if (left == null && above == null) 
            {
                //this is actually the bottom left
                //Console.WriteLine("upper left");

                //Console.WriteLine(message + "\n");
            
                return;
            }
            else if (left == null)
            {
                //return;
                //Console.WriteLine("bottom left");
                lowerleft = above.UpperLeft;//do not touch
                lowerright = above.UpperRight;//do not touch

            }
            else if (above == null)
            {
                //Console.WriteLine("upper right");
                upperleft = left.UpperRight;
                lowerleft = left.LowerRight;
            }
            else
            {
                //this is actually the top right
                //Console.WriteLine("bottom right");
                lowerleft = above.UpperLeft;//do not touch
                lowerright = above.UpperRight;//do not touch

                upperleft = left.UpperRight;
                lowerleft = left.LowerRight;
                //left.UpdateVertices(left.UpperLeft, left.UpperRight, left.LowerLeft, left.LowerRight);
        
            }
            //Console.WriteLine(message + "\n");
            UpdateVertices(upperleft, upperright, lowerleft, lowerright);
            //left.UpdateVertices(left.UpperLeft, above.LowerLeft, left.LowerLeft, left.LowerRight);
        }
        private void FillVertices()
        {
            // Fill in texture coordinates to display full texture
            // on quad


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
            Vertices[3].Position = UpperRight;// +new Vector3(10, 10, 0);
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
