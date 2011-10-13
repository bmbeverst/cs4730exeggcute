﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Exeggcute.src.assets;

namespace Exeggcute.src.entities
{
    abstract class Entity3D 
    {
        public virtual Vector3 Position { get; protected set; }
        public virtual Model Surface { get; protected set; }
        public virtual ModelName Name { get; protected set; }
        public virtual BoundingSphere Hitbox { get; protected set; }

        public virtual float X
        {
            get { return Position.X; }
            protected set { Position = new Vector3(value, Position.Y, Position.Z); }
        }

        public virtual float Y
        {
            get { return Position.Y; }
            protected set { Position = new Vector3(Position.X, value, Position.Z); }
        }

        public virtual float Z
        {
            get { return Position.Z; }
            protected set { Position = new Vector3(Position.X, Position.Y, value); }
        }

        public Entity3D(ModelName name, Vector3 pos)
        {
            Name = name;
            Surface = ModelBank.Get(name);
            Position = pos;
            Hitbox = Util.MergeSpheres(Surface.Meshes);
            Hitbox = new BoundingSphere(Position, Hitbox.Radius);
        }

        public void SetPosition(Vector3 newpos)
        {
            Position = newpos;
        }


        public virtual void Update()
        {
            Hitbox = new BoundingSphere(Position, Hitbox.Radius);
        }

        public virtual void Draw(GraphicsDevice graphics, Matrix view, Matrix projection)
        {
            Matrix[] transforms = new Matrix[Surface.Bones.Count];
            Surface.CopyAbsoluteBoneTransformsTo(transforms);
            foreach (ModelMesh mesh in Surface.Meshes)
            {
                foreach (BasicEffect currentEffect in mesh.Effects)
                {
                    //FIXME: absolutely no reason to do this every frame
                    currentEffect.World = transforms[mesh.ParentBone.Index] *
                        //Matrix.CreateRotationX(-MathHelper.PiOver2) *
                        Matrix.CreateTranslation(Position);
                    currentEffect.View = view;
                    currentEffect.Projection = projection;
                    /*currentEffect.CurrentTechnique = currentEffect.Techniques["Textured"];
                    currentEffect.Parameters["xWorld"].SetValue(world);
                    currentEffect.Parameters["xView"].SetValue(view);
                    currentEffect.Parameters["xProjection"].SetValue(projection);
                    currentEffect.Parameters["xTexture"].SetValue(texture);
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
                    currentEffect.Parameters["xSpotRange"].SetValue(13.0f);*/

                }
                mesh.Draw();
            }
        }
    }
}