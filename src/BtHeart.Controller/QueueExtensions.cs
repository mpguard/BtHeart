using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtHeart.Controller
{
    internal static class QueueExtensions
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

        public static void RemoveRange<T>(this Queue<T> queue, int count)
        {
            int queueCount = queue.Count;
            int minCount = Math.Min(queueCount, count);

            for (int i = 0; i < minCount; i++)
            {
               queue.Dequeue();
            }
        }

        public static double Median(this IEnumerable<double> queue)
        {
            List<double> sortList = new List<double>(queue);
            sortList.Sort();
            int mid = sortList.Count / 2;
            if (sortList.Count % 2 == 0) // 偶数
            {
                return (sortList[mid] + sortList[mid + 1]) / 2;
            }
            else // 奇数
            {
                return sortList[mid + 1];
            }
        }

        public static void EnqueueEx<T>(this Queue<T> queue,T item)
        {
            while (queue.Count >= 8)
                queue.Dequeue();
            queue.Enqueue(item);
        }

        public static void RefreshRR(this Queue<int> queue,int count)
        {
            for (int i = 0; i < queue.Count; i++)
            {
                int r = queue.Dequeue();
                queue.EnqueueEx(r - count);
            }
        }

        public static int RRInterval(this IEnumerable<int> queue)
        {
            int sum = 0;
            int count = queue.Count();
            for (int i = 1; i < queue.Count();i++)
            {
                sum += queue.ElementAt(i) - queue.ElementAt(i - 1);
            }
            if (count == 0)
                return 0;
            return sum / (count-1);
        }
    }
}
