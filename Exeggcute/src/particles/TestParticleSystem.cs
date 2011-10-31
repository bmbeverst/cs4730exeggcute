using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Exeggcute.src.assets;


namespace Exeggcute.src.particles
{
    class TestParticleSystem : ParticleSystem
    {
        public TestParticleSystem(GraphicsDevice device, ContentManager content)
            : base(device, content)
        { 
        
        }

        protected override void InitializeSettings(ParticleSettings settings)
        {
            settings.Texture = Assets.Texture["dot"];
            settings.MaxParticles = 2400;
            settings.Duration = 1;
            settings.BlendState = BlendState.Additive;
        }
    }
}
