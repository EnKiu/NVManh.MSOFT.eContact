﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MS.ApplicationCore.Resource {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Resource {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resource() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("MS.ApplicationCore.Resource.Resource", typeof(Resource).Assembly);
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
        ///   Looks up a localized string similar to Enum_DegreeClassification_AverageGood.
        /// </summary>
        public static string Enum_DegreeClassification_AverageGood {
            get {
                return ResourceManager.GetString("Enum_DegreeClassification_AverageGood", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Thu quỹ hàng năm.
        /// </summary>
        public static string Enum_ExpenditurePlanType_INCREMENT_ANNUAL {
            get {
                return ResourceManager.GetString("Enum_ExpenditurePlanType_INCREMENT_ANNUAL", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Thu cho sự kiện.
        /// </summary>
        public static string Enum_ExpenditurePlanType_INCREMENT_EVENT {
            get {
                return ResourceManager.GetString("Enum_ExpenditurePlanType_INCREMENT_EVENT", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Thu khác.
        /// </summary>
        public static string Enum_ExpenditurePlanType_INCREMENT_OTHER {
            get {
                return ResourceManager.GetString("Enum_ExpenditurePlanType_INCREMENT_OTHER", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Chi khác (Ma chay, hiếu hỉ...).
        /// </summary>
        public static string Enum_ExpenditurePlanType_REDUCE_OTHER {
            get {
                return ResourceManager.GetString("Enum_ExpenditurePlanType_REDUCE_OTHER", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Chi cho sự kiện.
        /// </summary>
        public static string Enum_ExpenditurePlanType_REDURE_EVENT {
            get {
                return ResourceManager.GetString("Enum_ExpenditurePlanType_REDURE_EVENT", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Có lỗi xảy ra vui lòng liên hệ Quản trị viên để được trợ giúp.
        /// </summary>
        public static string Exception_Message_General {
            get {
                return ResourceManager.GetString("Exception_Message_General", resourceCulture);
            }
        }
    }
}
