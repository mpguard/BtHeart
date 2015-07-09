using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtHeart.Controller
{
    internal static class ConcurrentQueueExtensions
    {
        public static void Clear<T>(this ConcurrentQueue<T> queue)
        {
            T item;
            while (queue.TryDequeue(out item))
            {
                // do nothing
            }
        }

        public static void RemoveRange<T>(this ConcurrentQueue<T> queue,int count)
        {
            int queueCount = queue.Count;
            int minCount = Math.Min(queueCount, count);

            for (int i = 0; i < minCount; i++)
            {
                T item;
                queue.TryDequeue(out item);
            }
        }
    }
}
