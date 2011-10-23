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

namespace Exeggcute.src.entities
{
    class Boss : ParentEntity
    {
        public BehaviorScript DeathScript { get; protected set; }
        protected int spellPtr = 0;

        protected Timer currentTimer;
        protected Arsenal currentAttack;
        protected string currentName;

        protected bool isDying;
        protected List<Spellcard> attacks;
        public Boss(Model model, BehaviorScript deathScript, List<Spellcard> attacks)
            : base(model, World.EnemyShots, World.GibList)
        {

            spellPtr = -1;
            this.attacks = attacks;
            loadNext();
            

        }

        public override void Update()
        {
            if (Health <= 0)
            {
                loadNext();
            }
            if (isDying && actionPtr == DeathScript.Count)
            {
                World.Process(new BossDeadEvent());
            }
            base.Update();
        }

        protected void loadNext()
        {
            spellPtr += 1;
            Spellcard next = attacks[spellPtr];
            if (spellPtr == attacks.Count - 1)
            {
                die();
            }

            this.currentAttack = next.Attack;
            this.currentTimer  = next.TimeLimit;
            this.currentName   = next.Name;
            this.Health        = next.Health;
            this.script        = next.Behavior;
        }

        protected void die()
        {
            script = DeathScript;
            actionPtr = 0;
            isDying = true;
        }



        
    }
}
