using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.console;

namespace Exeggcute.src
{
    class ContextStack
    {
        public bool ConsoleAttached { get; protected set; }
        protected DevConsole console;
        protected IContext[] stack;
        protected int size = 128;
        protected int ptr;


        public ContextStack()
        {
            stack = new IContext[size];
        }

        public void AttachConsole(DevConsole console)
        {
            this.console = console;
        }

        /// <summary>
        /// Gets the top of the stack.
        /// 
        /// Always succeeds, but may return null if the stack is empty.
        /// </summary>
        public IContext Top
        {
            get { return stack[ptr]; }
        }

        public IContext Front
        {
            get
            {
                if (ConsoleAttached)
                {
                    return console;
                }
                else
                {
                    return stack[ptr];
                }
            }
        }

        public IContext Pop()
        {
            IContext result = stack[ptr];
            stack[ptr] = null;
            if (ptr > 0) ptr -= 1;
            console.AttachParent(stack[ptr]);
            return result;
        }

        public void Push(IContext context)
        {
            ptr += 1;
            stack[ptr] = context;
            console.AttachParent(stack[ptr]);
        }

        public void Empty()
        {
            while (ptr > 0)
            {
                Pop().Unload();
            }
        }

        public int Count
        {
            get { return ptr + 1; }
        }

        public void ToggleConsole()
        {
            ConsoleAttached = !ConsoleAttached;
        }
    }
}
