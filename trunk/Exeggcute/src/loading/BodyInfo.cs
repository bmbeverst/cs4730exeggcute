using Exeggcute.src.assets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Exeggcute.src.loading
{
#pragma warning disable 0649
    class BodyInfo : Loadable
    {
        public Model Model;
        public Texture2D Texture;
        public float? Scale;
        public float? Radius;
        public Vector3? Rotation;
        public float? Mass;

        public BodyInfo(string filename)
            : base(filename)
        {
            loadFromFile(filename, true);
            
        }


        public static BodyInfo LoadFromFile(string filename)
        {
            return new BodyInfo(filename);
        }

        public static BodyInfo Parse(string s)
        {
            return Assets.Body[s];
        }
    }
}
