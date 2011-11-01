using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace Exeggcute.src.loading
{
#pragma warning disable 0649
    class ConversationInfo : Loadable
    {
        public SpriteFont Font;
        public float? Rate;
        public ConversationInfo(string filename, List<string[]> tokens)
            : base(filename)
        {
            loadFromTokens(tokens, true);
        }
    }
}
