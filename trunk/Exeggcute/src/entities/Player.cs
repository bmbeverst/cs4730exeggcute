using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Exeggcute.src.assets;
using Exeggcute.src.scripting;
using Exeggcute.src.scripting.action;
using Exeggcute.src.graphics;
using Exeggcute.src.scripting.arsenal;
using Exeggcute.src.entities.items;
using Microsoft.Xna.Framework.Audio;
using Exeggcute.src.sound;

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
        protected const int POWER_MAX = 128;
        protected float rollAngle;
        protected float pitchAngle;

        public float HitRadius { get; protected set; }
        public BoundingSphere InnerHitbox { get; protected set; }

        protected List<Arsenal> arsenalList;
        protected List<int> thresholds;
        protected int attackPtr;

        protected Arsenal currentAttack
        {
            get { return arsenalList[attackPtr]; }
        }

        public string RawData { get; protected set; }
        public string Name { get; protected set; }
        public bool IsCustom { get; protected set; }
        protected float lightLevel;


        public Player(string data,
                      string name,
                      bool isCustom,
                      Model model, 
                      Texture2D texture, 
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
                      float scale,
                      float hitRadius,
                      float lightLevel,
                      HashList<Shot> shotList, 
                      HashList<Gib> gibList)
            : base(model, texture, scale, deathScript, dieSFX, null, gibBatch, shotList, gibList)
        {
            this.RawData = data;
            this.Name = name;
            this.IsCustom = isCustom;
            this.arsenalList = arsenalList;
            this.thresholds = thresholds;
            this.attackPtr = 0;
            this.arsenal = arsenalList[attackPtr];

            this.lightLevel = lightLevel;

            this.graze = 0;
            this.score = 0;

            this.lives = numLives;
            this.bombs = numBombs;
            
            this.MoveSpeed = moveSpeed;
            this.FocusSpeed = focusSpeed;
            this.InvulnTimer = new Timer(120);
            this.bomb = bomb;
            this.HitRadius = hitRadius;
            this.InnerHitbox = new BoundingSphere(Position, HitRadius);
            this.deathScript = deathScript;


            LifeSprite = SpriteBank.Get("life");
            BombSprite = SpriteBank.Get("bomb");
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
        protected void processControls(ControlManager controls, bool canShoot)
        {
            //if (shotList.Count > 0) Util.Die("works");// Console.WriteLine("{0}", shotList.Count);
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
                //Angle = 0;
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
                Console.WriteLine("Begin bombing");
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
                    Console.WriteLine("End bombing");
                }
                else
                {
                    bombTimer.Increment();
                }
            }
            InvulnTimer.Increment();
            frames += 1;
            flashDraw = (IsInvulnerable && frames % 2 == 0);
            if (!CanControl && actionPtr == script.Count)
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
            InnerHitbox = new BoundingSphere(Position, InnerHitbox.Radius);
        }



        Sprite LifeSprite;
        Sprite BombSprite;
        public void DrawHUD(SpriteBatch batch, SpriteFont scoreFont)
        {
            for (int i = 0; i < lives; i += 1)
            {
                LifeSprite.Draw(batch, new Vector2(50, i * 40));
            }

            for (int i = 0; i < bombs; i += 1)
            {
                BombSprite.Draw(batch, new Vector2(100, i * 40));
            }
            //9 decimal places
            string scoreString = string.Format("Score {0:000,000,000}", score);
            batch.DrawString(scoreFont, scoreString, new Vector2(10, 120), Color.White);
            string grazeString = string.Format("Graze       {0:0,000}", graze);
            batch.DrawString(scoreFont, grazeString, new Vector2(10, 150), Color.White);
            string powerString = string.Format("Power         {0,3}", power);
            if (power >= POWER_MAX)
            {
                powerString = "Power       -MAX-";
            }
            batch.DrawString(scoreFont, powerString, new Vector2(10, 180), Color.White);
            
        }

        public override void Kill()
        {
            Console.WriteLine("KILLED!");
            script = deathScript;
            actionPtr = 0;
            lives -= 1;
            InvulnTimer.Reset();
            //spawn the death animation
        }



        public void Graze(Shot shot)
        {
            graze += 1;
            score += graze * graze;
        }

        public void Collect(Item item)
        {
            score += 100;
            Util.Warn("FIXME");
            //throw new InvalidOperationException("can only collect types of items");
        }

        public void Collect(ExtraLife life)
        {
            lives += 1;
            Util.Warn("FIXME");
        }

        public void Collect(PowerItem pitem)
        {
            power += 1;
            if (power > thresholds[attackPtr])
            {
                if (attackPtr < arsenalList.Count - 1)
                {
                    attackPtr += 1;
                }
                else
                {
                    //TODO
                    Util.Warn("FIXME");
                }
            }
            
        }

        public override void Draw(GraphicsDevice graphics, Matrix view, Matrix projection)
        {
            foreach (ModelMesh mesh in Surface.Meshes)
            {
                foreach (Effect currentEffect in mesh.Effects)
                {
                    currentEffect.Parameters["xPointLight2"].SetValue(Position);
                    currentEffect.Parameters["xPointIntensity2"].SetValue(1f);
                    Matrix world =
                        Matrix.CreateScale(Scale) *
                        Matrix.CreateRotationZ(MathHelper.PiOver2) *
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
    }
}
