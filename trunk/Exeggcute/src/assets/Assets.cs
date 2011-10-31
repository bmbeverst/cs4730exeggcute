using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Exeggcute.src.graphics;
using Exeggcute.src.scripting.arsenal;
using Exeggcute.src.loading;
using Exeggcute.src.scripting;
using Microsoft.Xna.Framework.Content;
using Exeggcute.src.sound;
using Exeggcute.src.entities;

namespace Exeggcute.src.assets
{
    static class Assets
    {
        public static XnaBank<SoundEffect> Sfx =
            new XnaBank<SoundEffect>("sfx", "xnb");

        public static XnaBank<SpriteFont> Font =
            new XnaBank<SpriteFont>("fonts", "xnb");

        public static XnaBank<Model> Model =
            new XnaBank<Model>("models", "xnb");

        public static XnaBank<Texture2D> Texture =
            new XnaBank<Texture2D>("sprites", "xnb");

        public static XnaBank<Song> Song =
            new XnaBank<Song>("songs", "xnb");

        public static XnaBank<Effect> Effect =
            new XnaBank<Effect>("shaders", "xnb");

        public static DataBank<Sprite> Sprite = 
            new DataBank<Sprite>("sprites", "sprite");

        public static DataBank<OptionInfo> Option =
            new DataBank<OptionInfo>("options", "option");

        public static DataBank<BodyInfo> Body =
            new DataBank<BodyInfo>("bodies", "body");

        public static DataBank<ItemInfo> Item =
            new DataBank<ItemInfo>("items", "item");

        public static DataBank<ItemBatch> ItemBatch =
            new DataBank<ItemBatch>("itembatches", "item");

        public static DataBank<Enemy> Enemy =
            new DataBank<Enemy>("enemies", "enemy");

        public static DataBank<Level> Level =
            new DataBank<Level>("levels", "level");

        public static DataBank<Player> Player =
            new DataBank<Player>("players", "player");

        public static DataBank<ScriptBase> Behavior =
            new DataBank<ScriptBase>("scripts/behaviors", "cl");

        public static DataBank<ScriptBase> Trajectory =
            new DataBank<ScriptBase>("scripts/trajectories", "traj");

        public static DataBank<ScriptBase> Spawn =
            new DataBank<ScriptBase>("scripts/spawns", "spawn");

        

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

        public static void LoadAll(ContentManager content)
        {

        }

        public static void UnloadAll(ContentManager content)
        {

        }
    }
}
