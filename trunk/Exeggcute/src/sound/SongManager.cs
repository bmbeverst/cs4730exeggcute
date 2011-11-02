using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Media;
using Exeggcute.src.config;

namespace Exeggcute.src.sound
{
    class SongManager
    {
        public enum SongState
        {
            Off,
            Playing,
            FadeIn,
            FadeOut
        }

        public SongState State { get; protected set; }
        protected SongState nextState = SongState.Off;
        protected Timer fadeTimer;
        protected float maxVolume
        {
            get { return Settings.Global.Audio.MusicVolume; }
        }
        protected float volume
        {
            get { return MediaPlayer.Volume; }
            set { MediaPlayer.Volume = value; }
        }

        protected bool isPaused;

        public SongManager()
        {
            this.volume = maxVolume;
        }


        public void Pause()
        {
            MediaPlayer.Pause();
            isPaused = true;
        }

        public void Unpause()
        {
            if (!isPaused)
            {
                Util.Warn("Shouldnt unpause if you arent paused!");
            }
            isPaused = false;
            MediaPlayer.Resume();
        }

        public void Update(bool toggled)
        {
            if (toggled)
            {
                if (isPaused)
                {
                    MediaPlayer.Resume();
                    isPaused = false;
                }
                else
                {
                    MediaPlayer.Pause();
                    isPaused = true;
                }
            }
            if (isPaused) return;
            if (State == SongState.FadeIn)
            {
                if (fadeTimer.IncrUntilDone())
                {
                    volume = maxVolume;
                    fadeTimer = null;
                    State = SongState.Playing;
                }
                else //not done yet
                {
                    volume = (1.0f - fadeTimer.GetRatio()) * maxVolume;
                }
            }
            else if (State == SongState.FadeOut)
            {
                if (fadeTimer.IncrUntilDone())
                {
                    MediaPlayer.Stop();
                    volume = maxVolume;
                    fadeTimer = null;
                    State = SongState.Off;
                }
                else //not done yet
                {
                    
                    volume = (1.0f - fadeTimer.GetRatio()) * maxVolume;
                }
            }
            else
            {
                
            }
        }


        public void Play(Song song)
        {
            if (!(State == SongState.Off))
            {

            }
            State = SongState.Playing;
            MediaPlayer.Play(song);
        }

        public void Stop()
        {
            if (!(State == SongState.Playing || State == SongState.FadeIn))
            {
                Util.Warn("Illegal trsansision");
            }
            else
            {
                State = SongState.Off;
                MediaPlayer.Stop();
            }
        }

        public void ResetState()
        {
            volume = maxVolume;
            State = SongState.Off;
            MediaPlayer.Stop();
            isPaused = false;
            nextState = SongState.Off;
            fadeTimer = null;
        }

        public void FadeOut(int frames)
        {
            if (State != SongState.Playing)
            {
                Util.Warn("Cannot fade out unless playing!");
            }

            State = SongState.FadeOut;
            fadeTimer = new Timer(frames);
            volume = maxVolume;
        }

        public void FadeIn(Song song, int frames)
        {
            if (State != SongState.Off)
            {
                Util.Warn("Cannot fade out unless playing!");
            }

            nextState = SongState.FadeIn;
            fadeTimer = new Timer(frames);
            volume = 0;
            MediaPlayer.Play(song);
        }
    }
}
