using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.assets;
using Microsoft.Xna.Framework.Audio;
using Exeggcute.src.config;

namespace Exeggcute.src.sound
{
    /// <summary>
    /// A repeated sound is a shared sound effect instance. Owners of this
    /// object may request that the sound be played, but if it does not
    /// allow the sound to be played if it is already playing.
    /// </summary>
    class RepeatedSound
    {
        protected SoundEffect sound;
        protected int framesSincePlayed;
        protected int duration;
        protected float myVolume;
        protected float volume
        {
            get { return myVolume * Settings.Global.Audio.SfxVolume; }
        }

        public RepeatedSound(SoundEffect sound, int duration, float volume)
        {
            this.sound = sound;
            this.duration = duration/2;
            this.myVolume = volume;
        }

        protected RepeatedSound(float volume)
        {
            this.myVolume = volume;
        }

        public virtual void Play()
        {
            if (canPlay())
            {
                sound.Play(Settings.Global.Audio.SfxVolume, 0, 0);
            }
        }

        public virtual bool canPlay()
        {
            if (framesSincePlayed >= duration)
            {
                framesSincePlayed = 0;
                return true;
            }
            return false;
        }

        public void Update()
        {
            framesSincePlayed += 1;
        }

        public static RepeatedSound Parse(string s)
        {
            string[] tokens = s.Split(',');
            string name = tokens[0];
            if (!Assets.Sfx.ContainsKey(name) && Assets.Sfx.ContainsKey(name + "0"))
            {
                return new RandoSound(name, 1.0f);
            }
            return Assets.MakeRepeated(name);
        }

        public static int GetDuration(string name)
        {
            return (int)(Assets.Sfx[name].Duration.Milliseconds / 1000f) * Engine.FPS;
        }
    }
}
