using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.entities.items;
using Exeggcute.src.loading;
using Exeggcute.src.scripting;

namespace Exeggcute.src
{
#pragma warning disable 0649
    class ItemInfo : Loadable
    {
        public BodyInfo Body;
        public BehaviorScript Behavior;
        public ItemType? Type;

        public ItemInfo(string filename)
            : base(filename)
        {
            loadFromFile(filename, true);
        }

        public static ItemInfo LoadFromFile(string filename)
        {
            return new ItemInfo(filename);
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
            else if (type == ItemType.ExtraBomb)
            {
                return new ExtraBomb(Body.Model,
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
            else if (type == ItemType.Point)
            {
                return new PointItem(Body.Model,
                                     Body.Texture,
                                     Body.Scale.Value,
                                     Body.Radius.Value,
                                     Body.Rotation.Value,
                                     Behavior);
            }
            else
            {
                World.ConsoleWrite("Item type \"{0}\" is not implemented", type);
                //FIXME this will break things
                return null;
            }
        }

    }
}
