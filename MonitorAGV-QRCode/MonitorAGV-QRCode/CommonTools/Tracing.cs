using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CommonTools
{
    public class Tracing
    {
        public static int TracePaint = 1;

        private static Tracing m_instance = new Tracing();

        private Stack<int> m_ticks = new Stack<int>();

        private Queue<StringBuilder> m_strings = new Queue<StringBuilder>();

        private Dictionary<int, bool> m_ids = new Dictionary<int, bool>();

        private Thread m_thread;

        private ManualResetEvent m_wait = new ManualResetEvent(false);

        public static void StartTrack(int id)
        {
            bool flag = !Tracing.m_instance.CanTrace(id);
            if (!flag)
            {
                Tracing.m_instance.PushTick();
            }
        }

        public static void EndTrack(int id, string text)
        {
            bool flag = !Tracing.m_instance.CanTrace(id);
            if (!flag)
            {
                Tracing.m_instance.PopTick(text, null);
            }
        }

        public static void EndTrack(int id, string text, params object[] args)
        {
            bool flag = !Tracing.m_instance.CanTrace(id);
            if (!flag)
            {
                Tracing.m_instance.PopTick(text, args);
            }
        }

        public static void AddId(int id)
        {
            Tracing.m_instance.m_ids[id] = true;
        }

        public static void WriteLine(int id, string text, params object[] args)
        {
            bool flag = !Tracing.m_instance.CanTrace(id);
            if (!flag)
            {
                Tracing.m_instance.PopTick(text, args);
            }
        }

        public static void EnableTrace()
        {
            Tracing.m_instance.m_thread = new Thread(new ThreadStart(Tracing.m_instance.DoTrace));
            Tracing.m_instance.m_thread.Name = "Tracing";
            Tracing.m_instance.m_thread.Priority = ThreadPriority.Normal;
            Tracing.m_instance.m_thread.Start();
        }

        public static void Terminate()
        {
            bool flag = Tracing.m_instance.m_thread != null;
            if (flag)
            {
                Tracing.m_instance.m_thread.Abort();
            }
            Tracing.m_instance.m_thread = null;
        }

        private Tracing()
        {
        }

        private bool CanTrace(int id)
        {
            return this.m_thread != null && this.m_ids.ContainsKey(id);
        }

        private void PushTick()
        {
            this.m_ticks.Push(Environment.TickCount);
        }

        private void PopTick(string text, params object[] args)
        {
            StringBuilder stringBuilder = new StringBuilder();
            int num = Environment.TickCount - this.m_ticks.Pop();
            bool flag = args == null;
            if (flag)
            {
                stringBuilder.AppendFormat("{0}: {1}, Ticks({2})", DateTime.Now.ToLongTimeString(), text, num.ToString());
            }
            else
            {
                stringBuilder.AppendFormat("{0}: ", DateTime.Now.ToLongTimeString());
                stringBuilder.AppendFormat(text, args);
                stringBuilder.AppendFormat(", Ticks({0})", num.ToString());
            }
            stringBuilder.AppendLine();
            Queue<StringBuilder> strings = this.m_strings;
            lock (strings)
            {
                this.m_strings.Enqueue(stringBuilder);
                this.m_wait.Set();
            }
        }

        private void WriteLine(string text, params object[] args)
        {
            StringBuilder stringBuilder = new StringBuilder();
            bool flag = args == null;
            if (flag)
            {
                stringBuilder.AppendFormat("{0}: {1}", DateTime.Now.ToLongTimeString(), text);
            }
            else
            {
                stringBuilder.AppendFormat("{0}: ", DateTime.Now.ToLongTimeString());
                stringBuilder.AppendFormat(text, args);
            }
            stringBuilder.AppendLine();
            Queue<StringBuilder> strings = this.m_strings;
            lock (strings)
            {
                this.m_strings.Enqueue(stringBuilder);
                this.m_wait.Set();
            }
        }

        private void DoTrace()
        {
            while (true)
            {
                this.m_wait.WaitOne();
                while (this.m_strings.Count > 0)
                {
                    StringBuilder stringBuilder = null;
                    Queue<StringBuilder> strings = this.m_strings;
                    lock (strings)
                    {
                        stringBuilder = this.m_strings.Dequeue();
                    }
                    Console.Write(stringBuilder.ToString());
                }
                this.m_wait.Reset();
            }
        }
    }
}
