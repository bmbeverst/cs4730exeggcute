using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Exeggcute.src.entities;
using Exeggcute.src.scripting;

namespace Exeggcute.src
{
    /// <summary>
    /// Miscellaneous utility functions
    /// </summary>
    static class Util
    {
        /// <summary>
        /// Contains the characters considered to be whitespace.
        /// </summary>
        public static readonly char[] Whitespace = new char[] {
            ' ', '\t', '\r', '\n'
        };

        /// <summary>
        /// Returns the lines from a file, taking failure to be non-recoverable.
        /// </summary>
        public static List<string> ReadLines(string filepath)
        {
            if (!File.Exists(filepath))
            {
                Die("File \"{0}\" does not exist, or could not be found in {1}", filepath, Directory.GetCurrentDirectory());
            }
            return File.ReadLines(filepath).ToList();
        }

        /// <summary>
        /// Used to issue warnings to the terminal.
        /// </summary>
        public static void Warn(string message, params object[] args)
        {
            string formatted = String.Format(message, args);
            Console.Error.WriteLine("WARNING: {0}", formatted);
        }

        /// <summary>
        /// Used to quit with an error message.
        /// </summary>
        public static void Die(string message, params object[] args)
        {
            string msg;
            if (args.Length == 0)
            {
                msg = message;
            }
            else
            {
                msg = string.Format(message, args);
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
        public static T Die<T>(string message, params object[] args)
        {
            Die(message, args);
            return default(T);
        }

        /// <summary>
        /// Writes the given string to the given file. If the file exists, 
        /// it will move that file to a temporary location before writing
        /// to ensure that data is never destroyed.
        /// </summary>
        public static void WriteFile(string filepath, string data)
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

        /// <summary>
        /// Splits a string based on newline characters and trims all 
        /// leading and trailing whitespace.
        /// </summary>
        public static List<string> SplitLines(string allText)
        {
            string[] splitted = Regex.Split(allText, "(\n|\r\n)");
            for (int i = 0; i < splitted.Length; i += 1)
            {
                string trimmed = splitted[i].Trim(Whitespace);
                splitted[i] = trimmed;
            }
            return splitted.ToList();
        }

        /// <summary>
        /// Returns a list of all buttons currently being pressed by player 1.
        /// Similar to keyboardState.GetPressedKeys, but the function does
        /// not exist for GamePadState for whatever reason.
        /// </summary>
        public static Buttons[] GetPressedButtons(GamePadState gpState)
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
        public static bool PowerOfTwo(int n)
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
        public static int Clamp(int val, int min, int max)
        {
            if (val < min) return min;
            else if (val > max) return max;
            else return val;
        }

        /// <summary>
        /// Removes all lines beginning with 'delim' (or whitespace followed 
        /// by 'delim'). Also removes all parts of a line after an occurance 
        /// of 'delim'.
        /// </summary>
        /// <param name="filepath">the file to be read</param>
        /// <param name="delim">the comment delimiting character</param>
        /// <param name="stripEmpty">empty lines are removed</param>
        public static List<string> StripComments(string filepath, char delim, bool stripEmpty = false)
        {
            List<string> lines = ReadLines(filepath);
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
        public static Stack<string> StackifyFile(string filepath, char delim)
        {
            List<string> lines = Util.StripComments(filepath, delim, true);
            return Util.Stackify(lines);
        }

        /// <summary>
        /// Removes whitespace from the beginning and end of a string.
        /// </summary>
        /// <param name="input">the string to be trimmed</param>
        /// <returns>the input with leading and trailing whitespace removed</returns>
        public static string Trim(string input)
        {
            return input.Trim(Whitespace);
        }

        /// <summary>
        /// Removes *all* whitespace characters from the given string.
        /// </summary>
        /// <param name="input">the string to be stripped</param>
        /// <returns>input string with all whitespace removed</returns>
        public static string RemoveSpace(string input)
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
        public static string FlattenSpace(string input)
        {
            input = Trim(input);
            return Regex.Replace(input, "( |\t)+", " ");
        }

        /// <summary>
        /// No idea why this isn't built in. Trys to convert a string 
        /// to a corresponding enum value. On failure throws ParseError.
        /// </summary>
        /// <typeparam name="TEnum">the type to be converted to</typeparam>
        /// <param name="name">the string to convert</param>
        public static TEnum ParseEnum<TEnum>(string name) where TEnum : struct
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
        public static BoundingSphere MergeSpheres(ModelMeshCollection meshes)
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

        /// <summary>
        /// Converts the input string into a Vector2. Expected format is (X,Y)
        /// </summary>
        public static Vector2 ParseVector2(string vec)
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
                vec = vec.Replace("(", "").Replace(")", "").Replace(" ", "");
                string[] nums = vec.Split(',');
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

        /// <summary>
        /// Displaces the X and Y coordinates of the given Vector3 by the 
        /// given angle and distance.
        /// </summary>
        public static Vector3 Displace(Vector3 origin, float angle, float distance)
        {
            float x = origin.X + distance * FastTrig.Sin(angle);
            float y = origin.Y + distance * FastTrig.Cos(angle);
            return new Vector3(x, y, origin.Z);
        }

        /// <summary>
        /// Returns the given rectangle grown by the given percentage on all sides.
        /// Assumes liveBuffer &gt; 0  
        /// </summary>
        public static Rectangle GrowRect(Rectangle rect, float liveBuffer)
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
        public static Stack<T> Stackify<T>(IEnumerable<T> lines)
        {
            lines = lines.Reverse();
            return new Stack<T>(lines);
        }

        public static Stack<string> Tokenize(string line, char delim)
        {
            return Util.Stackify<string>(line.Split(delim));
        }
    }
}
