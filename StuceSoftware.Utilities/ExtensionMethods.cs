// 
// Copyright 2011 - 2012 - Jeff Shergalis
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
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace StuceSoftware.Utilities
{
    /// <summary>
    /// Collection of helpful extension methods
    /// </summary>
    public static class ExtensionMethods
    {
        #region Data Validation Methods
        
        /// <summary>
        /// Throws a <see cref="ArgumentNullException"/> if the object is null.
        /// </summary>
        public static void ThrowIfNull<T>(this T item, string parameterName) where T: class
        {
            if (item == null)
            {
                var ee = new ArgumentNullException(parameterName);
                ee.Data["Object Type"] = typeof(T).ToString();
                throw ee;
            }
        }
        /// <summary>
        /// Throws an <see cref="ArgumentException" /> if <paramref name="item" />
        /// is <see cref="string.Empty" /> or <see langword="null"/>.
        /// </summary>
        /// <param name="item">The string to check.</param>
        /// <param name="parameterName">The name of the parameter to add to the exception.</param>
        public static void ThrowIfNullOrEmpty(this string item, string parameterName)
        {
            if (string.IsNullOrEmpty(item))
            {
                throw new ArgumentException("String was null or empty.", parameterName);
            }
        }

        #endregion
        
        #region Type Helpers
        
        /// <summary>
        /// Checks whether the object passed in is "Nullable".
        /// </summary>
        /// <param name="obj">The object to check.</param>
        /// <returns><see langword="true" /> if the object is
        /// nullable and <see langword="false" /> if it is not.</returns>
        public static bool IsNullable<T>(this T obj)
        {
            // This code has been copied (and slightly edited)
            // from a great StackOverflow post
            // by Marc Gravell - http://stackoverflow.com/users/23354/marc-gravell
            // Answer: http://stackoverflow.com/a/374663/416574
            
            if (obj == null) return true; // obvious
            
            Type type = typeof(T);
            if (!type.IsValueType // ref-type
                || Nullable.GetUnderlyingType(type) != null) // Nullable<T>
            {
                return true;
            }

            return false; // value-type
        }

        #endregion
        
        #region Array Methods
        
        /// <summary>
        /// Returns a new array containing a copy of the specified range of elements
        /// </summary>
        /// <param name="source">The source array to copy from</param>
        /// <param name="start">The start position in the array to copy from</param>
        /// <param name="end">The end position in the array to copy from</param>
        public static T[] Slice<T>(this T[] source, int start, int end)
        {
            if (end <= start)
            {
                throw new ArgumentException("End must be greater than start.");
            }
            if (start < 0 || start > source.Length)
            {
                throw new ArgumentOutOfRangeException("start");
            }
            if (end < 0 || end > source.Length)
            {
                throw new ArgumentOutOfRangeException("end");
            }
            
            var destination = new T[end - start + 1];
            Array.Copy(source, start, destination, 0, destination.Length);
            return destination;
        }

        #endregion Array Methods
        
        #region Exception Methods
        
        /// <summary>
        /// Adds the specified data value to the <see cref="Exception" /> data
        /// collection. If the <paramref name="key" /> already exists in the collection
        /// then '_#' will be appended to the key until a unique key is found.
        /// </summary>
        /// <param name="ex">The exception to add the debug data to.</param>
        /// <param name="key">The key for the data to add.</param>
        /// <param name="data">The data to be added.</param>
        public static void AddDebugData<T>(this Exception ex, string key, T data)
        {
            ex.ThrowIfNull("ex");
            key.ThrowIfNullOrEmpty("key");

            key = ex.GetUniqueKey(key);
            if (data.IsNullable() && data == null)
            {
                ex.Data.Add(key, "<null>");
            }
            else
            {
                ex.Data.Add(key, data);
            }
        }
        
        /// <summary>
        /// Generates a unique key for the exception data collection.
        /// </summary>
        private static string GetUniqueKey(this Exception ex, string key)
        {
            if (ex.Data.Contains(key))
            {
                string keyFormat = key + "_{D}";
                int i=1;
                do
                {
                    key = string.Format(CultureInfo.InvariantCulture, keyFormat, i++);
                }
                while (ex.Data.Contains(key));
            }
            
            return key;
        }
        
        #endregion
        
        #region Assembly Methods

        /// <summary>
        /// Gets a collection of custom attributes of the specified type from the <see cref="Assembly" />
        /// </summary>
        /// <param name="assembly">The assembly to search for the custom attributes.</param>
        /// <returns>A collection of custom attributes.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static ReadOnlyCollection<T> GetCustomAttributes<T>(this Assembly assembly) where T : class
        {
            return assembly.GetCustomAttributes(typeof(T), false)
                .Select(x => x as T)
                .ToList().AsReadOnly();
        }
        
        /// <summary>
        /// Gets the first custom attribute of the specified type from the <see cref="Assembly" />.
        /// If the specified custom attribute type is no found on the assembly then returns
        /// <see langword="null" />
        /// </summary>
        /// <param name="assembly">The assembly to search for the custom attributes.</param>
        /// <returns>Either the first custom attribute of the specified type, or
        /// <see langword="null" />.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static T GetFirstCustomAttribute<T>(this Assembly assembly) where T : class
        {
            return assembly.GetCustomAttributes(typeof(T), false)
                .Select(x => x as T)
                .FirstOrDefault();
        }
        
        #endregion Assembly Methods
    }
}
