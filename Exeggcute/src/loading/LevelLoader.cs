﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Media;
using Exeggcute.src.entities;
using Exeggcute.src.scripting.task;
using Exeggcute.src.graphics;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Exeggcute.src.scripting.roster;
using Exeggcute.src.scripting;
using Exeggcute.src.assets;
using Exeggcute.src.loading.specs;

namespace Exeggcute.src.loading
{

    class LevelLoader : Loader
    {
        TerrainLoader terrainLoader = new TerrainLoader();
        TaskListLoader taskLoader = new TaskListLoader();
        public Level Load(ContentManager content, GraphicsDevice graphics, Player player, string name)
        {

            LevelInfo levelInfo = null;
            WangMesh terrain = null;
            List<Task> taskList = new List<Task>();
            

            string filepath = string.Format("data/levels/{0}.level", name);
            string[] sections = File.ReadAllText(filepath).Split('@');
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
                    levelInfo = new LevelInfo(lines);
                }
                else if (matches("terrain"))
                {
                    lines.RemoveAt(0);
                    terrain = terrainLoader.Load(graphics, lines);
                }
                else if (matches("tasklist"))
                {
                    List<string> lineList = Util.StripComments(sections[k], true);
                    lineList.RemoveAt(0);
                    taskList = taskLoader.ParseLines(lineList);
                }
                else 
                {
                    throw new ParseError("Don't know what to do with heading \"{0}\"",currentField); 
                }
            }

            if (levelInfo == null ||
                taskList == null ||
                terrain == null)
            {
                throw new ParseError("Not all fields were initialized");
            }
            currentField = null;
            return new Level(graphics, 
                             content, 
                             player,
                             levelInfo.EnemyRoster,
                             levelInfo.LevelTheme,
                             levelInfo.BossTheme,
                             levelInfo.MiniBoss,
                             levelInfo.MainBoss,
                             taskList, 
                             terrain);
        }
    }
}
