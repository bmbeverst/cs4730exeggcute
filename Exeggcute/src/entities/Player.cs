﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.assets;
using Exeggcute.src.entities.items;
using Exeggcute.src.graphics;
using Exeggcute.src.input;
using Exeggcute.src.loading;
using Exeggcute.src.scripting;
using Exeggcute.src.scripting.action;
using Exeggcute.src.scripting.arsenal;
using Exeggcute.src.sound;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Exeggcute.src.config;

namespace Exeggcute.src.entities
{
    class Player : ParentEntity
    {
        public static readonly string DebugName = "debug";

        public float MoveSpeed { get; protected set; }
        public float FocusSpeed { get; protected set; }

        public int Lives { get; protected set; }
        private int bombs;
        public int Score { get; protected set; }

        public bool CanControl
        {
            get { return script == null; }
        }

        public bool IsInvulnerable
        {
            get { return !InvulnTimer.IsDone; }
        }

        public bool IsShooting { get; protected set; }
        public bool IsBombing { get; protected set; }
        public bool IsFocusing { get; protected set; }

        public Arsenal bomb;
        public Timer bombTimer = new Timer(180);
        public Timer InvulnTimer;

        protected int frames;
        protected bool flashDraw;
        protected BehaviorScript deathScript;

        protected int graze;
        protected int power;
        protected int powerMax;
        protected float rollAngle;
        protected float pitchAngle;

        protected List<Arsenal> arsenalList;
        protected List<int> thresholds;
        protected int attackPtr;

        public Arsenal CurrentAttack
        {
            get { return arsenalList[attackPtr]; }
        }

        public string RawData { get; protected set; }
        public string Name { get; protected set; }
        protected float lightLevel;

        public Player(string data,
                      string name,
                      Model model, 
                      Texture2D texture, 
                      float scale,
                      float radius,
                      Vector3 rotation,
                      BehaviorScript deathScript, 
                      Arsenal bomb, 
                      GibBatch gibBatch,
                      RepeatedSound dieSFX,
                      List<Arsenal> arsenalList, 
                      List<int> thresholds, 
                      int numLives,
                      int numBombs,
                      float moveSpeed,
                      float focusSpeed,
                      float hitRadius,
                      float lightLevel,
                      Alignment alignment)
            : base(model, texture, scale, radius, rotation, deathScript, dieSFX, null, gibBatch, alignment)
        {
            this.RawData = data;
            this.Name = name;
            this.arsenalList = arsenalList;
            this.thresholds = thresholds;

            this.powerMax = thresholds[thresholds.Count - 1];

            this.attackPtr = 0;
            this.arsenal = arsenalList[attackPtr];

            this.lightLevel = lightLevel;

            this.graze = 0;
            this.Score = 0;

            this.Lives = numLives;
            this.bombs = numBombs;
            
            this.MoveSpeed = moveSpeed;
            this.FocusSpeed = focusSpeed;
            this.InvulnTimer = new Timer(120);
            this.bomb = bomb;
            
            this.deathScript = deathScript;

            LifeSprite = Assets.Sprite["life"];
            BombSprite = Assets.Sprite["bomb"];

            chooseAlignment(Alignment);
            bomb.CheckAlignment(Alignment);
        }

        public static Player LoadFromFile(string filename)
        {
            return Loaders.Player.LoadFromFile(filename);
        }

        public void LockPosition(Rectangle gameArea)
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

        protected void processControls(ControlManager controls, bool canShoot)
        {
            float speed;
            IsFocusing = controls[Ctrl.Focus].IsPressed;
            if (IsFocusing)
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
            }
            else if (controls[Ctrl.Right].IsPressed)
            {
                dx = 1;
            }


            //This makes it so we dont overwrite the angle if our speed is 0
            if ((dx | dy) == 0)
            {
                Speed = 0;
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

            IsShooting = controls[Ctrl.Action].IsPressed && canShoot;

            if (controls[Ctrl.Cancel].JustPressed && !IsBombing && bombs > 0 && canShoot)
            {
                IsBombing = true;
                bombs -= 1;
            }

            processPitchRoll(dx, dy);
        }

        protected void processPitchRoll(int dx, int dy)
        {
            float rollMax = MathHelper.PiOver4;
            float rollMin = -MathHelper.PiOver4;
            float pitchMax = MathHelper.PiOver4;
            float pitchMin = -MathHelper.PiOver4;
            float rollSpeed;
            float pitchSpeed;
            float rightSpeed;
            //Slow based on focus;
            if (IsFocusing)
            {
                rollSpeed = 0.01f;
                pitchSpeed = 0.01f;
                rightSpeed = 0.1f;
            }
            else
            {
                rollSpeed = 0.02f;
                pitchSpeed = 0.02f;
                rightSpeed = 0.1f;
            }

            // If we are moving opposite of our angle then 
            // right ourselves more quickly.
            if (rollAngle < 0 && dx > 0 ||
                rollAngle > 0 && dx < 0)
            {
                rollSpeed = rightSpeed;
            }

            if (pitchAngle < 0 && dy > 0 ||
                pitchAngle > 0 && dy < 0)
            {
                pitchSpeed = rightSpeed;
            }

            rollAngle += dx * rollSpeed;
            pitchAngle += dy * pitchSpeed;


            // Clamp values to min and max;
            // If not moving, rightourselves.
            rollAngle = Math.Min(rollAngle, rollMax);
            rollAngle = Math.Max(rollAngle, rollMin);
            if (dx == 0)
            {
                rollAngle -= Math.Sign(rollAngle) * rightSpeed;
            }

            pitchAngle = Math.Min(pitchAngle, pitchMax);
            pitchAngle = Math.Max(pitchAngle, pitchMin);
            if (dy == 0)
            {
                pitchAngle -= Math.Sign(pitchAngle) * rightSpeed;
            }

            // Keep from jittering
            if (Math.Abs(pitchAngle) <= rightSpeed && dy == 0)
            {
                pitchAngle = 0;
            }
            if (Math.Abs(rollAngle) <= rightSpeed && dx == 0)
            {
                rollAngle = 0;
            }
        }

        public void DoDemo()
        {
            BehaviorScript script = Assets.GetBehavior("playerdemo0");
            this.script = script;
            ActionPtr = 0;
        }

        public void ResetFromDemo()
        {
            this.script = deathScript;
            ActionPtr = 0;
            attackPtr = 0;
            power = 0;
        }

        int frame = 0;
        public void Update(ControlManager controls, bool canShoot)
        {
            //shotSpawner.SetPosition(Position);
            if (CanControl) processControls(controls, canShoot);
            frame += 1;
            //if (IsShooting && frames % 10 == 0) SoundBank.Get("shot0").Play();
            if (IsBombing)
            {
                bomb.FireAll();
                bomb.Update(Position, Angle);
                if (bombTimer.IsDone)
                {
                    IsBombing = false;
                    bombTimer.Reset();
                }
                else
                {
                    bombTimer.Increment();
                }
            }
            InvulnTimer.Increment();
            frames += 1;
            flashDraw = (IsInvulnerable && frames % 2 == 0);
            if (!CanControl && ActionPtr == script.Count)
            {
                script = null;
            }
            if (IsShooting)
            {
                arsenal.FireAll();
                base.Update();
            }
            else
            {
                arsenal.StopAll();
                base.UpdateMovers();
            }
        }

        Sprite LifeSprite;
        Sprite BombSprite;
        public void DrawHUD(SpriteBatch batch, SpriteFont scoreFont)
        {
            Texture2D gamelogo = Assets.Texture["gamelogo"];
            batch.Draw(gamelogo, new Vector2(0,10), null, Color.White, 0, Vector2.Zero, 0.16f, SpriteEffects.None, 0.9f);
            float yStart = 210;
            float xStart = 20;
            

            //9 decimal places
            string scoreString = string.Format("Score {0:000,000,000}", Score);
            batch.DrawString(scoreFont, scoreString, new Vector2(xStart, yStart), Color.White);
            string grazeString = string.Format("Graze       {0:0,000}", graze);
            batch.DrawString(scoreFont, grazeString, new Vector2(xStart, yStart + 30), Color.White);
            string powerString = string.Format("Power         {0,3}", power);
            if (power == powerMax)
            {
                powerString = "Power       -MAX-";
            }
            batch.DrawString(scoreFont, powerString, new Vector2(xStart, yStart + 60), Color.White);

            for (int i = 0; i < Lives; i += 1)
            {
                LifeSprite.Draw(batch, new Vector2(86, i * 40 + yStart + 120));
            }

            for (int i = 0; i < bombs; i += 1)
            {
                BombSprite.Draw(batch, new Vector2(136, i * 40 + yStart + 120));
            }
            
        }

        public override void Process(UpgradeAction upgrade)
        {
            power = thresholds[attackPtr] + 1;
            attackPtr += 1;
            Console.WriteLine("PWOER {0} {1}", power, thresholds[attackPtr]);
            if (power > powerMax || attackPtr == arsenalList.Count || attackPtr > upgrade.Max)
            {
                attackPtr = 0;
                power = 0;
            }
            this.arsenal = CurrentAttack;
            this.arsenal.CheckAlignment(Alignment);
        }

        public override void Kill()
        {
            Lives -= 1;
            if (Lives < 0)
            {
                Worlds.World.GameOver();
            }
            else
            {
                script = deathScript;
                ActionPtr = 0;
                
                deathSound.Play();
                InvulnTimer.Reset();
            }
            Position = new Vector3(-1000, 1000, 1000);
            //spawn the death animation
        }



        public void Graze(Shot shot)
        {
            graze += 1;
            Score += 100;
        }
        int itemsCollected = 0;
        public void Collect(Item item)
        {
            itemsCollected += 1;
            Score += 100;
            //throw new InvalidOperationException("can only collect types of items");
        }

        public void Collect(ExtraBomb extraBomb)
        {
            bombs += 1;
            Score += 1000;
        }

        public void Collect(ExtraLife life)
        {
            Lives += 1;
            Score += 10000;
        }

        public void Collect(PowerItem pitem)
        {
            Score += 10 * itemsCollected;
            if (power < powerMax)
            {
                power += 1;
            }

            if ((attackPtr < arsenalList.Count - 1 &&
                power > thresholds[attackPtr+1]) || 
                (power == powerMax &&
                attackPtr < thresholds.Count - 1))
            {
                attackPtr += 1;
                Assets.Sfx["powerup"].Play(Settings.Global.Audio.SfxVolume, 0, 0);
                this.arsenal = CurrentAttack;
                chooseAlignment(Alignment);
            }
        }

        public override void Draw3D(GraphicsDevice graphics, Matrix view, Matrix projection)
        {
            //arsenal.Draw(graphics, view, projection, Position);
            foreach (ModelMesh mesh in Surface.Meshes)
            {
                foreach (Effect currentEffect in mesh.Effects)
                {
                    currentEffect.Parameters["xPointLight2"].SetValue(Position);
                    currentEffect.Parameters["xPointIntensity2"].SetValue(1f);
                    Matrix world =
                        BaseMatrix *
                        Matrix.CreateRotationY(rollAngle) *
                        Matrix.CreateRotationX(-pitchAngle) *
                        Matrix.CreateTranslation(Position);
                    currentEffect.Parameters["xWorld"].SetValue(world);
                    currentEffect.Parameters["xView"].SetValue(view);
                    currentEffect.Parameters["xProjection"].SetValue(projection);
                    currentEffect.Parameters["xTexture"].SetValue(Texture);
                    
                }
                mesh.Draw();
            }

        }

        internal void BeginLevel()
        {
            this.script = deathScript;
            ActionPtr = 0;
        }

        internal void GivePoints(int points)
        {
            Score += points;
        }
        internal void GivePoints(Enemy enemy)
        {
            Score += enemy.BaseHealth;
        }
    }
}
