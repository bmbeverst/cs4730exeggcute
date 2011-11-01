using System;
using System.Collections.Generic;
using Exeggcute.src.assets;
using Microsoft.Xna.Framework.Audio;

namespace Exeggcute.src.sound
{
    class RandoSound : RepeatedSound
    {
        protected SoundEffectInstance[] effects;
        protected static Random rng = new Random();

        protected const int MIN_SOUNDS = 3;
        protected const int MIN_SUGGESTED = 5;

        protected int[] order;
        protected int ptr = 0;

        protected int maxDuration;

        public RandoSound(string name, float volume)
            : base(volume)
        {
            List<SoundEffectInstance> all = new List<SoundEffectInstance>();

            int i = 0;
            for (string current = name + i; Assets.Sfx.ContainsKey(current); current = name + i)
            {
                int curDuration = GetDuration(current);
                maxDuration = Math.Max(maxDuration, curDuration);
                all.Add(Assets.Sfx[current].CreateInstance());
                i += 1;
            }
            if (i < 4)
            {
                Util.Warn("Sound \"{0}\" has less than the minimum recommended number of sounds for a RandoSound. Should have at least {1}, found {2}", name, MIN_SUGGESTED, i);
            }
            else if (i <= 2)
            {
                throw new ExeggcuteError("Not enough sound effects to make \"{0}\", need at least {1} only found {2}", name, MIN_SOUNDS, i);
            }

            effects = all.ToArray();

            order = new int[i * 100];
            generateOrder();
        }

        public override void Play()
        {
            int index = order[ptr];
            effects[index].Play();
            ptr += 1;
            ptr %= order.Length;
        }

        protected void generateOrder()
        {
            int prev = 0;
            int prevprev = 0;
            int next = 0;
            List<int> possible = getPossible(prev, prevprev);
            for (int i = 0; i < order.Length; i += 1)
            {
                possible = getPossible(next, prev);
                prevprev = prev;
                prev = next;

                next = Util.GetRandomFrom(possible, rng);
                order[i] = next;
            }
            for (int i = 0; i < order.Length; i += 1)
            {
                prevprev = prev;
                prev = next;
                next = order[i];
                if (next == prev || next == prevprev)
                {
                    //Util.Warn("you are doing it wrong");
                }
            }
        }
        protected List<int> getPossible(int prev, int prevprev)
        {
            List<int> result = new List<int>();
            for (int i = 0; i < effects.Length; i += 1)
            {
                if (i == prev || i == prevprev) continue;
                result.Add(i);
            }
            return result;
        }


    }
}
