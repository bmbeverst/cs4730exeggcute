using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Media;

namespace Exeggcute.src
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

        public SongPlayer(bool loop, float volume, float fadeFrames)
        {
            this.Loop = loop;
            this.Volume = volume;
            this.Speed = 1.0f / fadeFrames;
        }

        public void Play(Song song)
        {
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

        public void Update()
        {
            if (IsFading)
            {
                Volume += fadeDirection * Speed;
                MediaPlayer.Volume = Volume;
                
                if (isFadeDone())
                {
                    IsFading = false;
                    if (IsPlaying)
                    {
                        MediaPlayer.Stop();
                    }
                    if (queuedSong != null)
                    {
                        playQueue = true;
                    }
                    IsPlaying = !IsPlaying;
                }
            }


            if (playQueue)
            {
                if (!queueTimer.IsDone)
                {
                    queueTimer.Increment();
                }
                else
                {
                    queueTimer = null;
                    MediaPlayer.Play(queuedSong);
                    MediaPlayer.Volume = 1;
                    Volume = 1;
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
            else if (fadeDirection == 1 && Volume >= 1)
            {
                Volume = 1;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
