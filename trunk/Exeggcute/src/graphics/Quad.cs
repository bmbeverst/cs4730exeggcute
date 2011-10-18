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
        /*
         * Vertices[0].Position = BottomLeft;
            Vertices[0].TextureCoordinate = textureBottomLeft;

            Vertices[1].Position = TopLeft;
            Vertices[1].TextureCoordinate = textureTopLeft;

            Vertices[2].Position = BottomRight;
            Vertices[2].TextureCoordinate = textureBottomRight;

            Vertices[3].Position = TopRight;
            Vertices[3].TextureCoordinate = textureTopRight;
         */
        public Vector3 Center;
        public Vector3 BottomLeft
        {
            get { return Vertices[0].Position; }
            set { Vertices[0].Position = value; }
        }
        public Vector3 TopLeft
        {
            get { return Vertices[1].Position; }
            set { Vertices[1].Position = value; }
        }
        public Vector3 BottomRight
        {
            get { return Vertices[2].Position; }
            set { Vertices[2].Position = value; }
        }
        public Vector3 TopRight
        {
            get { return Vertices[3].Position; }
            set { Vertices[3].Position = value; }
        }
        
        public Vector3 Normal;
        public Vector3 Up;
        public Vector3 Left;

        public Vector2 UVBottomLeft
        {
            get { return Vertices[0].TextureCoordinate;  }
            set { Vertices[0].TextureCoordinate = value; }
        }

        public Vector2 UVTopLeft
        {
            get { return Vertices[1].TextureCoordinate; }
            set { Vertices[1].TextureCoordinate = value; }
        }

        public Vector2 UVBottomRight
        {
            get { return Vertices[2].TextureCoordinate; }
            set { Vertices[2].TextureCoordinate = value; }
        }

        public Vector2 UVTopRight
        { 
            get { return Vertices[3].TextureCoordinate;  }
            set { Vertices[3].TextureCoordinate = value; }
        }
        
       

        public VertexPositionNormalTexture[] Vertices;
        public short[] Indexes;
        public float Width { get; protected set; }
        public float Height { get; protected set; }

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

        public void initQuad(Vector2 texTopLeft, Vector2 texBottomRight, Vector3 center, Vector3 back)
        {
            Vertices = new VertexPositionNormalTexture[4];
            Indexes = new short[6]; ;
            Center = center;
            Normal = back;
            Up = Vector3.Up;
            UVTopLeft = texTopLeft;
            UVTopRight = new Vector2(texBottomRight.X, texTopLeft.Y);
            UVBottomLeft = new Vector2(texTopLeft.X, texBottomRight.Y);
            UVBottomRight = texBottomRight;
            // Calculate the quad corners
            Left = Vector3.Cross(Normal, Up);
            Vector3 topcenter = (Up * Height / 2) + Center;
            TopLeft = topcenter + (Left * Width / 2);
            TopRight = topcenter - (Left * Width / 2);
            BottomLeft = TopLeft - (Up * Height);
            BottomRight = TopRight - (Up * Height);
            recalculateNormals();

            Indexes[0] = 0;
            Indexes[1] = 1;
            Indexes[2] = 2;
            Indexes[3] = 2;
            Indexes[4] = 1;
            Indexes[5] = 3;

        }

        public void UpdateVertices(Vector3 topleft, Vector3 topright, Vector3 bottomleft, Vector3 bottomright)
        {
            
            TopLeft = topleft;
            TopRight = topright;
            BottomLeft = bottomleft;
            BottomRight = bottomright;
            recalculateNormals();
        }

        protected void recalculateNormals()
        {
            Vector3 right = TopLeft - TopRight;
            Vector3 down = TopRight - BottomRight;
            Normal = Vector3.Cross(down, right);
            Normal.Normalize();
            for (int i = 0; i < Vertices.Length; i++)
            {
                Vertices[i].Normal = Normal;
            }
        }

        public void Lock(Quad left, Quad below)
        {

            bool lockLeft = left != null;
            bool lockBelow = below != null;

            if (lockLeft)
            {
                TopLeft = left.TopRight;
                BottomLeft = left.BottomRight;
                
            }
            if (lockBelow)
            {
                BottomLeft = below.TopLeft;
                BottomRight = below.TopRight;
            }
            
            recalculateNormals();
            
        }


        
    }
}
