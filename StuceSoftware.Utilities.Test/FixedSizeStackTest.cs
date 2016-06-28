using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace StuceSoftware.Utilities.Test
{
    [TestClass]
    public class FixedSizeStackTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestConstructorMaxSizeCannotBeZero()
        {
            var stack = new FixedSizeStack<int>(0);
            Assert.Fail("Expected exception to be thrown.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestConstructorMaxSizeCannotBeLessThanZero()
        {
            var stack = new FixedSizeStack<int>(-1);
            Assert.Fail("Expected exception to be thrown.");
        }

        [TestMethod]
        public void TestConstructorKeepsFirstElementsFromIEnumerable()
        {
            var stack = new FixedSizeStack<int>(5, Enumerable.Range(0, 10));
            Assert.IsTrue(stack.SequenceEqual(Enumerable.Range(0, 5)));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestMaxSizeCannotSetZero()
        {
            var stack = new FixedSizeStack<int>(10);
            stack.MaxSize = 0;
            Assert.Fail("Expected exception to be thrown.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestMaxSizeCannotSetLessThanZero()
        {
            var stack = new FixedSizeStack<int>(10);
            stack.MaxSize = -1;
            Assert.Fail("Expected exception to be thrown.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestCannotPeekEmptyStack()
        {
            var stack = new FixedSizeStack<int>(10);
            stack.Peek();
            Assert.Fail("Expected exception to be thrown.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestCannotPopEmptyStack()
        {
            var stack = new FixedSizeStack<int>(10);
            stack.Pop();
            Assert.Fail("Expected exception to be thrown.");
        }

        [TestMethod]
        public void TestPushSuccessful()
        {
            const int ValueToPush = 42;
            var stack = new FixedSizeStack<int>(10);
            Assert.IsTrue(stack.Count == 0);

            stack.Push(ValueToPush);

            Assert.IsTrue(stack.Count == 1);
            Assert.AreEqual(ValueToPush, stack.Peek());
        }

        [TestMethod]
        public void TestPeekSuccessful()
        {
            const int ValueToPush = 42;
            var stack = new FixedSizeStack<int>(10);
            Assert.IsTrue(stack.Count == 0);

            stack.Push(ValueToPush);
            Assert.IsTrue(stack.Count == 1);

            Assert.AreEqual(ValueToPush, stack.Peek());
            Assert.IsTrue(stack.Count == 1);
        }

        [TestMethod]
        public void TestPopSuccessful()
        {
            const int ValueToPush = 42;
            var stack = new FixedSizeStack<int>(10);
            Assert.IsTrue(stack.Count == 0);

            stack.Push(ValueToPush);
            Assert.IsTrue(stack.Count == 1);

            Assert.AreEqual(ValueToPush, stack.Pop());
            Assert.IsTrue(stack.Count == 0);
        }

        [TestMethod]
        public void TestPushBeyondMaxSizeWillNotIncreaseSize()
        {
            var stack = new FixedSizeStack<int>(10);
            for(int i=0; i < 100; ++i)
            {
                stack.Push(i);
            }
            Assert.IsTrue(stack.Count == 10);
        }

        [TestMethod]
        public void TestPushBeyondMaxSizeDumpsOlderElements()
        {
            var stack = new FixedSizeStack<int>(10);
            for(int i=0; i < 11; ++i)
            {
                stack.Push(i);
            }
            Assert.IsFalse(stack.Contains(0));
            Assert.IsTrue(stack.Contains(10));
        }

        [TestMethod]
        public void TestSetMaxSizeSmallerDumpsElements()
        {
            var stack = new FixedSizeStack<int>(10);
            for(int i=0; i < 10; ++i)
            {
                stack.Push(i);
            }
            Assert.IsTrue(stack.Count == 10);

            stack.MaxSize = 5;
            Assert.IsTrue(stack.Count == 5);
            for(int i=0; i < 5; ++i)
            {
                Assert.IsFalse(stack.Contains(i));
            }
        }

        [TestMethod]
        public void TestSetMaxSizeWillNotDumpWhenLessElements()
        {
            var stack = new FixedSizeStack<int>(10, Enumerable.Range(0, 5));
            Assert.IsTrue(stack.Count == 5);

            stack.MaxSize = 6;
            Assert.IsTrue(stack.Count == 5);
        }

        [TestMethod]
        public void TestToArray()
        {
            var array = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var stack = new FixedSizeStack<int>(array.Length, array);

            var test = stack.ToArray();
            Assert.IsTrue(array.SequenceEqual(test));
        }

        [TestMethod]
        public void TestClearEmptiesStack()
        {
            var stack = new FixedSizeStack<int>(50, Enumerable.Range(0, 50));
            Assert.IsTrue(stack.Count == 50);

            stack.Clear();
            Assert.IsTrue(stack.Count == 0);
        }
    }
}
