using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.entities;
using Exeggcute.src.graphics;
using Exeggcute.src.loading;
using Exeggcute.src.loading.specs;
using Exeggcute.src.scripting;
using Exeggcute.src.sound;
using Exeggcute.src.text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Exeggcute.src.assets
{
    static class Assets
    {
        public static XnaBank<SoundEffect> Sfx;
        public static XnaBank<SpriteFont> Font;
        public static XnaBank<Model> Model;
        public static XnaBank<Texture2D> Texture;
        public static XnaBank<Song> Song;
        public static XnaBank<Effect> Effect;

        public static DataBank<Sprite> Sprite;
        public static DataBank<OptionInfo> Option;
        public static DataBank<BodyInfo> Body;
        public static DataBank<ItemInfo> Item;
        public static DataBank<ItemBatch> ItemBatch;
        public static DataBank<Enemy> Enemy;
        public static DataBank<Level> Level;
        public static DataBank<Boss> Boss;
        public static DataBank<Player> Player;
        public static DataBank<Conversation> Conversation;
        public static DataBank<ScriptBase> Behavior;
        public static DataBank<ScriptBase> Trajectory;
        public static DataBank<ScriptBase> Spawn;


        public static BehaviorScript GetBehavior(string name)
        {
            return new BehaviorScript(Behavior[name]);
        }

        public static TrajectoryScript GetTrajectory(string name)
        {
            return new TrajectoryScript(Trajectory[name]);
        }

        public static SpawnScript GetSpawn(string name)
        {
            return new SpawnScript(Spawn[name]);
        }

        public static bool ScriptExists(string name)
        {
            return Spawn.ContainsKey(name) ||
                   Trajectory.ContainsKey(name) ||
                   Behavior.ContainsKey(name);
        }

        private static Dictionary<string, RepeatedSound> repeatedSoundCache =
            new Dictionary<string, RepeatedSound>();

        public static RepeatedSound MakeRepeated(string name, int duration=-1)
        {
            if (repeatedSoundCache.ContainsKey(name))
            {
                return repeatedSoundCache[name];
            }
            else
            {
                float volume = 1.0f;
                SoundEffect sound = Sfx[name];
                if (duration == -1)
                {
                    duration = (int)(sound.Duration.Milliseconds / 1000f) * Engine.FPS;
                }
                RepeatedSound repeated = new RepeatedSound(sound, duration, volume);
                repeatedSoundCache[name] = repeated;
                return repeated;
            }
        }

        public static void UpdateSfx()
        {
            foreach (RepeatedSound sound in repeatedSoundCache.Values)
            {
                sound.Update();
            }
        }

        public static void Reset()
        {
            ResetXna();
            ResetData();

        }

        public static void ResetXna()
        {
            Sfx = new XnaBank<SoundEffect>("sfx", "xnb");
            Font = new XnaBank<SpriteFont>("fonts", "xnb");
            Model = new XnaBank<Model>("models", "xnb");
            Texture = new XnaBank<Texture2D>("sprites", "xnb");
            Song = new XnaBank<Song>("songs", "xnb");
            Effect = new XnaBank<Effect>("shaders", "xnb");
        }

        public static void ResetData()
        {
            Sprite = new DataBank<Sprite>("sprites", "sprite");
            Option = new DataBank<OptionInfo>("options", "option");
            Body = new DataBank<BodyInfo>("bodies", "body");
            Item = new DataBank<ItemInfo>("items", "item");
            ItemBatch = new DataBank<ItemBatch>("itembatches", "item");
            Enemy = new DataBank<Enemy>("enemies", "enemy");
            Level = new DataBank<Level>("levels", "level");
            Boss = new DataBank<Boss>("bosses", "boss");
            Player = new DataBank<Player>("players", "player");
            Conversation = new DataBank<Conversation>("convo", "txt");
            Behavior = new DataBank<ScriptBase>("scripts/behaviors", "cl");
            Trajectory = new DataBank<ScriptBase>("scripts/trajectories", "traj");
            Spawn = new DataBank<ScriptBase>("scripts/spawns", "spawn");
        }

        public static void LoadAll(ContentManager content)
        {
            LoadXna(content);
            LoadData();     
        }

        public static void LoadXna(ContentManager content)
        {
            Texture.LoadAll(content);//must be first
            Font.LoadAll(content);
            Effect.LoadAll(content);
            Model.LoadAll(content);
            Sfx.LoadAll(content);
            Song.LoadAll(content);
        }
        
        public static void LoadData()
        {
            //depends texture
            Sprite.LoadAll();

            //model, texture
            Body.LoadAll();
            
            //none
            Behavior.LoadAll();

            //none
            Trajectory.LoadAll();

            //none            
            Spawn.LoadAll();

            
            //body, behavior
            Item.LoadAll();

            //item
            ItemBatch.LoadAll();

            //BodyInfo
            //Sfx
            //Behavior
            //Trajectory
            //Spawn
            Option.LoadAll();

            //Font
            Conversation.LoadAll();

            //BehaviorScript, ItemBatch
            //Model Texture2D Arsenal BehaviorScript
            //RepeatedSound ItemBatch GibBatch Alignment
            Enemy.LoadAll();

            //Model
            //Texture2D
            //BehaviorScript
            //Arsenal
            //GibBatch
            //RepeatedSound
            //Arsenal
            Player.LoadAll();

            //Model model, 
            //Texture2D texture, 
            //Conversation intro, 
            //Conversation outro, 
            //RepeatedSound hurtSound,
            //BehaviorScript entryScript, 
            //Spellcard
            Boss.LoadAll();


            //HUD
            //Roster
            //Song bossTheme,
            //Boss mainBoss,
            //WangMesh
            Level.LoadAll();
        }




    }
}
