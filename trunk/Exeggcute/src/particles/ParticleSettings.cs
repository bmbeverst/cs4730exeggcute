
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Exeggcute.src.assets;


namespace Exeggcute.src.particles
{
    class ParticleSettings
    {
        public Texture2D Texture = TextureBank.Get("dot");
        public int MaxParticles = 1000;
        public float Duration = 1;
        public BlendState BlendState = BlendState.NonPremultiplied;
    }
}
