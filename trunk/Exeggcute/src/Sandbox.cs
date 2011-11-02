using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.console.commands;
using Exeggcute.src.entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Exeggcute.src.scripting.task;
using Exeggcute.src.graphics;
using Microsoft.Xna.Framework.Media;


namespace Exeggcute.src
{
    class Sandbox : ConsoleContext
    {

        protected WangMesh terrain;

        protected List<Task> taskList;
        protected int taskPtr;

        public EntityManager collider;

        public Rectangle GameArea;
        public Rectangle LiveArea;

        protected float liveBuffer = 1f / 4f;

        protected Player player;

        public Sandbox(WangMesh terrain)
        {
            this.collider = new EntityManager();
            this.GameArea = new Rectangle(-Level.HalfWidth, -Level.HalfHeight, Level.HalfWidth * 2, Level.HalfHeight * 2);
            this.LiveArea = Util.GrowRect(GameArea, liveBuffer);
            this.terrain = terrain;
        }

        public void AttachPlayer(Player player)
        {
            this.player = player;
        }

        public override void AcceptCommand(ConsoleCommand command)
        {

        }

        public override void AcceptCommand(GoCommand context)
        {
            //if (context.
        }
        public virtual void Process(Task task)
        {
            throw new InvalidOperationException("Must call a subclass overload");
        }

        
        public virtual void Process(SongFadeTask task)
        {
            

        }

        public virtual void Process(BarrierTask barrier)
        {
            if (Worlds.World.CanPassBarrier(barrier))
            {
                taskPtr += 1;
            }
        }

        public virtual void Process(SpawnTask task)
        {
            
        }

        public virtual void Process(ClearTask task)
        {
            Worlds.World.ClearLists();
        }

        protected int waitCounter = 0;
        public virtual void Process(WaitTask task)
        {
            if (waitCounter >= task.Duration)
            {
                waitCounter = 0;
                taskPtr += 1;
            }
            waitCounter += 1;
        }

        public virtual void Process(KillAllTask kill)
        {
            Worlds.World.KillEnemies();
            taskPtr += 1;
        }

        public virtual void Process(BossTask bossTask)
        {

        }

        public virtual void ProcessTasks()
        {
            if (taskPtr >= taskList.Count) return;
            Task current = taskList[taskPtr];
            current.Process(this);
        }

        public virtual void AcceptTaskList(List<Task> tasks)
        {
            foreach (Task task in tasks)
            {
                task.Process(this);
            }
        }
        
        public override void Update(ControlManager controls)
        {
            
            if (player != null)
            {
                player.Update(controls, true);
            }
            collider.UpdateAll(LiveArea);

            MediaPlayer.GetVisualizationData(soundData);
            terrain.Update(soundData.Frequencies);
        }


        public override void Draw3D(GraphicsDevice graphics, Camera camera)
        {
            Matrix view = camera.GetView();
            Matrix projection = camera.GetProjection();
            if (player != null)
            {
                player.Draw3D(graphics, view, projection);
            }

            collider.DrawAll3D(graphics, view, projection);
            terrain.Draw(graphics, view, projection);
        }


        public override void Draw2D(SpriteBatch batch)
        {
            if (player != null) player.Draw2D(batch);
        }

        public override void Unload()
        {

        }


        public override void Dispose()
        {

        }

        public static bool IsName(string name)
        {
            return Util.StrEq("sandbox", name) || Util.StrEq("sb", name);
        }
    }
}
