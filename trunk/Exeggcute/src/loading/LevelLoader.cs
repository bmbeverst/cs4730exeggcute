using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

    class LevelLoader
    {
        public Level LoadByFile(ContentManager content, GraphicsDevice graphics, HUD hud, Difficulty difficulty, string filename)
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
                Matcher match = new Matcher(header[0]);
                //get rid of the header
                lines.RemoveAt(0);
                if (match["info"])
                {
                    levelInfo = new LevelInfo(filename, lines);
                }
                else if (match["terrain"])
                {
                    
                    TerrainInfo info = new TerrainInfo(filename, lines);
                    terrain = info.MakeMesh(graphics);
                }
                else if (match["tasklist"])
                {
                    List<string> lineList = Util.StripComments(sections[k], true);
                    lineList.RemoveAt(0);
                    taskList = Loaders.TaskList.ParseLines(filename, lineList);
                }
                else if (match["lights"])
                {
                    lightSettings = new LightSettings(filename, lines);
                }
                else
                {
                    throw new ParseError("Don't know what to do with heading \"{0}\"", header[0]);
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
            return new Level(graphics,
                             content,
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

        public Level LoadByName(ContentManager content, GraphicsDevice graphics, HUD hud, Difficulty difficulty, string name)
        {
            string filename = string.Format("data/levels/{0}.level", name);
            return LoadByFile(content, graphics, hud, difficulty, filename);
        }
    }
}
