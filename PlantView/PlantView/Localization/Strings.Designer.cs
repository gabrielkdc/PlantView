﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34011
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PlantView.Localization {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Strings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Strings() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("PlantView.Localization.Strings", typeof(Strings).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An error has occurred while trying to make last operation. Please check the file ErrorLog.txt to get more details.
        /// </summary>
        public static string AnErrorOccurred {
            get {
                return ResourceManager.GetString("AnErrorOccurred", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An error occurred.
        /// </summary>
        public static string ErrorMessageTitle {
            get {
                return ResourceManager.GetString("ErrorMessageTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Events.
        /// </summary>
        public static string Events {
            get {
                return ResourceManager.GetString("Events", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Import Files.
        /// </summary>
        public static string ImportText {
            get {
                return ResourceManager.GetString("ImportText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Process Files.
        /// </summary>
        public static string ProcessFiles {
            get {
                return ResourceManager.GetString("ProcessFiles", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Select Files ....
        /// </summary>
        public static string SelectFiles {
            get {
                return ResourceManager.GetString("SelectFiles", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Total selected files: [b]{0}[/b].
        /// </summary>
        public static string TotalSelectedFiles {
            get {
                return ResourceManager.GetString("TotalSelectedFiles", resourceCulture);
            }
        }
    }
}
