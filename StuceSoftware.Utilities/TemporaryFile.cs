// 
// Copyright 2009 - 2012 - Jeff Shergalis
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
using System.IO;

namespace StuceSoftware.Utilities
{
    /// <summary>
    /// Class for generating and temporary files.  The file will be created in the
    /// constructor and then cleaned up when the class is disposed.
    /// </summary>
    public class TemporaryFile : IDisposable
    {
        #region Fields
        
        /// <summary>
        /// Gets the name of the temporary file.
        /// </summary>
        public string FileName { get; private set; }
        
        #endregion
        
        #region Constructors
        
        /// <overloads>
        /// Initializes a new instance of the <see cref="TemporaryFile"/> class.
        /// </overloads>
        /// <summary>
        /// Initializes a new instance of the <see cref="TemporaryFile"/> class.
        /// </summary>
        public TemporaryFile() : this(null, null)
        {
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="TemporaryFile"/> class.
        /// </summary>
        /// <param name="extension">The extension to append to the temporary file.</param>
        public TemporaryFile(string extension) : this(extension, null)
        {
        }
        
        /// <summary>
        /// Intitializes a new instance of the <see cref="TemporaryFile"/> class.
        /// </summary>
        /// <param name="extension">The extension to append to the temporary file.</param>
        /// <param name="directory">The directory to create the temporary file in.</param>
        public TemporaryFile(string extension, string directory)
        {
            CreateTemporaryFile(extension, directory);
        }
        
        #endregion
        
        #region Methods
        
        /// <summary>
        /// Generates a unique temporary file with the specified extension in the specified
        /// directory.
        /// </summary>
        /// <param name="extension">The extension for the generated file.</param>
        /// <param name="directory">The directory to generate the file in.</param>
        void CreateTemporaryFile(string extension, string directory)
        {
            // If no path is specified use system temp path
            if (string.IsNullOrEmpty(directory))
            {
                directory = Path.GetTempPath();
            }
            // Ensure the directory exists
            else if (!Directory.Exists(directory))
            {
                DirectoryNotFoundException ex = new DirectoryNotFoundException();
                ex.AddDebugData("Directory Name", directory);
                throw ex;
            }
            
            // If no extension specified use .tmp
            if (string.IsNullOrEmpty(extension))
            {
                extension = ".tmp";
            }
            // Ensure there is a '.' on the extension
            else if (extension[0] != '.')
            {
                extension = "." + extension;
            }
            
            while(true)
            {
                string fileName = Path.Combine(directory, Path.GetRandomFileName());
                fileName += extension;
                if (!File.Exists(fileName))
                {
                    try
                    {
                        // try to create the file
                        new FileStream(fileName, FileMode.CreateNew).Dispose();
                        FileName = fileName;
                    }
                    catch(IOException)
                    {
                        // Just eat IO exceptions since they are caused by the file existing
                    }
                }
            }
        }
        
        #endregion
        
        #region IDisposable
        
        /// <summary>
        /// Tries to ensure that the temporary file is deleted
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        /// <summary>
        /// Performs file deletion.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        protected virtual void Dispose(bool disposing)
        {
            try
            {
                // Ensure there is a file name and the file still exists
                if (!string.IsNullOrWhiteSpace(FileName) && File.Exists(FileName))
                {
                    // Try to delete the file
                    File.Delete(FileName);
                }
            }
            catch
            {
                // Just eat exceptions in the dispose method
            }
        }
        
        /// <summary>
        /// Finalizer, last chance to clean up the temporary file if dispose was not called
        /// </summary>
        ~TemporaryFile()
        {
            Dispose(false);
        }
        
        #endregion
    }
}
