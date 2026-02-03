using Notification.Api.Enums;

namespace Notification.Api.Queues
{
    /// <summary>
    /// Purpose: Stores notifications sent by Producers and ensures priority-based receipt by Consumers
    /// Mechanism:
    ///     - lock(_lock) → provides thread-safe access to the queue
    ///     - Monitor.Wait → consumer waits if the queue is empty
    ///     - Monitor.Pulse → wakes up consumers when the producer adds a new notification
    ///     - SortedDictionary → maintains priority order
    ///     - Reverse → Enum defaults to Low = 0 → Critical=3, with Reverse checking Critical first
    /// </summary>
    public class NotificationQueue
    {
        private readonly SortedDictionary<Priority, Queue<Models.Notification>> _queues = new()
        {
            {Priority.Critical,new Queue<Models.Notification>() },
            {Priority.High,new Queue<Models.Notification>() },
            {Priority.Normal,new Queue<Models.Notification>() },
            {Priority.Low,new Queue<Models.Notification>() },
        };

        private readonly object _lock = new object();
        private bool isShutDown = false;



        /// <summary>
        /// Producer side: Adds Notification
        /// </summary>
        /// <param name="notification"></param>
        public void Enqueue(Models.Notification notification)
        {
            lock (_lock)
            {
                _queues[notification.Priority].Enqueue(notification);

                // Notify consumer threads
                Monitor.Pulse(_lock);
            }
        }


        /// <summary>
        /// Consumer side: Receives priority-based notifications
        /// </summary>
        /// <returns></returns>
        public Models.Notification Dequeue()
        {
            lock (_lock)
            {

                // Wait if the queue is empty
                while (isEmpty() && !isShutDown)
                {
                    Monitor.Wait(_lock);
                }

                if (isShutDown) return null;


                // Issue notification by priority
                foreach (var priority in Enum.GetValues<Priority>().Reverse())
                {
                    if (_queues[priority].Count > 0)
                    {
                        var notification = _queues[priority].Dequeue();
                        notification.Status = Status.Processing;
                        return notification;
                    }
                }

                return null;
            }
        }


        /// <summary>
        /// Force all consumers to wake up when the system stops
        /// </summary>
        public void Shutdown()
        {
            lock (_lock)
            {
                isShutDown = true;
                Monitor.PulseAll(_lock);
            }
        }


        private bool isEmpty()
        {
            return _queues.Values.All(x => x.Count == 0);
        }

    }
}
