using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Core
{
    public class PriorityQueue<T>
    {
        private readonly List<T> tree = new List<T>();

        private readonly Func<T, T, bool> less;

        public int Count
        {
            get => tree.Count;
        }

        public T Peek
        {
            get => tree[0];
        }

        public PriorityQueue(Func<T, T, bool> less)
        {
            this.less = less;
        }

        public void Push(T item)
        {
            tree.Add(item);
            Up();
        }

        public T Pop()
        {
            T popped = tree[0];
            tree[0] = tree[Count - 1];
            tree.RemoveAt(Count - 1);
            Down();

            return popped;
        }

        private void Up()
        {
            int i = tree.Count - 1;

            while (i != 0 && less(tree[i], tree[GetParent(i)]))
            {
                Swap(i, GetParent(i));
                i = GetParent(i);
            }
        }

        private void Down()
        {
            int i = 0;

            while (true)
            {
                int li = GetLeft(i);
                int ri = GetRight(i);

                if (ri >= Count)
                {
                    if (li >= Count || less(tree[i], tree[li]))
                    {
                        return;
                    }

                    Swap(i, li);
                    i = li;

                    continue;
                }

                if (less(tree[li], tree[ri]))
                {
                    if (less(tree[li], tree[i]))
                    {
                        Swap(i, li);
                        i = li;

                        continue;
                    }

                    return;
                }

                if (less(tree[ri], tree[i]))
                {
                    Swap(i, ri);
                    i = ri;

                    continue;
                }

                return;
            }
        }

        private int GetParent(int i)
        {
            return (i - 1) / 2;
        }

        private int GetLeft(int i)
        {
            return 2 * i + 1;
        }

        private int GetRight(int i)
        {
            return 2 * i + 2;
        }

        private void Swap(int i, int j)
        {
            T temp = tree[i];
            tree[i] = tree[j];
            tree[j] = temp;
        }
    }
}
