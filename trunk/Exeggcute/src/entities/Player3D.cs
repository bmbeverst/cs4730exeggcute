using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Exeggcute.src.assets;

namespace Exeggcute.src.entities
{
    class Player3D : PlanarEntity3D
    {
        public List<Shot> shots = new List<Shot>();
        public List<ShotSpawner> spawners = new List<ShotSpawner>();
        public static readonly Point Bounds = new Point(30, 37);

        private int lives;
        public Player3D(ModelName name, Vector3 pos)
            : base(name, pos)
        {
            Shot shot = new Shot(ModelName.testcube, Vector3.Zero);
            spawners.Add(new ShotSpawner(shot, Vector2.UnitX, 10, 10, MathHelper.Pi / 2, 2));
            spawners.Add(new ShotSpawner(shot, -Vector2.UnitX, 10, 5, MathHelper.Pi / 2, 2));
            lives = 3;
        }
        public void LockPosition(Camera camera)
        {
            if (X < -Bounds.X)
            {
                X = -Bounds.X;
            }
            else if (X > Bounds.X)
            {
                X = Bounds.X;
            }

            if (Y < -Bounds.Y)
            {
                Y = -Bounds.Y;
            }
            else if (Y > Bounds.Y)
            {
                Y = Bounds.Y;
            }

        }

        public void Update(ControlManager controls)
        {
            float speed = 1;
            int dx = 0;
            int dy = 0;
            if (controls[Ctrl.Up].IsPressed)
            {
                dy = 1;
            }
            else if (controls[Ctrl.Down].IsPressed)
            {
                dy = -1;
            }

            if (controls[Ctrl.Left].IsPressed)
            {
                dx = -1;
            }
            else if (controls[Ctrl.Right].IsPressed)
            {
                dx = 1;
            }
            if ((dx | dy) == 0)
            {
                Speed = 0;
                Angle = 0;
            }
            else
            {
                Speed = speed;
                Angle = FastTrig.Atan2(dy, dx);
            }

            if (controls[Ctrl.RShoulder].IsPressed)
            {
                Z += speed;
            }
            else if (controls[Ctrl.LShoulder].IsPressed)
            {
                Z -= speed;
            }

            foreach (ShotSpawner spawner in spawners)
            {
                Shot spawned = spawner.TrySpawnAt(Position, controls[Ctrl.Action]);
                if (spawned != null)
                {
                    shots.Add(spawned);
                }
            }

            for (int i = shots.Count - 1; i >= 0; i -= 1)
            {
                Shot shot = shots[i];
                shot.Update();
                if (shot.Y > Bounds.Y + 4)
                {
                    shots.RemoveAt(i);
                }
            }
            base.Update();
        }

        public override void Draw(GraphicsDevice graphics, Matrix view, Matrix projection)
        {
            base.Draw(graphics, view, projection);
            shots.ForEach(shot => shot.Draw(graphics, view, projection));
        }

        public void DrawHUD(SpriteBatch batch)
        {
            for (int i = 0; i < lives; i += 1)
            {

            }
        }
    }
}
