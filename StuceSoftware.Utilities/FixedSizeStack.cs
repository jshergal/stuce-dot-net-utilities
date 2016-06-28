//
// Copyright 2016 - Jeff Shergalis
//
// Licensed under the MIT License - http://www.opensource.org/licenses/mit-license.php
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
// associated documentation files (the "Software"), to deal in the Software without restriction,
// including without limitation the rights to use, copy, modify, merge, publish, distribute,
// sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE
// AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES
// OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT
// OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace StuceSoftware.Utilities
{
    /// <summary>
    /// Implementation of a standard Stack collection but this stack has a limit on its size. When the max size is reached, any new
    /// element being added will cause the oldest element to be discarded. This collection is not thread safe.
    /// </summary>
    [Serializable, ComVisible(false)]
    public sealed class FixedSizeStack<T> : IEnumerable<T>, ICollection<T>, IEnumerable, ICollection, IReadOnlyCollection<T>
    {
        private const string EmptyStackErrorMessage = @"The FixedSizeStack is empty.";

        private readonly LinkedList<T> _list;

        private int _maxSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="FixedSizeStack{T}"/> class.
        /// </summary>
        /// <param name="maxSize">The maximum size of the stack.</param>
        /// <param name="collection">A collection to initialize the stack from.
        /// If collection.Count > maxSize will only take the first maxSize elements.</param>
        /// <exception cref="ArgumentOutOfRangeException">maxSize is &lt;=0</exception>
        public FixedSizeStack(int maxSize, IEnumerable<T> collection = null)
        {
            if (maxSize <= 0) throw new ArgumentOutOfRangeException("maxSize", maxSize, "Maximum size must be > 0.");

            _maxSize = maxSize;
            _list = collection != null ? new LinkedList<T>(collection.Take(maxSize)) : new LinkedList<T>();
        }

        /// <summary>
        /// Gets the number of elements contained in the FizedSizeStack.
        /// </summary>
        public int Count
        {
            get
            {
                return _list.Count;
            }
        }

        /// <summary>
        /// The maximum size of the stack. When this size is reached, any new element pushed on the stack causes the oldest element to be discarded.
        /// <para>Note: Setting a new MaxSize that is less than the current MaxSize will cause the stack to shrink if the current size is
        /// greater than the new MaxSize.</para>
        /// </summary>
        public int MaxSize
        {
            get
            {
                return _maxSize;
            }
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException("maxSize", value, "Maximum size must be > 0.");

                _maxSize = value;
                while (_list.Count > _maxSize)
                {
                    _list.RemoveLast();
                }
            }
        }
        /// <summary>
        /// Removes all objects from the FixedSizeStack.
        /// </summary>
        public void Clear()
        {
            _list.Clear();
        }

        /// <summary>
        /// Determines whether an element is in the FixedSizeStack.
        /// </summary>
        public bool Contains(T element)
        {
            return _list.Contains(element);
        }

        /// <summary>
        /// Copies the FixedSizeStack to an existing one dimensional array starting at the specified array index.
        /// </summary>
        public void CopyTo(T[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Returns the object at the top of the FixedSizeStack without removing it.
        /// </summary>
        /// <exception cref="InvalidOperationException">The FixedSizeStack is empty.</exception>
        public T Peek()
        {
            if (_list.Count > 0) return _list.First.Value;

            throw new InvalidOperationException(EmptyStackErrorMessage);
        }

        /// <summary>
        /// Returns and removes the object from the top of the FixedSizeStack.
        /// </summary>
        /// <exception cref="InvalidOperationException">The FixedSizeStack is empty.</exception>
        public T Pop()
        {
            if (_list.Count > 0)
            {
                var value = _list.First.Value;
                _list.RemoveFirst();
                return value;
            }

            throw new InvalidOperationException(EmptyStackErrorMessage);
        }

        /// <summary>
        /// Inserts an object at the top of the FixedSizeStack. If the stack has reached max capacity, the oldest item in the stack
        /// will be discarded.
        /// </summary>
        public void Push(T item)
        {
            if (_list.Count == _maxSize)
            {
                _list.RemoveLast();
            }
            _list.AddFirst(item);
        }

        /// <summary>
        /// Copies the FixedSizeStack to a new array.
        /// </summary>
        /// <remarks>The elements are copied onto the array in last-in-first-out (LIFO) order,
        /// similar to the order of the elements returned by a succession of calls to Pop.</remarks>
        public T[] ToArray()
        {
            var array = new T[_list.Count];
            _list.CopyTo(array, 0);
            return array;
        }

        #region ICollection/ICollection<T>

        void ICollection.CopyTo(Array array, int index)
        {
            ((ICollection)_list).CopyTo(array, index);
        }

        void ICollection<T>.Add(T item)
        {
            ((ICollection<T>)_list).Add(item);
        }

        bool ICollection<T>.Remove(T item)
        {
            return _list.Remove(item);
        }

        bool ICollection.IsSynchronized
        {
            get
            {
                return ((ICollection)_list).IsSynchronized;
            }
        }

        object ICollection.SyncRoot
        {
            get
            {
                return ((ICollection)_list).SyncRoot;
            }
        }

        bool ICollection<T>.IsReadOnly
        {
            get
            {
                return ((ICollection<T>)_list).IsReadOnly;
            }
        }

        #endregion

        #region IEnumerable/IEnumerable<T>

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_list).GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        #endregion
    }
}