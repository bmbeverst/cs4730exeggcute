using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.assets;
using Exeggcute.src.console.commands;
using Exeggcute.src.entities;
using Exeggcute.src.entities.items;
using Exeggcute.src.graphics;
using Exeggcute.src.gui;
using Exeggcute.src.particles;
using Exeggcute.src.physics;
using Exeggcute.src.scripting.roster;
using Exeggcute.src.scripting.task;
using Exeggcute.src.text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace Exeggcute.src
{

    /// <summary>
    /// Base class for game "levels". If there is only one persistent
    /// level in our game, then that counts too.
    /// </summary>
    class Level : Sandbox
    {
        public string Name { get; protected set; }
        public Difficulty Difficulty { get; protected set; }

        /// <summary>
        /// If the user has made no customizations, we will save the
        /// high scores.
        /// </summary>
        public bool ValidScore { get; protected set; }
        private ParticleSystem particles;

        private PhysicsManager physics;
        private Roster roster;

        public static readonly int HalfWidth = 30;
        public static readonly int HalfHeight = 37;

        public List<TextBoxList> boxes = new List<TextBoxList>();

        private GrowBox shotEater;

        private Boss mainBoss;
        private Boss miniBoss;

        private Boss boss;

        private Song levelTheme;
        private Song bossTheme;

        public int initialScore;
        public bool IsStarted { get; protected set; }
        protected LightSettings lightSettings;

        public Level(GraphicsDevice graphics, 
                     ContentManager content, 
                     HUD hud,
                     Difficulty difficulty,
                     bool validScore,
                     string name,
                     Roster roster, 
                     Song levelTheme, 
                     Song bossTheme,
                     Boss miniBoss, 
                     Boss mainBoss,
                     List<Task> tasks, 
                     WangMesh terrain,
                     LightSettings lightSettings)
            : base(terrain)
        {
            MediaPlayer.IsVisualizationEnabled = true;
            //HACK HARDCODED
            Effect light = Assets.Effect["light0"];
            loadLights(lightSettings, light);
            this.Name        = name;
            this.Difficulty  = difficulty;
            this.ValidScore  = validScore;
            this.roster      = roster;
            this.taskList    = tasks;

            this.miniBoss    = miniBoss;
            this.mainBoss    = mainBoss;

            this.levelTheme = levelTheme;
            this.bossTheme = bossTheme;

            this.collider = new EntityManager();
            this.physics  = new PhysicsManager();

            this.lightSettings = lightSettings;

            //HARDCODED FIXME
            GameArea = new Rectangle(-HalfWidth, -HalfHeight, HalfWidth * 2, HalfHeight * 2);

            LiveArea = Util.GrowRect(GameArea, liveBuffer);
            particles = new TestParticleSystem(graphics, content);
            //TODO parse the player file here
            
            
        }



        public void Start()
        {
            Console.WriteLine("START {0}", Name);
            taskPtr = 0;
            initialScore = player.Score;
            hud.DoFade(FadeType.In);
            player.SetPosition(Engine.Jail);
            player.BeginLevel();
            Console.WriteLine(levelTheme.Name);
            Worlds.World.RequestPlay(levelTheme);
            miniBoss.AttachConversations(this);
            mainBoss.AttachConversations(this);
            IsStarted = true;
            loadLights(lightSettings, Assets.Effect["light0"]);


        }


        public static Level LoadFromFile(string filename)
        {
            return Worlds.World.LoadLevelFromFile(filename);
        }

        public override void AcceptCommand(ConsoleCommand command)
        {
            throw new NotImplementedException();
        }


        public override void Process(SongFadeTask task)
        {
            Worlds.World.DoFadeOut(task.NumFrames);
            taskPtr += 1;

        }

        public override void Process(SpawnTask task)
        {
            Enemy toSpawn = roster.Clone(task.ID, task.Position, task.Angle);
            Worlds.World.AddEnemy(toSpawn);
            taskPtr += 1;
        }

        public override void Process(BossTask bossTask)
        {
            if (boss == null)
            {
                boss = miniBoss;
            }
            taskPtr += 1;
        }

        public override void Update(ControlManager controls)
        {
            Update(controls, true);
        }

        public void Update(ControlManager controls, bool playerCanShoot)
        {
            //Console.WriteLine("{0} by {1}", Name, new StackFrame(3).GetMethod().DeclaringType.Name);
            if (!IsStarted) 
            { 
                Start(); 
                IsStarted = true; 
            }
            hud.Update();
            //camera.Update(controls);
            ProcessTasks();
            particles.Update();

            for (int i = 0; i < 1; i += 1)
            {
                if (player.Velocity.Equals(Vector3.Zero)) break;
                particles.AddParticle(player.Position, -10*player.Velocity);
            }

            physics.Affect(Worlds.World.GetGibList(), true);
            //physics.Affect(playerShots, false);
            foreach (Gib gib in Worlds.World.GetGibList())
            {
                collider.CollideTerrain(terrain, gib, GameArea);
            }

            physics.Affect(Worlds.World.GetDying(), true);
            collider.CollideDying(terrain);

            processHit();

            collider.Collide(player, Worlds.World.GetPlayerShots(), Worlds.World.GetEnemies());
            collider.CollideItems(Worlds.World.GetItemList(), player);


            //collider.UpdateAll(LiveArea);


            //player.Update(controls, playerCanShoot);
            player.LockPosition(GameArea);

            if (boss != null)
            {
                collider.CollideBoss(player, Worlds.World.GetPlayerShots(), boss);
                boss.Update();
            }
            base.Update(controls);
        }

        /// <summary>
        /// What to do when the player gets hit
        /// </summary>
        private void processHit()
        {
            bool hit = collider.CollidePlayer(player, Worlds.World.GetEnemies()) ||
                       collider.HitPlayer(Worlds.World.GetEnemyShots(), player);
            if (hit)
            {
                player.Kill();
                shotEater = new GrowBox(1);
            }
            if (shotEater != null)
            {
                shotEater.Update();
                collider.EatShots(Worlds.World.GetEnemyShots(), shotEater.Rect);
                if (shotEater.Rect.Height > HalfHeight * 4)
                {
                    shotEater = null;
                }
            }
        }

        public override void Draw3D(GraphicsDevice graphics, Camera camera)
        {
            Matrix view = camera.GetView();
            Matrix projection = camera.GetProjection();

            if (boss != null)
            {
                boss.Draw3D(graphics, view, projection);
            }

            base.Draw3D(graphics, camera);

            particles.SetCamera(view, projection);
            particles.Draw(graphics);
        }

        public override void Draw2D(SpriteBatch batch)
        {
            hud.Draw(batch, player);
            if (boss != null) boss.Draw2D(batch);
            base.Draw2D(batch);
        }

        bool cleanupStarted;
        public bool DoneCleanup()
        {
            if (!cleanupStarted)
            {
                hud.DoFade(FadeType.Out);
                cleanupStarted = true;
            }
            if (!hud.IsFading())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void StartBoss()
        {
            this.boss = mainBoss;
        }

        public void EndBossIntro(Boss boss)
        {
            Worlds.World.RequestPlay(bossTheme);
        }

        public override void Unload()
        {
            boss = null; 
            shotEater = null;
            miniBoss.Reset();
            mainBoss.Reset();
            taskPtr = 0;
            IsStarted = false;
        }

        public override void Dispose()
        {
            Unload();
        }


        private void loadLights(LightSettings settings, Effect effect)
        {
            effect.CurrentTechnique = effect.Techniques["Textured"];


            if (settings.Point1On.Value)
            {
                effect.Parameters["xPointLight1"].SetValue(settings.Point1Pos.Value);
                effect.Parameters["xPointIntensity1"].SetValue(settings.Point1Level.Value);
                effect.Parameters["xPointIntensity1"].SetValue(settings.Point1Level.Value);

            }
            if (settings.DirOn.Value)
            {
                effect.Parameters["xLightDirection"].SetValue(settings.DirDirection.Value);
                effect.Parameters["xDirLightIntensity"].SetValue(settings.DirLevel.Value);
            }
            if (settings.SpotOn.Value)
            {
                effect.Parameters["xSpotPos"].SetValue(settings.SpotPos.Value);
                effect.Parameters["xSpotDir"].SetValue(settings.SpotDir.Value);
                effect.Parameters["xSpotInnerCone"].SetValue(settings.SpotInner.Value);
                effect.Parameters["xSpotOuterCone"].SetValue(settings.SpotOuter.Value);
                effect.Parameters["xSpotRange"].SetValue(settings.SpotRange.Value);
                effect.Parameters["xSpotIntensity"].SetValue(settings.SpotLevel.Value);
            }

            effect.Parameters["xAmbient"].SetValue(settings.Ambient.Value);

        }
    }
}
