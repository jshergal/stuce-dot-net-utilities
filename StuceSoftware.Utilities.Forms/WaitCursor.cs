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

using System.Diagnostics.CodeAnalysis;

namespace StuceSoftware.Utilities.Forms;

/// <summary>
///     Wait cursor class can be used in a <see cref="using" /> statement
///     to display a wait cursor (hourglass).
/// </summary>
public sealed class WaitCursor : IDisposable
{
    // Note:
    // This class is based on an answer from Hans Passant on Stack Overflow:
    // http://stackoverflow.com/a/302865/416574
    //

    /// <summary>
    ///     Initializes a new instance of the <see cref="WaitCursor" /> class.
    /// </summary>
    public WaitCursor() => Enabled = true;

    /// <summary>
    ///     Gets/sets whether or not the wait cursor (hourglass) is displayed.
    /// </summary>
    public static bool Enabled
    {
        get => Application.UseWaitCursor;
        set
        {
            if (value != Application.UseWaitCursor)
            {
                Application.UseWaitCursor = value;

                // Must force a WM_SETCURSOR call for wait cursor
                // to be displayed/undisplayed properly
                NativeMethods.SetCursor(Form.ActiveForm);
            }
        }
    }

    /// <summary>
    ///     Sets the cursor back to normal.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
    public void Dispose()
    {
        Enabled = false;
    }
}