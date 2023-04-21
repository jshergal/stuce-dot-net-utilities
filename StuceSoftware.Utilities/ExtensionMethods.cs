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

using System.Globalization;
using System.Reflection;

namespace StuceSoftware.Utilities;

/// <summary>
///     Collection of helpful extension methods
/// </summary>
public static class ExtensionMethods
{
    #region Type Helpers

    /// <summary>
    ///     Checks whether the object passed in is "Nullable".
    /// </summary>
    /// <param name="obj">The object to check.</param>
    /// <returns>
    ///     <see langword="true" /> if the object is
    ///     nullable and <see langword="false" /> if it is not.
    /// </returns>
    public static bool IsNullable<T>(this T obj)
    {
        // This code has been copied (and slightly edited)
        // from a great StackOverflow post
        // by Marc Gravell - http://stackoverflow.com/users/23354/marc-gravell
        // Answer: http://stackoverflow.com/a/374663/416574

        if (obj is null) return true; // obvious

        var type = typeof(T);
        return !type.IsValueType // ref-type
            || Nullable.GetUnderlyingType(type) != null; // Nullable<T>
    }

    #endregion

    #region Array Methods

    /// <summary>
    ///     Returns a new array containing a copy of the specified range of elements
    /// </summary>
    /// <param name="source">The source array to copy from</param>
    /// <param name="start">The start position in the array to copy from</param>
    /// <param name="end">The end position in the array to copy from</param>
    public static T[] Slice<T>(this T[] source, int start, int end) => source[start..end];

    #endregion Array Methods

    #region Assembly Methods

    /// <summary>
    ///     Gets the first custom attribute of the specified type from the <see cref="Assembly" />.
    ///     If the specified custom attribute type is no found on the assembly then returns
    ///     <see langword="null" />
    /// </summary>
    /// <param name="assembly">The assembly to search for the custom attributes.</param>
    /// <returns>
    ///     Either the first custom attribute of the specified type, or
    ///     <see langword="null" />.
    /// </returns>
    public static T? GetFirstCustomAttribute<T>(this Assembly assembly) where T : class
        => assembly.GetCustomAttributes(typeof(T), false).OfType<T>().FirstOrDefault();

    #endregion Assembly Methods

    #region Exception Methods

    /// <summary>
    ///     Adds the specified data value to the <see cref="Exception" /> data
    ///     collection. If the <paramref name="key" /> already exists in the collection
    ///     then '_#' will be appended to the key until a unique key is found.
    /// </summary>
    /// <param name="ex">The exception to add the debug data to.</param>
    /// <param name="key">The key for the data to add.</param>
    /// <param name="data">The data to be added.</param>
    public static void AddDebugData<T>(this Exception ex, string key, T data)
    {
        if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("Value cannot be empty", nameof(key));

        key = ex.GetUniqueKey(key);
        if (data.IsNullable() && data is null)
            ex.Data.Add(key, "<null>");
        else
            ex.Data.Add(key, data);
    }

    /// <summary>
    ///     Generates a unique key for the exception data collection.
    /// </summary>
    private static string GetUniqueKey(this Exception ex, string key)
    {
        if (!ex.Data.Contains(key)) return key;
        var keyFormat = key + "_{D}";
        var i = 1;
        do
        {
            key = string.Format(CultureInfo.InvariantCulture, keyFormat, i++);
        } while (ex.Data.Contains(key));

        return key;
    }

    #endregion
}