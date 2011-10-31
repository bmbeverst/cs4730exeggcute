using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.graphics;
using Microsoft.Xna.Framework.Graphics;
using Exeggcute.src.assets;
using Microsoft.Xna.Framework;
using Exeggcute.src.gui;
using Microsoft.Xna.Framework.Media;
using System.Collections.ObjectModel;
using Exeggcute.src.loading;
using Exeggcute.src.sound;

namespace Exeggcute.src.graphics
{
    enum Concavity
    {
        Inside,
        Outside
    }

    enum TerrainType
    {
        Flat,
        Varied,
        Pulse
    }
    /// <summary>
    /// A polygonal mesh tiled aperiodically by Wang tiles used for 
    /// scrolling background. The tiles are curved into a cylinder
    /// to allow continuous scrolling.
    /// </summary>
    class WangMesh
    {
        private WangArray wangGrid;
        private int vertCols;
        private int vertRows;
        private Wrapped2DArray<Quad> quads;
        private QuadBatch quadBatch;
        private TileMap tilemap;
        private Random rng = new Random();

        private RepeatedSound collideSound;

        /// <summary>
        /// The width of the terrain in world coordinates.
        /// </summary>
        public float Width { get; protected set; }

        /// <summary>
        /// The circumference of the terrain in world coordinates
        /// </summary>
        public float Height { get; protected set; }

        /// <summary>
        /// The terrain rotates at a constant speed given by this 
        /// value.
        /// </summary>
        public float Speed { get; protected set; }

        /// <summary>
        /// The center of the cylinder is located at (0,0,Depth + Radius)
        /// </summary>
        public float Depth { get; protected set; }

        /// <summary>
        /// The center of the cylinder is located at (0,0,Depth + Radius)
        /// </summary>
        public float Radius { get; protected set; }

        /// <summary>
        /// The size of each square tile on the mesh in world coordinates.
        /// </summary>
        public float TileSize { get; protected set; }

        /// <summary>
        /// The amount the terrain has rotated based on its Speed.
        /// </summary>
        public float Rotation { get; protected set; }
        
        /// <summary>
        /// The angle between each face on the cylinder.
        /// </summary>
        public readonly float FaceAngle;

        /// <summary>
        /// The row currently directly beneath the 
        /// </summary>
        public int ViewRow { get; protected set; }

        /// <summary>
        /// Specifies whether the player resides inside or outside the 
        /// cylinder.
        /// </summary>
        public Concavity Orientation { get; protected set; }

        /// <summary>
        /// Specifies how to generate the terrain at runtime.
        /// </summary>
        public TerrainType Type { get; protected set; }

        protected float heightVariance;

        protected float centerDepth;

        //MAGIC
        protected int jOffset;
        protected float ANGLEOFFSET = 3 * MathHelper.Pi / 2;

        public WangMesh(GraphicsDevice graphics, Texture2D texture, int cols, int rows, float size, float heightVariance, float scrollSpeed, Concavity orientation, TerrainType type, float depth, float radius)
        {
            //MUST be initialized first!
            this.Orientation = orientation;
            //fixme
            this.collideSound = Assets.MakeRepeated("crash");
            this.Type = type;
            this.heightVariance = heightVariance;
            Texture2D wangTexture = texture;
            int texWidth = wangTexture.Width;
            int texHeight = wangTexture.Height;
            this.quadBatch = new QuadBatch(graphics, texture);
            this.vertCols = cols + 1;
            this.vertRows = rows + 1;
            this.Speed = -scrollSpeed;//MINUS INTENTIONAL
            this.Width = cols * size; //
            this.Height = rows * size;
            this.Depth = -depth;//MINUS INTENTIONAL
            this.TileSize = size;
            //counterintuitively, this should be vertcols and vertrows
            this.wangGrid = new WangArray(vertCols, vertRows);
            this.quads = new Wrapped2DArray<Quad>(vertCols, vertRows);
            this.tilemap = new TileMap(texture, 32, 32);
            this.Radius = -radius;//MINUS INTENTIONAL
            this.FaceAngle = MathHelper.TwoPi / vertRows;


            // account for concavity!
            int sign = getSign();
            this.centerDepth = Depth + sign * Radius;

            this.jOffset = 0; //*= sign;
            this.Depth   *= sign;
            this.Speed   *= sign;

            this.jOffset = sign * (rows * 3 / 4);
            initMesh();
            
        }

        private void initMesh()
        {

            Vector3 back = new Vector3(0, 0, -1);
            Vector3 up =  new Vector3(0, 1, 0);

            for (int i = 0; i < vertCols; i += 1)
            {
                for (int j = 0; j < vertRows; j += 1)
                {
                    int index = wangGrid[i, j];
                    float height;
                    if (Type != TerrainType.Varied)
                    {
                        height = 0;
                    }
                    else
                    {
                        height = rng.Next() * heightVariance - heightVariance / 2 + Depth;
                    }
                    float r = Radius + height;
                    float theta = FaceAngle * j + ANGLEOFFSET;
                    float y = -r * FastTrig.Cos(theta);
                    float z = r + Depth + r * FastTrig.Sin(theta);
                    z *= getSign();
                    float xOffset = TileSize / 2; //because i is the vertex!
                    Vector3 position = new Vector3(i * TileSize - Width / 2 + xOffset, y, z);
                    quads[i, j] = tilemap.CreateQuad(index, position, back, up, TileSize, TileSize);
                }
            }

            lockEdges();
        }

        private void lockEdges()
        {
            for (int i = 0; i < vertCols; i += 1)
            {
                for (int j = 0; j < vertRows; j += 1)
                {
                    Quad left;

                    if (i <= 0) left = null;
                    else left  = quads[i - 1, j];

                    Quad below = quads[i, j - 1];

                    quads[i, j].Lock(left, below);
                }
            }
            for (int i = 0; i < vertCols; i += 1)
            {
                Quad left;
                Quad below = quads[i, vertRows - 1];

                if (i <= 0) left = null;
                else left = quads[i - 1, 0];

                quads[i, 0].Lock(left, below);
            }
        }

        private void lockLocal(int xIndex, int yIndex)
        {
            
            int xMin = Math.Max(0, xIndex);
            int xMax = Math.Min(vertCols - 1, xIndex + 2);

            for (int i = xMin; i < xMax; i += 1)
            {
                for (int j = yIndex; j < yIndex + 2; j += 1)
                {
                    Quad left;
                    if (i <= 0) left = null;
                    else left = quads[i - 1, j];

                    Quad below = quads[i, j - 1];

                    quads[i, j].Lock(left, below);
                }
            }
            
        }
        
        public void Update(ReadOnlyCollection<float> freqs)
        {
            Rotation += Speed;
            ViewRow = CalculateViewIndex();
            if (Type != TerrainType.Pulse) return;
            int spacing = 256 / vertCols;
            for (int j = 0; j < vertRows; j += 1)
            {
                for (int i = 0; i < vertCols; i += 1)
                {
                    Quad current = quads[i, j];
                    float height = (1 - freqs[i * spacing]) * 32;
                    float r = Radius + height;
                    float theta = FaceAngle * j + ANGLEOFFSET;
                    float y = -r * FastTrig.Cos(theta);
                    float z = Radius + Depth + r * FastTrig.Sin(theta);
                    z *= getSign();
                    float x = current.TopRight.X;
                    quads[i, j].UpdateVertices(current.TopLeft, new Vector3(x, y, z), current.BottomLeft, current.BottomRight);
                }
            }
            lockEdges();
        }

        
        public void Impact(float x, float y, float mass, float speed)
        {
            //i = x
            //j = y
            //x = x * FastTrig.Cos(-Spin) - y * FastTrig.Sin(-Spin);
            //y = x * FastTrig.Sin(-Spin) + y * FastTrig.Cos(-Spin);
            int i = (int)((x + Width/2 + TileSize/2) / TileSize);
            int j = (int)(CalculateViewIndex() + (y / TileSize));

            collideSound.Play();
            for (int tempI = i - 2; tempI < i + 2; tempI++)
            {
                for (int tempJ = j - 2; tempJ < j + 2; tempJ++)
                {

                    //j < quads.Rows-1 && j > -(quads.Rows-1)
                    //i < clums-1 && i > -1
                    if (i < vertCols && i > 0)
                    {
                        float distace = 4 - Math.Abs(tempI - i + tempJ - j);
                        Quad current = quads[tempI, tempJ];
                        float curY = current.TopRight.Y;
                        float curZ = current.TopRight.Z;

                        float height = getSign() * 2 + distace * 5;

                        float r = Radius + height;
                        float theta = FaceAngle * tempJ + ANGLEOFFSET;
                        float newY = -r * FastTrig.Cos(theta);
                        float newZ = Radius + Depth + r * FastTrig.Sin(theta);
                        newZ *= getSign();
                        float xQuad = current.TopRight.X;
                        quads[tempI, tempJ].UpdateVertices(current.TopLeft, new Vector3(xQuad, newY, newZ), current.BottomLeft, current.BottomRight);
                        lockLocal(tempI, tempJ);    
                    }
                    
                }
            }
        }

        public int getSign()
        {
            if (Orientation == Concavity.Inside) return -1;
            else return 1;

        }

        public int CalculateViewIndex()
        {
            return (int)(-Rotation / FaceAngle + 0.5f);
        }

        public void Draw(GraphicsDevice graphics, Matrix view, Matrix projection)
        {
            /*RasterizerState old = graphics.RasterizerState;
            RasterizerState rs = new RasterizerState();
            rs.FillMode = FillMode.WireFrame;
            graphics.RasterizerState = rs;*/
            quadBatch.SetCamera(view, projection);
            int buffer = 22;
            quadBatch.DrawNear(graphics, quads, centerDepth, Rotation, ViewRow, buffer);
            //quadBatch.Draw(graphics, quads, centerDepth, Rotation);
            
            //graphics.RasterizerState = old;
        }
        float zRot;
        public void DrawRot(GraphicsDevice graphics, Matrix view, Matrix projection, float zRotSpeed)
        {
            zRot += zRotSpeed;
            quadBatch.SetCamera(view, projection);
            int buffer = 22;
            quadBatch.DrawNearRot(graphics, quads, centerDepth, Rotation, ViewRow, buffer, zRot);
            
        }
    }
}
