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
        public Vector3 Center;
        public Vector3 BottomLeft
        {
            get { return Vertices[0].Position; }
            protected set { Vertices[0].Position = value; }
        }
        public Vector3 TopLeft
        {
            get { return Vertices[1].Position; }
            protected set { Vertices[1].Position = value; }
        }
        public Vector3 BottomRight
        {
            get { return Vertices[2].Position; }
            protected set { Vertices[2].Position = value; }
        }
        public Vector3 TopRight
        {
            get { return Vertices[3].Position; }
            protected set { Vertices[3].Position = value; }
        }

        public Vector2 UVBottomLeft
        {
            get { return Vertices[0].TextureCoordinate;  }
            protected set { Vertices[0].TextureCoordinate = value; }
        }

        public Vector2 UVTopLeft
        {
            get { return Vertices[1].TextureCoordinate; }
            protected set { Vertices[1].TextureCoordinate = value; }
        }

        public Vector2 UVBottomRight
        {
            get { return Vertices[2].TextureCoordinate; }
            protected set { Vertices[2].TextureCoordinate = value; }
        }

        public Vector2 UVTopRight
        { 
            get { return Vertices[3].TextureCoordinate;  }
            protected set { Vertices[3].TextureCoordinate = value; }
        }

        public VertexPositionNormalTexture[] Vertices;
        public short[] Indexes;

        public float Width { get; protected set; }
        public float Height { get; protected set; }

        /// <summary>
        /// A texture mapped quad.
        /// </summary>
        /// 
        /// <param name="topleft">
        /// Vector in texel space (range 0 to 1) pointing to the upper left 
        /// portion of the texture we wish to map onto this quad
        /// </param>
        /// 
        /// <param name="bottomRight">
        /// Vector in texel space (range 0 to 1) pointing to the bottom right
        /// portion of the texture we wish to map onto this quad
        /// </param>
        /// 
        /// <param name="position">T
        /// he position of the center of this quad.
        /// </param>
        /// <param name="back">Vector pointing towards the camera.</param>
        /// <param name="width">width of the quad</param>
        /// <param name="height">height of the quad</param>
        public Quad(Vector2 topleft,
                    Vector2 bottomRight,
                    Vector3 position,
                    Vector3 back,
                    Vector3 up,
                    float width,
                    float height)
        {
            Width = width;
            Height = height;
            initQuad(topleft, bottomRight, position, back, up);
        }

        public void initQuad(Vector2 texTopLeft, Vector2 texBottomRight, Vector3 center, Vector3 back, Vector3 up)
        {
            Vertices = new VertexPositionNormalTexture[4];
            Indexes = new short[6]; ;
            Center = center;
            UVTopLeft = texTopLeft;
            UVTopRight = new Vector2(texBottomRight.X, texTopLeft.Y);
            UVBottomLeft = new Vector2(texTopLeft.X, texBottomRight.Y);
            UVBottomRight = texBottomRight;
            
            // I know this seems backwards, but it works
            Vector3 left = Vector3.Cross(back, up);
            Vector3 topcenter = Center + (up * Height / 2) ;
            TopLeft = topcenter + (left * Width / 2);
            TopRight = topcenter - (left * Width / 2);
            BottomLeft = TopLeft - (up * Height);
            BottomRight = TopRight - (up * Height);
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
            Vector3 normal = Vector3.Cross(down, right);
            normal.Normalize();
            for (int i = 0; i < Vertices.Length; i++)
            {
                Vertices[i].Normal = normal;
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
