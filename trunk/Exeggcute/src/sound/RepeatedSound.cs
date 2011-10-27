﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Exeggcute.src.assets;

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
        protected float volume;

        public RepeatedSound(SoundEffect sound, int duration, float volume)
        {
            this.sound = sound;
            this.duration = duration/2;
            this.volume = volume;
        }

        protected RepeatedSound(float volume)
        {
            this.volume = volume;
        }

        public virtual void Play()
        {
            if (canPlay())
            {
                sound.Play();
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
            float volume = 1.0f;
            string[] tokens = s.Split(',');
            string name = tokens[0];
            if (!SfxBank.Contains(name) && SfxBank.Contains(name + "0"))
            {
                return new RandoSound(name, volume);
            }
            return SfxBank.MakeRepeated(name);
            SoundEffect sound = SfxBank.DeprecatedGetSound(name);
            int duration = SfxBank.GetDuration(name);
            return new RepeatedSound(sound, duration, volume);
        }
    }
}
