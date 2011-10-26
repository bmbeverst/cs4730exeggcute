using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Exeggcute.src.scripting;
using Microsoft.Xna.Framework.Audio;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Exeggcute.src.loading
{



    class EntityInfo : LoadedInfo
    {
        public Model Surface { get; protected set; }
        public Texture2D Texture { get; protected set; }
        public float? Scale { get; protected set; }

        public EntityInfo(List<string> lines)
        {
            //Type t = 
        }

        public void SetMatch(string name, string value)
        {

        }
    }
    class EnemyInfo : EntityInfo
    {
        
        public int? Health{ get; protected set; } 
        public int? Defense{ get; protected set; } 
        public BehaviorScript deathScript{ get; protected set; } 
        public SoundEffect shootSFX{ get; protected set; } 
        public SoundEffect dieSFX{ get; protected set; } 
        public ItemBatch itembatch{ get; protected set; } 
        public GibBatch gibbatch{ get; protected set; }

        public EnemyInfo(List<string> lines)
            : base(lines)
        {

            
        }
    }
}
