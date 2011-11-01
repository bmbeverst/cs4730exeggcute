using Exeggcute.src.entities;

namespace Exeggcute.src.loading
{
#pragma warning disable 0649
    class GibInfo : Loadable
    {
        protected BodyInfo body;

        public GibInfo(string name)
            : base(getFilename(name))
        {
            string filename = getFilename(name);
            loadFromFile(filename, true);
        }

        public Gib MakeGib()
        {
            return new Gib(body.Model, body.Texture, body.Scale.Value, body.Radius.Value, body.Rotation.Value);
        }

        public static string getFilename(string name)
        {
            return string.Format("data/bodies/{0}.body", name);
        }
    }
}
