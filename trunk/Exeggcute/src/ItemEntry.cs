using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Exeggcute.src.scripting;
using Exeggcute.src.entities.items;
using Exeggcute.src.loading;

namespace Exeggcute.src
{
#pragma warning disable 0649
    class ItemEntry : Loadable
    {
        public BodyInfo Body;
        public BehaviorScript Behavior;
        public ItemType? Type;

        public ItemEntry(string filename)
            : base(filename)
        {
            loadFromFile(filename);
        }

        public Item MakeItem()
        {
            ItemType type = Type.Value;
            if (type == ItemType.ExtraLife)
            {
                return new ExtraLife(Body.Model,
                                     Body.Texture,
                                     Body.Scale.Value,
                                     Body.Radius.Value,
                                     Body.Rotation.Value,
                                     Behavior);
            }
            else if (type == ItemType.Power)
            {
                return new PowerItem(Body.Model,
                                     Body.Texture,
                                     Body.Scale.Value,
                                     Body.Radius.Value,
                                     Body.Rotation.Value,
                                     Behavior);
            }
            throw new NotImplementedException();
            
        }

    }
}
