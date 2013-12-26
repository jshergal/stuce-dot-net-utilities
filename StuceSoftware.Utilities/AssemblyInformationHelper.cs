// 
// Copyright 2012 - Jeff Shergalis
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
using System.Reflection;

namespace StuceSoftware.Utilities
{
    /// <summary>
    /// Helper class to pull attribute information from the specified assembly.
    /// </summary>
    public class AssemblyInformationHelper
    {
        #region Fields
        
        private readonly Assembly _assembly;
        
        private string _title;
        private string _version;
        private string _description;
        private string _product;
        private string _copyright;
        private string _company;

        
        #endregion Fields
        
        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyInformationHelper" /> class.
        /// This will initialize the class with the current calling assembly.
        /// <seealso cref="Assembly.GetCallingAssembly" />
        /// </summary>
        public AssemblyInformationHelper() : this(Assembly.GetCallingAssembly())
        {
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyInformationHelper" /> class
        /// with the specified <see cref="Assembly" />.
        /// If <paramref name="assembly" /> is <see langword="null" /> then this will initialize
        /// the class with the current calling assembly. <seealso cref="Assembly.GetCallingAssembly" />
        /// </summary>
        public AssemblyInformationHelper(Assembly assembly)
        {
            _assembly = assembly ?? Assembly.GetCallingAssembly();
        }
        
        #region Properties
        
        /// <summary>
        /// Gets the title for the <see cref="Assembly" />
        /// </summary>
        public string Title
        {
            get
            {
                if (!string.IsNullOrEmpty(_title))
                {
                    return _title;
                }
                var attribute = _assembly.GetFirstCustomAttribute<AssemblyTitleAttribute>();
                if (attribute != null && !string.IsNullOrEmpty(attribute.Title))
                {
                    _title = attribute.Title;
                }
                else
                {
                    _title = Path.GetFileNameWithoutExtension(_assembly.CodeBase);
                }
                
                return _title;
            }
        }

        /// <summary>
        /// Gets the version string for the <see cref="Assembly" />
        /// </summary>
        public string Version
        {
            get
            {
                if (string.IsNullOrEmpty(_version))
                {
                    _version = _assembly.GetName().Version.ToString();
                }
                return _version;
            }
        }

        /// <summary>
        /// Gets the description for the <see cref="Assembly" />
        /// </summary>
        public string Description
        {
            get
            {
                if (_description == null)
                {
                    var attribute = _assembly.GetFirstCustomAttribute<AssemblyDescriptionAttribute>();
                    _description = attribute != null ? attribute.Description : string.Empty;
                }
                return _description;
            }
        }

        /// <summary>
        /// Gets the product from the <see cref="Assembly" />
        /// </summary>
        public string Product
        {
            get
            {
                if (_product == null)
                {
                    var attribute = _assembly.GetFirstCustomAttribute<AssemblyProductAttribute>();
                    _product = attribute != null ? attribute.Product : string.Empty;
                }
                return _product;
            }
        }

        /// <summary>
        /// Gets the copyright for the <see cref="Assembly" />
        /// </summary>
        public string Copyright
        {
            get
            {
                if (_copyright == null)
                {
                    var attribute = _assembly.GetFirstCustomAttribute<AssemblyCopyrightAttribute>();
                    _copyright = attribute != null ? attribute.Copyright : string.Empty;
                }
                return _copyright;
            }
        }

        /// <summary>
        /// Gets the company for the <see cref="Assembly" />
        /// </summary>
        public string Company
        {
            get
            {
                if (_company == null)
                {
                    var attribute = _assembly.GetFirstCustomAttribute<AssemblyCompanyAttribute>();
                    _company = attribute != null ? attribute.Company : string.Empty;
                }
                return _company;
            }
        }
        
        #endregion

    }
}
