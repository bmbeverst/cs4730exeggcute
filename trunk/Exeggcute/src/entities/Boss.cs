﻿using System;
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
        protected bool outroStarted;


        protected int spellPtr;

        protected Timer currentTimer;
        protected string currentName;

        protected bool isDying;
        protected bool isDefeated;
        protected bool isStarted;
        protected List<Spellcard> attacks;
        
        public Boss(Model model, float modelScale, Conversation intro, Conversation outro, BehaviorScript entryScript, BehaviorScript defeatScript, BehaviorScript deathScript, List<Spellcard> attacks)
            : base(model, World.EnemyShots, World.GibList)
        {

            this.spellPtr = -1;
            this.intro = intro;
            this.outro = outro;
            this.Scale = modelScale;
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
        public void Start()
        {
            isStarted = true;
            LoadNext();
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
            this.Health       = next.Health;
            this.script       = next.Behavior;
            actionPtr = 0;
            counter = 0;
        }

        protected void die()
        {
            script = defeatScript;

            actionPtr = 0;
            isDefeated = true;
            arsenal = Arsenal.None;
        }

        public void Reset()
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
            counter = 0;
            arsenal = Arsenal.None;
            intro.Reset();
            outro.Reset();
        }

        
    }
}