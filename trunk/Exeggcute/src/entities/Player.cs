﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Exeggcute.src.assets;
using Exeggcute.src.scripting;
using Exeggcute.src.scripting.action;

namespace Exeggcute.src.entities
{
    class Player : ParentEntity
    {

        public float MoveSpeed { get; protected set; }
        public float FocusSpeed { get; protected set; }

        //private Spawner shotSpawner;

        private int lives;
        private int bombs;
        private int score;

        public bool CanControl
        {
            get { return actionList == null; }
        }

        public bool IsInvulnerable
        {
            get { return !InvulnTimer.IsDone; }
        }

        public bool IsShooting { get; protected set; }
        public bool IsBombing { get; protected set; }

        public MassSpawner bomb;
        public Timer InvulnTimer;

        protected int frames;
        protected bool flashDraw;

        public Player(ModelName model, ArsenalName arsenalName, HashList<Shot> shotList, HashList<Gib> gibList)
            : base(model, ScriptName.playerspawn, arsenalName, shotList, gibList)
        {
            lives = 3;
            bombs = 3;
            score = 1234;
            MoveSpeed = 1;
            FocusSpeed = 0.5f;
            InvulnTimer = new Timer(120);
            //bomb = new MassSpawner(null, 120, shotList);
            Hitbox = new BoundingSphere(Position, 0.2f);
        }

        public void LockPosition(Camera camera, Rectangle gameArea)
        {
            // HACK
            // If we cant control, then don't lock us to the screen, we are
            // probably respawning
            if (!CanControl) return;


            if (X < gameArea.Left)
            {
                X = gameArea.Left;
            }
            else if (X > -gameArea.Left)
            {
                X = -gameArea.Left;
            }

            if (Y < gameArea.Top)
            {
                Y = gameArea.Top;
            }
            else if (Y > -gameArea.Top)
            {
                Y = -gameArea.Top;
            }

        }
        protected void processControls(ControlManager controls)
        {
            //if (shotList.Count > 0) Util.Die("works");// Console.WriteLine("{0}", shotList.Count);
            float speed;
            if (controls[Ctrl.Focus].IsPressed)
            {
                speed = FocusSpeed;
            }
            else
            {
                speed = MoveSpeed;
            }
            if (IsBombing) speed /= 2;

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
                ModelRotX = 1;
            }
            else if (controls[Ctrl.Right].IsPressed)
            {
                dx = 1;
                ModelRotX = -1;
            }
            else
            {
                ModelRotX = 0;
            }

            //This makes it so we dont overwrite the angle if our speed is 0
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

            IsShooting = controls[Ctrl.Action].IsPressed;

            if (controls[Ctrl.Cancel].JustPressed && !IsBombing && bombs > 0)
            {
                IsBombing = true;
                bombs -= 1;
                Console.WriteLine("Begin bombing");
            }

            if (controls[Ctrl.Start].DoEatPress())
            {
                World.Pause();
            }

            
        }
        int frame = 0;
        public void Update(ControlManager controls)
        {
            //shotSpawner.SetPosition(Position);
            if (CanControl) processControls(controls);
            frame += 1;
            if (IsShooting && frames % 10 == 0) SoundBank.Get(SoundName.shot0).Play();
            if (IsBombing)
            {
                bomb.Update(this);
                if (bomb.IsDone)
                {
                    IsBombing = false;
                    bomb.Reset();
                    Console.WriteLine("End bombing");
                }
            }
            score += 123;
            InvulnTimer.Increment();
            frames += 1;
            flashDraw = (IsInvulnerable && frames % 2 == 0);
            if (!CanControl && actionPtr == actionList.Count)
            {
                actionList = null;
            }
            if (IsShooting)
            {
                base.Update();
            }
            else
            {
                base.ParentEntityBaseUpdate();
            }
        }

        public override void Draw(GraphicsDevice graphics, Matrix view, Matrix projection)
        {
            if (!flashDraw) base.Draw(graphics, view, projection);
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
            actionPtr = 0;
            lives -= 1;
            InvulnTimer.Reset();
            //spawn the death animation
        }


    }
}
