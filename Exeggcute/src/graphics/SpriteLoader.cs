using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Exeggcute.src.assets;
using Exeggcute.src.scripting;

namespace Exeggcute.src.graphics
{
    class SpriteLoader
    {
        /// <summary>encloses a vector2 entry from the left side</summary>
        private const char LDELIM = '(';

        /// <summary>encloses a vector2 entry from the right side</summary>
        private const char RDELIM = ')';

        /// <summary>separates the two integer values of a vector2</summary>
        private const char POINT_SEP = ',';

        /// <summary>separates the list of integers in the order line</summary>
        private const char ORDER_SEP = ' ';

        /// <summary>indicates the looping point for the animation</summary>
        private const char LOOP_IND = 'L';

        /// <summary>separates width from height on the dimensions line</summary>
        private const char DIM_SEP = 'x';

        /// <summary>file extension for sprites</summary>
        private const string EXT = "sprite";

        /// <summary>
        /// Loads a sprite from a .sprite file.
        /// </summary>
        /// <typeparam name="TSprite">the type of sprite expected</typeparam>
        /// <param name="name">the filename minus extension</param>
        public static Sprite Load(SpriteName name)
        {
            string filepath = String.Format("ExeggcuteContent/{0}.{1}", name, EXT);
            List<string> lines = Util.StripComments(filepath, '#', true);
            lines.Reverse(); //???
            Stack<string> lineStack = new Stack<string>(lines);
            List<IAnimation> anims = new List<IAnimation>();
            while (lineStack.Count != 0)
            {
                string textureString = lineStack.Pop();
                //Console.WriteLine(textureString);
                TextureName textureName = Util.ParseEnum<TextureName>(textureString);
                string sizeString = lineStack.Pop();
                //Console.WriteLine(sizeString);
                string[] size = sizeString.Split(DIM_SEP);
                int width = int.Parse(size[0]);
                int height = int.Parse(size[1]);
                string framesString = lineStack.Pop();
                //Console.WriteLine(framesString);
                Point[] frames = parseFrames(framesString);
                if (frames.Length == 1)
                {
                    // I need a cast? really??
                    anims.Add(new StaticAnimation(textureName, frames[0], width, height));
                    continue;
                }
                //else, animated sprite
                string orderString = lineStack.Pop();

                bool loop;
                int loopAt;
                int[] order = parseOrder(orderString, out loop, out loopAt);

                string speedString = lineStack.Pop();
                int speed = int.Parse(speedString);
                anims.Add(new Animation(textureName, width, height, frames, order, speed, loop, loopAt));

            }
            if (anims.Count == 0) throw new ParseError("No animations found");
            if (anims.Count == 1 && anims[0] is StaticAnimation)
            {
                //This is guaranteed safe because we checked it first
                StaticAnimation staticAnim = (StaticAnimation)anims[0];
                return new StaticSprite(staticAnim);
            }
            else
            {
                return new AnimatedSprite(anims);
            }
        }

        /// <summary>
        /// This function assumes that the order line is a space separated
        /// list of integers, with the loop position (if specified) preceeded
        /// directly by LOOP_IND.
        /// Example: '0 1 2 L3 4' 
        /// </summary>
        private static int[] parseOrder(string line, out bool loop, out int loopAt)
        {
            loopAt = 0;
            loop = false;
            try
            {
                List<int> result = new List<int>();
                string sanitized = Util.FlattenSpace(line);
                string[] tokens = sanitized.Split(' ');
                for (int i = 0; i < tokens.Length; i += 1)
                {
                    string current = tokens[i];
                    if (current[0] == LOOP_IND)
                    {
                        loop = true;
                        current = current.Substring(1);
                    }
                    result.Add(int.Parse(current));
                }
                return result.ToArray();
            }
            catch
            {
                throw new ParseError("Failed to parse \"{0}\"", line);
            }
        }

        private static Point[] parseFrames(string line)
        {
            List<Point> result = new List<Point>();
            line = line.Replace(LDELIM, ' ').Replace(RDELIM, ' ');
            line = Util.FlattenSpace(line);
            string sanitized = Util.RemoveSpace(line);
            if (line.Length <= 1)
            {
                Util.Die("Failed to parse \"{0}\"", line);
            }
            string[] pairs = line.Split(' ');
            for (int i = 0; i < pairs.Length; i += 1)
            {
                result.Add(parsePoint(pairs[i]));
            }
            return result.ToArray();
        }

        private static Point parsePoint(string input)
        {
            try
            {
                string[] tokens = input.Split(POINT_SEP);
                int x = int.Parse(tokens[0]);
                int y = int.Parse(tokens[1]);
                return new Point(x, y);
            }
            catch
            {
                throw new ParseError("Failed to parse point from string \"{0}\"", input);
            }
        }

    }
}
