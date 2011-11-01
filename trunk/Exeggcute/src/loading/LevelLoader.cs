using System.Collections.Generic;
using System.IO;
using Exeggcute.src.entities;
using Exeggcute.src.graphics;
using Exeggcute.src.gui;
using Exeggcute.src.loading.specs;
using Exeggcute.src.scripting;
using Exeggcute.src.scripting.task;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Exeggcute.src.loading
{

    class LevelLoader : Loader
    {
        public Level LoadByFile(ContentManager content, GraphicsDevice graphics, Player player, HUD hud, Difficulty difficulty, string filename)
        {
            string name = Path.GetFileNameWithoutExtension(filename);
            LevelInfo levelInfo = null;
            WangMesh terrain = null;
            List<Task> taskList = new List<Task>();
            LightSettings lightSettings = null;


            string[] sections = File.ReadAllText(filename).Split('@');
            for (int k = 0; k < sections.Length; k += 1)
            {
                if (sections[k].Length == 0) continue;
                List<string[]> lines = Util.CleanData(sections[k]);
                string[] header = lines[0];
                currentField = header[0];
                //get rid of the header

                if (matches("info"))
                {
                    lines.RemoveAt(0);
                    levelInfo = new LevelInfo(filename, lines);
                }
                else if (matches("terrain"))
                {
                    lines.RemoveAt(0);
                    TerrainInfo info = new TerrainInfo(filename, lines);
                    terrain = info.MakeMesh(graphics);
                }
                else if (matches("tasklist"))
                {
                    List<string> lineList = Util.StripComments(sections[k], true);
                    lineList.RemoveAt(0);
                    taskList = Loaders.TaskList.ParseLines(filename, lineList);
                }
                else if (matches("lights"))
                {
                    List<string> lineList = Util.StripComments(sections[k], true);
                    lineList.RemoveAt(0);
                    lightSettings = new LightSettings(lineList);
                }
                else
                {
                    throw new ParseError("Don't know what to do with heading \"{0}\"", currentField);
                }
            }

            //fixme i solved this problem!
            if (levelInfo == null ||
                taskList == null ||
                terrain == null ||
                lightSettings == null)
            {
                throw new ParseError("Not all fields were initialized");
            }
            currentField = null;
            return new Level(graphics,
                             content,
                             player,
                             hud,
                             difficulty,
                             false,//FIXME
                             name,
                             levelInfo.Roster,
                             levelInfo.LevelTheme,
                             levelInfo.BossTheme,
                             levelInfo.MiniBoss,
                             levelInfo.MainBoss,
                             taskList,
                             terrain,
                             lightSettings);
        }
        public Level LoadByName(ContentManager content, GraphicsDevice graphics, Player player, HUD hud, Difficulty difficulty, string name)
        {
            string filename = string.Format("data/levels/{0}.level", name);
            return LoadByFile(content, graphics, player, hud, difficulty, filename);
        }
    }
}
