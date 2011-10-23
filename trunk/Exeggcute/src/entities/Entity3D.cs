using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Exeggcute.src.assets;

namespace Exeggcute.src.entities
{
    abstract class Entity3D : Entity
    {
        public virtual Vector3 Position { get; protected set; }
        public virtual Model Surface { get; protected set; }
        public virtual BoundingSphere OuterHitbox { get; protected set; }

        public float ModelRotX { get; protected set; }

        

        public Entity3D(Model model, Vector3 pos)
        {
            Surface = model;
            /*foreach (ModelMesh mesh in Surface.Meshes)
            {
                
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    Effect currentEffect = EffectBank.Get(EffectName.light0);
                    Texture2D texture = TextureBank.Get(TextureName.fractal);
                    currentEffect.Parameters["xEnableLighting"].SetValue(true);
                    currentEffect.Parameters["xAmbient"].SetValue(0.3f);
                    currentEffect.Parameters["xLightDirection"].SetValue(new Vector3(-1.0f, -0.5f, 1.0f));
                    currentEffect.Parameters["xDirLightIntensity"].SetValue(1.0f);

                    //begin area light
                    currentEffect.Parameters["xPointLight1"].SetValue(new Vector3(0f, 0f, 0f));
                    currentEffect.Parameters["xPointIntensity1"].SetValue(10.0f);
                    currentEffect.Parameters["xPointLight2"].SetValue(new Vector3(94.0f, 10.0f, -87.0f));
                    currentEffect.Parameters["xPointIntensity2"].SetValue(10.0f);
                    currentEffect.Parameters["xPointLight3"].SetValue(new Vector3(90.0f, 10.0f, -87.0f));
                    currentEffect.Parameters["xPointIntensity3"].SetValue(10.0f);
                    //end area light

                    currentEffect.Parameters["xPointLight4"].SetValue(new Vector3(19.0f, 0.5f, -93.0f));
                    currentEffect.Parameters["xPointIntensity4"].SetValue(20.0f);
                    currentEffect.Parameters["xSpotPos"].SetValue(new Vector3(10.0f, 5.0f, -46.0f));
                    currentEffect.Parameters["xSpotDir"].SetValue(new Vector3(50.0f, -1.0f, 0.0f));
                    currentEffect.Parameters["xSpotInnerCone"].SetValue(0.3490f);
                    currentEffect.Parameters["xSpotOuterCone"].SetValue(0.6981f);
                    currentEffect.Parameters["xSpotRange"].SetValue(13.0f);
                    currentEffect.Parameters["xTexture"].SetValue(texture);
                    //part.Effect = EffectBank.Get(EffectName.light0);
                }
            }*/
            Position = pos;
            OuterHitbox = Util.MergeSpheres(Surface.Meshes);
            OuterHitbox = new BoundingSphere(Position, OuterHitbox.Radius);
        }

        public Entity3D(Vector3 pos)
        {
            Position = pos;
        }


        public void SetPosition(Vector3 newpos)
        {
            Position = newpos;
        }






        public virtual void Update()
        {
            OuterHitbox = new BoundingSphere(Position, OuterHitbox.Radius);
        }

        public abstract void Draw(GraphicsDevice graphics, Matrix view, Matrix projection);

    }
}
