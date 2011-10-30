using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.Text.RegularExpressions;
using Exeggcute.src.assets;
using Exeggcute.src.scripting;
using Exeggcute.src.contexts;
using Exeggcute.src.scripting.arsenal;
using Exeggcute.src.text;
using Exeggcute.src.loading;
using Microsoft.Xna.Framework.Audio;
using Exeggcute.src.sound;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace Exeggcute.src.entities
{
    class Boss : ParentEntity
    {
        protected BehaviorScript entryScript;
        protected BehaviorScript defeatScript;
        protected BehaviorScript deathScript;

        protected Conversation intro;
        protected Conversation outro;

        protected Timer buffer = new Timer(20);

        protected bool introStarted;
        protected bool outroStarted;

        protected int spellPtr;

        protected Timer currentTimer;
        protected string currentName;

        protected bool isDying;
        protected bool isDefeated;
        protected bool isStarted;
        protected List<Spellcard> attacks;

        protected string name;

        protected RepeatedSound hurtSound;
        protected Level level;

        protected int maxHealth;

        public HealthBar HealthMeter { get; protected set; }

        public Boss(string name, 
                    Model model, 
                    Texture2D texture, 
                    float scale, 
                    float radius,
                    Vector3 rotation, 
                    Conversation intro, 
                    Conversation outro, 
                    RepeatedSound hurtSound,
                    BehaviorScript entryScript, 
                    BehaviorScript defeatScript, 
                    BehaviorScript deathScript, 
                    List<Spellcard> attacks)
            : base(model, texture, scale, radius, rotation, null, World.EnemyShots, World.GibList)
        {
            
            this.hurtSound = hurtSound;
            this.name = name;
            this.spellPtr = -1;
            this.intro = intro;
            this.outro = outro;
            this.entryScript = entryScript;
            this.defeatScript = defeatScript;
            this.deathScript = deathScript;
            this.script = entryScript;
            this.attacks = attacks;
            this.arsenal = Arsenal.None;
        }
        bool songStarted = false;
        public override void Update()
        {
            if (HealthMeter != null) HealthMeter.Update(Health);
            base.Update();
            hurtSound.Update();
            if (isStarted && !isDefeated)
            {
                if (Health <= 0 || currentTimer.IsDone)
                {
                    currentTimer.Reset();
                    LoadNext();
                }
                else
                {
                    currentTimer.Increment();
                }
            }
            else if (isDefeated && !isDying)
            {
                if (!outroStarted)
                {
                    World.PushContext(outro);
                    World.DoFadeOut(120);
                    outroStarted = true;
                }
                if (outro.IsDone)
                {
                    if (buffer.IsDone && !isDying)
                    {
                        isDying = true;
                        //do whatever here
                        buffer.Reset();
                        script = deathScript;
                        actionPtr = 0;
                    }
                    else
                    {
                        buffer.Increment();
                    }
                }
            }
            else if (isDying)
            {
                if (actionPtr == script.Count)
                {
                    World.CleanupLevel();
                }
            }
            else
            {
                if (!introStarted)
                {
                    
                    World.PushContext(intro);
                    introStarted = true;
                }
                if (intro.IsDone)
                {
                    if (buffer.IsDone)
                    {
                        buffer.Reset();
                        Start();
                    }
                    else
                    {
                        buffer.Increment();
                    }
                }
            }
        }

        public void AttachConversations(Level parent)
        {
            intro.AttachParent(parent);
            outro.AttachParent(parent);
            this.level = parent;
        }

        public void Start()
        {
            isStarted = true;
            level.EndBossIntro(this);
            LoadNext();
        }
        
        public override void Collide(Shot shot)
        {
            if (isStarted)
            {
                hurtSound.Play();
                base.Collide(shot);
            }
        }

        public void LoadNext()
        {
            spellPtr += 1;
            
            if (spellPtr == attacks.Count)
            {
                die();
                return;
            }
            Console.WriteLine("Starting {0}", spellPtr);
            Spellcard next = attacks[spellPtr];
            this.arsenal      = next.Attack;
            this.currentTimer = next.TimeLimit;
            this.currentName  = next.Name;
            this.maxHealth    = next.Health;
            this.Health       = next.Health;
            this.HealthMeter  = new HealthBar(maxHealth, 500, 5, currentTimer);
            this.script       = next.Behavior;
            actionPtr = 0;
            waitCounter = 0;
        }

        protected void die()
        {
            script = defeatScript;

            actionPtr = 0;
            isDefeated = true;
            arsenal = Arsenal.None;
        }

        public override void Reset()
        {
            buffer.Reset();
            introStarted = false;
            outroStarted = false;
            spellPtr = -1;
            isDying = false;
            isDefeated = false;
            isStarted = false;
            for (int i = 0; i < attacks.Count; i += 1)
            {
                attacks[i].Reset();
            }
            actionPtr = 0;
            waitCounter = 0;
            arsenal = Arsenal.None;
            intro.Reset();
            outro.Reset();
        }

        public static Boss Parse(string name)
        {
            BossInfo info = new BossInfo(name);
            return info.MakeBoss();
        }

        public void Draw2D(SpriteBatch batch)
        {
            if (HealthMeter != null) HealthMeter.Draw(batch, new Vector2(0, 0));
        }

        
    }
}
