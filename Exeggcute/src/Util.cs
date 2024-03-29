﻿using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;
using Exeggcute.src.assets;
using Exeggcute.src.scripting;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Exeggcute.src
{
    /// <summary>
    /// Miscellaneous utility functions
    /// </summary>
    internal static class Util
    {
        /// <summary>
        /// Contains the characters considered to be whitespace.
        /// </summary>
        internal static readonly char[] Whitespace = new char[] {
            ' ', '\t', '\r', '\n'
        };

        /// <summary>
        /// Returns the lines from a file, taking failure to be non-recoverable.
        /// </summary>
        internal static List<string> ReadLines(string filepath)
        {
            if (!File.Exists(filepath))
            {
                throw new ExeggcuteError("File \"{0}\" does not exist, or could not be found in {1}", filepath, Directory.GetCurrentDirectory());
            }
            return File.ReadLines(filepath).ToList();
        }

        internal static string ReadAllText(string filepath)
        {
            if (!File.Exists(filepath))
            {
                throw new ExeggcuteError("File \"{0}\" does not exist, or could not be found in {1}", filepath, Directory.GetCurrentDirectory());
            }
            return File.ReadAllText(filepath);
        }

        /// <summary>
        /// Used to issue warnings to the terminal.
        /// </summary>
        internal static void Warn(string message, params object[] args)
        {
            string formatted = String.Format(message, args);
            log.Add(formatted);
            Console.Error.WriteLine("WARNING: {0}", formatted);
        }

        /// <summary>
        /// Used to quit with an error message.
        /// </summary>
        internal static void Die(object message, params object[] args)
        {
            string msg;
            if (message == null)
            {
                msg = "null";
            }
            else if (args.Length == 0)
            {
                msg = message.ToString();
            }
            else
            {
                msg = string.Format(message.ToString(), args);
            }
            Console.Error.WriteLine(msg);
            Console.Error.WriteLine("Press enter to exit");
            Console.ReadLine();
            Environment.Exit(1);
        }

        /// <summary>
        /// Used to quit with an error message. Also fools C# into thinking
        /// that you're returning a value when one is expected.
        /// </summary>
        /// <typeparam name="T">The type that the calling method is expecting to return</typeparam>
        /// <returns>never</returns>
        internal static T Die<T>(string message, params object[] args)
        {
            Die(message, args);
            return default(T);
        }

        /// <summary>
        /// Writes the given string to the given file. If the file exists, 
        /// it will move that file to a temporary location before writing
        /// to ensure that data is never destroyed.
        /// </summary>
        internal static void WriteFile(string filepath, string data)
        {
            string tempname = filepath + "-temp";
            try
            {
                if (File.Exists(filepath))
                {
                    File.Move(filepath, tempname);
                }
                File.WriteAllText(tempname, data);
                File.Move(tempname, filepath);
            }
            catch (Exception ex)
            {
                throw new IOException(string.Format("{0}\nFailed to write data to file {1}", ex.Message, filepath));
            }
        }

        internal static void WriteFile(string filepath, List<string> lines)
        {
            string total = "";
            foreach (string line in lines)
            {
                total += line + '\n';
            }
            WriteFile(filepath, total);
        }

        internal static string MangleName(string name, string extra)
        {
            return string.Format("{0}&{1}", name, extra);
        }

        /// <summary>
        /// Splits a string based on newline characters and trims all 
        /// leading and trailing whitespace.
        /// </summary>
        internal static List<string> SplitLines(string allText, bool trimStart=true)
        {
            allText = Regex.Replace(allText, "\r\n", "\n");
            string[] splitted = Regex.Split(allText, "\n");
            for (int i = 0; i < splitted.Length; i += 1)
            {
                string trimmed;
                if (trimStart)
                {
                    trimmed = splitted[i].Trim(Whitespace);
                }
                else
                {
                    trimmed = splitted[i].TrimEnd(Whitespace);
                }

                splitted[i] = trimmed;
            }
            return splitted.ToList();
        }

        /// <summary>
        /// Returns a list of all buttons currently being pressed by player 1.
        /// Similar to keyboardState.GetPressedKeys, but the function does
        /// not exist for GamePadState for whatever reason.
        /// </summary>
        internal static Buttons[] GetPressedButtons(GamePadState gpState)
        {
            List<Buttons> pressed = new List<Buttons>();
            foreach (Buttons button in Enum.GetValues(typeof(Buttons)))
            {
                if (gpState.IsButtonDown(button))
                {
                    pressed.Add(button);
                }
            }
            return pressed.ToArray();
        }

        /// <summary>
        /// Returns true if the input is a power of two.
        /// </summary>
        internal static bool PowerOfTwo(int n)
        {
            return (n & (n - 1)) == 0;
        }

        /// <summary>
        /// Makes sure that the given value is in between a min and max value. 
        /// </summary>
        /// <returns> 
        /// val if min -le val -le max
        /// min if val -lt min
        /// max if val -gt max
        /// </returns>
        internal static int Clamp(int val, int min, int max)
        {
            if (val < min) return min;
            else if (val > max) return max;
            else return val;
        }

        internal static float Clamp(float val, float min, float max)
        {
            if (val < min) return min;
            else if (val > max) return max;
            else return val;
        }

        internal static bool IsWhitespace(string s)
        {
            string stripped = Util.RemoveSpace(s);
            return stripped.Length == 0;
        }

        internal static string Join<TObject>(List<TObject> strings, char delim)
        {
            string result = "";

            int end = strings.Count - 1;
            if (end < 0)
            {
                return "";
            }
            else if (end == 0)
            {
                return strings[0].ToString();
            }
            for (int i = 0; i < end; i += 1)
            {
                result += strings[i].ToString() + delim;
            }

            if (end > 0) result += strings[end].ToString();

            return result;
        }

        /// <summary>
        /// Removes all lines beginning with 'delim' (or whitespace followed 
        /// by 'delim'). Also removes all parts of a line after an occurance 
        /// of 'delim'.
        /// </summary>
        /// <param name="filepath">the file to be read</param>
        /// <param name="delim">the comment delimiting character</param>
        /// <param name="stripEmpty">empty lines are removed</param>
        internal static List<string> ReadAndStrip(string filepath, bool stripEmpty = false)
        {
            string data = File.ReadAllText(filepath);
            return StripComments(data, stripEmpty);
        }

        internal static List<string> StripComments(string data, bool stripEmpty)
        {
            List<string> lines = Regex.Split(data, @"\n|\r\n").ToList();
            char delim = '#';
            for (int i = lines.Count - 1; i >= 0; i -= 1)
            {
                string[] parts = lines[i].Split(delim);
                string trimmed = Trim(lines[i]);
                if ((stripEmpty && lines[i].Length == 0) ||
                    (trimmed.Length == 0 || trimmed[0] == delim || lines[i].Length == 0))
                {
                    lines.RemoveAt(i);
                }
                else
                {
                    lines[i] = parts[0];
                }
            }
            return lines;
        }

        /// <summary>
        /// Removes all the comments from a file and returns a stack ordered
        /// with the first line on top.
        /// </summary>
        /// <param name="filepath">path to the file to be read</param>
        /// <param name="delim">character beginning a comment</param>
        internal static Stack<string> StackifyFile(string filepath)
        {
            List<string> lines = Util.ReadAndStrip(filepath, true);
            return Util.Stackify(lines);
        }

        /// <summary>
        /// Removes whitespace from the beginning and end of a string.
        /// </summary>
        /// <param name="input">the string to be trimmed</param>
        /// <returns>the input with leading and trailing whitespace removed</returns>
        internal static string Trim(string input)
        {
            return input.Trim(Whitespace);
        }

        /// <summary>
        /// Removes *all* whitespace characters from the given string.
        /// </summary>
        /// <param name="input">the string to be stripped</param>
        /// <returns>input string with all whitespace removed</returns>
        internal static string RemoveSpace(string input)
        {
            foreach(char c in Whitespace)
            {
                input = input.Replace(c + "", ""); 
            }
            return input;
        }

        /// <summary>
        /// Reduces all spans of tabs/spaces to a single space, and trims
        /// leading and trailing whitespace of all kinds.
        /// </summary>
        internal static string FlattenSpace(string input)
        {
            input = Trim(input);
            if (input.Contains('@'))
            {
                Console.WriteLine("ok");
            }
            string result = Regex.Replace(input, "( |\t)+", " ");
            return result;
        }

        /// <summary>
        /// No idea why this isn't built in. Trys to convert a string 
        /// to a corresponding enum value. On failure throws ParseError.
        /// </summary>
        /// <typeparam name="TEnum">the type to be converted to</typeparam>
        /// <param name="name">the string to convert</param>
        internal static TEnum ParseEnum<TEnum>(string name) where TEnum : struct
        {
            TEnum result;
            bool success = Enum.TryParse<TEnum>(name, true, out result);
            if (!success) throw new ParseError("Failed to parse {0} name from \"{1}\"",typeof(TEnum), name);
            return result;
        }

        /// <summary>
        /// Gets a sphere encompassing all of the model's bounding spheres.
        /// </summary>
        /// <param name="meshes">myModel.Meshes</param>
        internal static BoundingSphere MergeSpheres(ModelMeshCollection meshes)
        {
            //could use "defaultifempty" here, but we should probably die instead
            if (meshes.Count == 0)
            {
                throw new InvalidDataException("A model must contain at least one mesh");
            }
            BoundingSphere result = meshes[0].BoundingSphere;
            for (int i = 1; i < meshes.Count; i += 1)
            {
                result = BoundingSphere.CreateMerged(result, meshes[i].BoundingSphere);
            }
            return result;
        }

        internal static Rectangle NearestMult(Rectangle bounds, int multiple)
        {
            int heightMult = (bounds.Height / multiple) + 1;
            int widthMult = (bounds.Width / multiple) + 1;

            return new Rectangle(bounds.Left, bounds.Top, widthMult * multiple, heightMult * multiple);
        }

        /// <summary>
        /// Converts the input string into a Vector2. Expected format is (X,Y)
        /// </summary>
        internal static Vector2 ParseVector2(string vec)
        {
            try
            {
                vec = vec.Replace("(", "").Replace(")", "").Replace(" ", "");
                string[] nums = vec.Split(',');
                float x = float.Parse(nums[0]);
                float y = float.Parse(nums[1]);
                return new Vector2(x, y);
            }
            catch
            {
                throw new ParseError("Unable to parse string {0} into a Vector2", vec);
            }

        }

        /// <summary>
        /// Converts the input string into a Vector3. Expected format is (X,Y,Z)
        /// </summary>
        public static Vector3 ParseVector3(string vec)
        {
            try
            {
                string cleaned = vec.Replace("(", "").Replace(")", "").Replace(" ", "");
                string[] nums = cleaned.Split(',');
                float x = float.Parse(nums[0]);
                float y = float.Parse(nums[1]);
                float z = float.Parse(nums[2]);
                return new Vector3(x, y, z);
            }
            catch
            {
                throw new ParseError("Unable to parse string {0} into a Vector3", vec);
            }

        }

        public static Float3 ParseFloat3(string vec)
        {
            vec = Util.RemoveSpace(vec);
            vec = vec.Replace("(", "").Replace(")", "");
            string[] ranges = vec.Split(',');
            FloatValue x = ParseFloatValue(ranges[0]);
            FloatValue y = ParseFloatValue(ranges[1]);
            FloatValue z = ParseFloatValue(ranges[2]);
            return new Float3(x, y, z);

        }

        /// <summary>
        /// Displaces the X and Y coordinates of the given Vector3 by the 
        /// given angle and distance.
        /// </summary>
        internal static Vector3 Displace(Vector3 origin, float angle, float distance)
        {
            float x = origin.X + distance * FastTrig.Sin(angle);
            float y = origin.Y + distance * FastTrig.Cos(angle);
            return new Vector3(x, y, origin.Z);
        }

        /// <summary>
        /// Returns the given rectangle grown by the given percentage on all sides.
        /// Assumes liveBuffer &gt; 0  
        /// </summary>
        internal static Rectangle GrowRect(Rectangle rect, float liveBuffer)
        {
            int newWidth = (int)(rect.Width * (1 + 2 * liveBuffer));
            int newHeight = (int)(rect.Height * (1 + 2 * liveBuffer));
            int newLeft = rect.Left - (int)(rect.Width * liveBuffer);
            int newTop = rect.Top - (int)(rect.Height * liveBuffer);

            return new Rectangle(newLeft, newTop, newWidth, newHeight);
            
        }

        /// <summary>
        /// Turns a list of strings into a stack of strings, with the 
        /// top of the file at the top of the stack.
        /// </summary>
        internal static Stack<T> Stackify<T>(IEnumerable<T> lines)
        {
            lines = lines.Reverse();
            return new Stack<T>(lines);
        }

        /// <summary>
        /// Splits a string by the given delim into a stack of
        /// strings with the first split string on top.
        /// </summary>
        internal static Stack<string> Tokenize(string line, char delim)
        {
            return Util.Stackify<string>(line.Split(delim));
        }

        internal static bool StrEq(string s0, string s1)
        {
            return Regex.IsMatch(s0, s1, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
        }

        internal static string Indent(string input, int size)
        {
            List<string> lines = Util.SplitLines(input, false);
            string result = "";
            foreach (string line in lines)
            {
                result += "".PadLeft(size) + line + '\n';
            }
            return result;
        }


        /// <summary>
        /// Converts a speed,angle into a Vector2
        /// </summary>
        internal static Vector2 Vec2FromSpeed(float speed, float angle)
        {
            return new Vector2(speed * FastTrig.Cos(angle), speed * FastTrig.Sin(angle));
        }


        internal static string[] GetFiles(string reldir)
        {
            string[] result;
            try
            {
                result = (string[])Directory.EnumerateFiles(reldir, "*", SearchOption.AllDirectories).ToArray();
                
            }
            catch
            {
                string curdir = Directory.GetCurrentDirectory();
                Util.Warn("Unable to find directory {0} in {1}", reldir, curdir); 
                throw new ExeggcuteError();
            }
            return result;
        }
        internal static Tuple<string, string> PreprocessRange(string s)
        {
            string flattened = Util.RemoveSpace(s).Replace("]", "").Replace("[", "") ;
            string[] split = flattened.Split('|');
            return new Tuple<string,string>(split[0],split[1]);
        }

        internal static bool IsInt(string s)
        {
            int dummy = 0;
            return int.TryParse(s, out dummy);
            
        }

        internal static bool IsFloat(string s)
        {
            float dummy = 0;
            return float.TryParse(s, out dummy);
        }

        internal static FloatValue ParseFloatValue(string s, float scale=1)
        {
            if (IsFloat(s))
            {
                float value = float.Parse(s);
                return new FloatValue(value * scale);
            }
            else
            {
                Tuple<string, string> prep = PreprocessRange(s);
                float min = float.Parse(prep.Item1) * scale;
                float max = float.Parse(prep.Item2) * scale;
                return new FloatRange(min, max);
            }
        }

        internal static Vector3 AngleToVector3(float debugAngle)
        {
            return new Vector3(FastTrig.Cos(debugAngle), FastTrig.Sin(debugAngle), 0);
        }

        internal static float AimAt(Vector3 origin, Vector3 target)
        {
            float y = target.Y - origin.Y;
            float x = target.X - origin.X;
            //HACK
            return FastTrig.Atan2(y, x + 0.0000000001f);
        }

        internal static string[] CleanEntry(string entry)
        {
            return RemoveSpace(entry).Split(':');
        }

        /// <summary>
        /// Converts
        /// <para>FieldName1:    value1 </para>
        /// <para>FIeldName2:    value2</para>
        /// To a list of string arrays whose entries are fieldname, value(string) pairs.
        /// </summary>
        internal static List<string[]> CleanData(string data)
        {
            List<string[]> result = new List<string[]>();

            List<string> lines = Util.StripComments(data, true);
            for (int i = 0; i < lines.Count; i += 1)
            {
                string cleaned = Util.RemoveSpace(lines[i]);
                result.Add(cleaned.Split(':'));
            }
            return result;
        }

        public static Model ParseModel(string name)
        {
            return Assets.Model[name];
        }

        public static Song ParseSong(string name)
        {
            return Assets.Song[name];
        }

        public static TEnum? ParseEnumNullable<TEnum>(string name) where TEnum : struct
        {
            TEnum? value = Util.ParseEnum<TEnum>(name);
            return value;
        }

        public static Texture2D ParseTexture2D(string name)
        {
            return Assets.Texture[name];
        }


        public static string ParseString(string s)
        {
            // that was easy
            return s;
        }

        public static SpriteFont ParseSpriteFont(string s)
        {
            return Assets.Font[s];
        }

        public static T GetRandomFrom<T>(List<T> items, Random rng)
        {
            return items[rng.NextInt(items.Count)];
        }

        public static void Debug(string message, params object[] args)
        {
            string formatted = string.Format(message, args);
            log.Add(formatted);
        }



        private static List<string> log = new List<string>();
        public static void WriteLog()
        {
            if (log == null || log.Count == 0) return;
            if (Engine.WRITE_LOG)
            {
                TimeSpan secs = DateTime.UtcNow - new DateTime(1970, 1, 1);
                string logName = string.Format("Exeggcute-debug-{0}.txt", secs.Seconds);
                string outName = string.Format("Exeggcute-debug-{0}.gz", secs.Seconds);
                File.WriteAllLines(logName, log);
                FileInfo info = new FileInfo(logName);

                using (FileStream inFile = info.OpenRead())
                    using (FileStream outFile = File.Create(outName))
                        using (GZipStream stream = new GZipStream(outFile, CompressionMode.Compress))
                            inFile.CopyTo(stream);
            }

        }

        internal static float Time()
        {
            return Environment.TickCount / 1000f;
        }


        internal static bool SphereContains(Vector3 center, float radius, Vector2 point)
        {
            radius = Math.Min(2, radius);
            if (PointDistance(center, point) < radius)
            {
                return true;
            }
            return false;
        }

        internal static float PointDistance(Vector3 position, Vector2 point)
        {
            float x = point.X;
            float y = point.Y;
            float z = 0;
            float dx = position.X - point.X;
            float dy = position.Y - point.Y;
            float dz = position.Z - z;
            return (float)Math.Sqrt(dx * dx + dy * dy);

        }

        internal static string[] SplitQuoteTokens(string input)
        {
            List<string> tokens = new List<string>();
            string current = "";
            bool inQuote = false;
            char prevChar = '\0';
            int backslashSeen = 0;
            int count = 0;
            foreach (char c in input)
            {
                if (c == '\'' && prevChar != '\'')
                {
                    backslashSeen = 1;
                }
                else if (c == '\'')
                {
                    backslashSeen += 1;
                }
                else
                {
                    backslashSeen = 0;
                }
                if (!inQuote)
                {
                    if (prevChar == '"')
                    {

                    }
                    else if (c == ' ' && prevChar != ' ')
                    {
                        tokens.Add(current);
                        current = "";
                    }
                    else if (c == '"' && prevChar != ' ' && count != 0)
                    {
                        throw new ParseError("If a string literal is not the first token, it must be preceeded by a space.");
                    }
                    else if (c == '"')
                    {
                        inQuote = true;
                        current = "";
                    }
                    else
                    {
                        current += c;
                    }
                }
                else
                {
                    if (c == '"' && backslashSeen % 2 == 0)
                    {
                        inQuote = false;
                        tokens.Add(current);
                        count += 1;
                        current = "";
                    }
                    else
                    {
                        current += c;
                    }

                }
                prevChar = c;
            }

            if (current.Length > 0 && !Util.IsWhitespace(current))
            {
                tokens.Add(current);
            }


            return tokens.ToArray();
        }

        internal static Vector2 GameToScreen(Vector2 vec)
        {
            //float xScale = 55.0f / (Engine.XRes / 2);
            //float yScale = -41.0f / (Engine.YRes / 2);
            //return new Vector2(xScale * (vec.X - Engine.XRes / 2),
            //                   yScale * (vec.Y - Engine.YRes / 2));

            float xScale =  (Engine.XRes / 2) / 55.0f;
            float yScale =  (Engine.YRes / 2) / -41.0f;
            return new Vector2(xScale * (vec.X) + Engine.XRes / 2,
                               yScale * (vec.Y) + Engine.YRes / 2);
        }

        internal static string JoinStack<T>(Stack<T> stack, string delim)
        {
            int iMax = stack.Count;
            string result = "";
            for (int i = 0; i < iMax; i += 1)
            {
                result += stack.Pop() + delim;
            }
            return result;
        }

        internal static void SetEffect(Model model, Effect effect)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = effect;
                }
            }
        }

        internal static string MakeFieldPair(string lhs, string rhs)
        {
            return string.Format("{0}:{1}", lhs, rhs);
        }
    }
}
