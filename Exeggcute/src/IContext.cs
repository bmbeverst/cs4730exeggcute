using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Exeggcute.src
{
    interface IContext
    {
        void Update(ControlManager controls);
        void Draw(GraphicsDevice graphics, SpriteBatch batch);
        void Load(ContentManager content);
        void Unload();
        void Dispose();
    }
}
