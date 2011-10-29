using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Media;

namespace Exeggcute.src.sound
{
    class SongPlayer
    {
        public bool Loop { get; protected set; }
        public float Volume { get; protected set; }
        public bool IsPlaying { get; protected set; }
        public bool IsFading { get; protected set; }
        public float Speed { get; protected set; }

        protected Song queuedSong;
        protected int fadeDirection;
        protected bool playQueue;
        protected Timer queueTimer;

        protected float maxVolume;

        public SongPlayer(bool loop, float volume, float fadeFrames)
        {
            this.Loop = loop;
            this.Volume = volume;
            this.maxVolume = volume;
            this.Speed = 1.0f / fadeFrames;
        }

        public void Play(Song song)
        {
            //return;
            MediaPlayer.IsRepeating = Loop;
            if (IsPlaying)
            {
                DoFade(-1);
                queuedSong = song;
            }
            else
            {
                IsPlaying = true;
                MediaPlayer.Play(song);
            }
        }

        public void Play(Song newSong, int buffer)
        {
            queueTimer = new Timer(buffer);
            if (!IsPlaying)
            {
                Util.Warn("Not already playing a song!");
            }
            
            Play(newSong);
        }

        public void DoFade(int dir)
        {
            fadeDirection = dir;
            IsFading = true;
        }

        public void DoFade(int dir, int duration)
        {
            fadeDirection = dir;
            IsFading = true;
            Speed = 1.0f / duration;
        }

        public void SetVolume(float vol)
        {
            this.Volume = vol;
            MediaPlayer.Volume = vol;
        }
        public void Update()
        {
            if (IsFading)
            {

                Volume += fadeDirection * Speed * maxVolume;
                MediaPlayer.Volume = Volume;

                if (isFadeDone())
                {
                    IsFading = false;
                    if (IsPlaying)
                    {
                        MediaPlayer.Stop();
                        IsPlaying = false;
                    }
                    if (queuedSong != null)
                    {
                        playQueue = true;
                    }
                    
                }
            }


            if (playQueue && queueTimer != null)
            {
                if (!queueTimer.IsDone)
                {
                    queueTimer.Increment();
                }
                else
                {
                    queueTimer = null;
                    MediaPlayer.Play(queuedSong);
                    MediaPlayer.Volume = maxVolume;

                    Volume = maxVolume;
                    IsPlaying = true;
                    playQueue = false;
                }
            }

        }

        protected bool isFadeDone()
        {
            if (fadeDirection == -1 && Volume <= 0)
            {
                Volume = 0;
                return true;
            }
            else if (fadeDirection == 1 && Volume >= maxVolume)
            {
                Volume = maxVolume;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
