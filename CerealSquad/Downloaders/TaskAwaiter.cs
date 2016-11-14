using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CerealSquad.Downloaders
{
    class TaskAwaiter
    {
        public enum TaskStatus
        {
            Empty,
            Running,
            Completed,
            Faulted
        }

        private List<Task> _Tasks = new List<Task>();

        System.Timers.Timer timer = new System.Timers.Timer(100);

        public TaskAwaiter()
        {
            Status = TaskStatus.Empty;
            timer.AutoReset = true;
            timer.Elapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Finished = _Tasks.Where(x => x.Status == System.Threading.Tasks.TaskStatus.RanToCompletion).Count();
            if (Finished == Total)
            {
                timer.Stop();
                Status = TaskStatus.Completed;
            }
            if (_Tasks.Where(x => x.IsFaulted).Count() > 0)
                Status = TaskStatus.Faulted;
        }

        public void Reset()
        {
            Status = TaskStatus.Empty;
            _Tasks.Clear();
            Finished = 0;
            if (timer.Enabled)
                timer.Stop();
        }

        public void Add(Task task)
        {
            _Tasks.Add(task);
            if (!timer.Enabled)
            {
                timer.Start();
                Status = TaskStatus.Running;
            }
        }

        public Exception Exception { get { return _Tasks.FirstOrDefault(x => x.IsFaulted).Exception; } }

        public TaskStatus Status { get; private set; }
        public int Finished { get; private set; }
        public int Total { get { return _Tasks.Count; } }
    }
}
