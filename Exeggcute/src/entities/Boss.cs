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


        protected int spellPtr;

        protected Timer currentTimer;
        protected string currentName;

        protected bool isDying;
        protected bool started;
        protected List<Spellcard> attacks;
        public Boss(Model model, Conversation intro, Conversation outro, BehaviorScript entryScript, BehaviorScript defeatScript, BehaviorScript deathScript, List<Spellcard> attacks)
            : base(model, World.EnemyShots, World.GibList)
        {

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

        public override void Update()
        {
            base.Update();
            if (started)
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
                if (isDying && actionPtr == deathScript.Count)
                {
                    Util.Die("explode");
                    World.Process(new BossDeadEvent());
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
        public void Start()
        {
            started = true;
            LoadNext();
        }
        public void LoadNext()
        {
            spellPtr += 1;
            Spellcard next = attacks[spellPtr];
            if (spellPtr == attacks.Count - 1)
            {
                //die();
            }

            this.arsenal      = next.Attack;
            this.currentTimer = next.TimeLimit;
            this.currentName  = next.Name;
            this.Health       = next.Health;
            this.script       = next.Behavior;
            actionPtr = 0;
            counter = 0;
        }

        protected void die()
        {
            script = deathScript;
            actionPtr = 0;
            isDying = true;
        }



        
    }
}
