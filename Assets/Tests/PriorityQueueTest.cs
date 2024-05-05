using Assets.Scripts.Core;
using NUnit.Framework;

namespace Assets.Tests
{
    public class PriorityQueueTest
    {
        [Test]
        public void PushTest()
        {
            PriorityQueue<int> pQueue = new PriorityQueue<int>((a, b) => a < b);

            pQueue.Push(7);
            Assert.AreEqual(7, pQueue.Peek);
            pQueue.Push(9);
            Assert.AreEqual(7, pQueue.Peek);
            pQueue.Push(3);
            Assert.AreEqual(3, pQueue.Peek);
            pQueue.Push(1);
            Assert.AreEqual(1, pQueue.Peek);
            pQueue.Push(9);
            Assert.AreEqual(1, pQueue.Peek);
        }

        [Test]
        public void PopTest1()
        {
            PriorityQueue<int> pQueue = new PriorityQueue<int>((a, b) => a < b);

            pQueue.Push(7);
            Assert.AreEqual(7, pQueue.Pop());
            Assert.AreEqual(0, pQueue.Count);
            pQueue.Push(7);
            pQueue.Push(9);
            pQueue.Push(13);
            pQueue.Push(7);
            pQueue.Push(3);
            pQueue.Push(1);
            Assert.AreEqual(1, pQueue.Pop());
            Assert.AreEqual(3, pQueue.Pop());
            Assert.AreEqual(7, pQueue.Pop());
            Assert.AreEqual(3, pQueue.Count);
        }

        [Test]
        public void PopTest2()
        {
            PriorityQueue<int> pQueue = new PriorityQueue<int>((a, b) => a < b);

            pQueue.Push(1);
            pQueue.Push(2);
            pQueue.Push(3);
            Assert.AreEqual(1, pQueue.Pop());
            Assert.AreEqual(2, pQueue.Pop());
            Assert.AreEqual(3, pQueue.Pop());
        }
    }
}
