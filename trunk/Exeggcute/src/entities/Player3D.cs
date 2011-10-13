﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Exeggcute.src.assets;
using Exeggcute.src.scripting;

namespace Exeggcute.src.entities
{
    class Player3D : CommandEntity
    {
        public List<Shot> shots;
        public static readonly Point Bounds = new Point(30, 37);

        public float MoveSpeed { get; protected set; }
        public float FocusSpeed { get; protected set; }

        private CommandEntity shotSpawner;

        private int lives;
        private int bombs;
        private int score;

        public bool CanControl
        {
            get { return actionList == null; }
        }

        public Player3D(ModelName name, List<Shot> shots)
            : base(name, ScriptName.playerspawn, new List<Shot>())
        {
            Shot shot = new Shot(ModelName.testcube, ScriptName.playershot0);
            shotSpawner = new CommandEntity(ModelName.testcube, ScriptName.playerspawner0, new List<Shot> { shot });
            spawnList.Add(shot);
            this.shots = shots;
            lives = 3;
            bombs = 3;
            score = 1234;
            MoveSpeed = 1;
            FocusSpeed = 0.5f;
        }

        public override void Process(VanishAction vanish)
        {
            actionList = null;
        }

        public void LockPosition(Camera camera)
        {
            // HACK
            // If we cant control, then don't lock us to the screen, we are
            // probably respawning
            if (!CanControl) return;


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
        protected void processControls(ControlManager controls)
        {
            float speed;
            if (controls[Ctrl.Focus].IsPressed)
            {
                speed = FocusSpeed;
            }
            else
            {
                speed = MoveSpeed;
            }
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

            //FIXME: temporary, just for debugging
            if (controls[Ctrl.RShoulder].IsPressed)
            {
                Z += speed;
            }
            else if (controls[Ctrl.LShoulder].IsPressed)
            {
                Z -= speed;
            }

            if (controls[Ctrl.Action].IsPressed)
            {
                shotSpawner.Update();
            }

            if (controls[Ctrl.Start].DoEatPress())
            {
                World.Pause();
            }

            
        }

        public void Update(ControlManager controls)
        {
            shotSpawner.SetPosition(Position);
            if (CanControl) processControls(controls);
            shotSpawner.UpdateChildren();
            for (int i = shotSpawner.ShotList.Count - 1; i >= 0; i -= 1)
            {
                Shot shot = shotSpawner.ShotList[i];
                if (shot.Y > Bounds.Y + 4)
                {
                    shotSpawner.ShotList.RemoveAt(i);
                }
            }
            score += 123;
            
            base.Update();
        }

        public override void Draw(GraphicsDevice graphics, Matrix view, Matrix projection)
        {
            base.Draw(graphics, view, projection);
            shotSpawner.Draw(graphics, view, projection);
            //shots.ForEach(shot => shot.Draw(graphics, view, projection) );
        }

        public void DrawHUD(SpriteBatch batch, SpriteFont scoreFont)
        {
            for (int i = 0; i < lives; i += 1)
            {
                LifeItem.HUDSprite.Draw(batch, new Vector2(50, i * 40));
            }

            for (int i = 0; i < bombs; i += 1)
            {
                BombItem.HUDSprite.Draw(batch, new Vector2(100, i * 40));
            }
            //9 decimal places
            string scoreString = string.Format("{0:000,000,000}", score);
            batch.DrawString(scoreFont, scoreString, new Vector2(10, 120), Color.White);

        }

        public void Kill()
        {
            Console.WriteLine("KILLED!");
            actionList = ScriptBank.Get(ScriptName.playerspawn);
            cmdPtr = 0;
            lives -= 1;
            //spawn the death animation
        }
    }
}