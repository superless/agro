﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace trifenix.connect.agro.queries {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class QueryRes {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal QueryRes() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("trifenix.connect.agro.queries.QueryRes", typeof(QueryRes).Assembly);
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
        ///   Looks up a localized string similar to SELECT DISTINCT value c.id from c where c.IdProduct=&apos;{0}&apos; and c.Active=true and c.Default=false.
        /// </summary>
        public static string ACTIVEDOSESIDS_FROM_PRODUCTID {
            get {
                return ResourceManager.GetString("ACTIVEDOSESIDS_FROM_PRODUCTID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT value c.id FROM c where c.IdBusinessName = &apos;{0}&apos;.
        /// </summary>
        public static string BUSINESSNAME_FROM_COSTCENTER {
            get {
                return ResourceManager.GetString("BUSINESSNAME_FROM_COSTCENTER", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT value c.ClientId FROM c where c.id =&apos;{0}&apos;.
        /// </summary>
        public static string CORRELATIVE_FROM_DOSESID {
            get {
                return ResourceManager.GetString("CORRELATIVE_FROM_DOSESID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT value count(1) FROM c where c.id = &apos;{0}&apos;.
        /// </summary>
        public static string COUNT_BY_ID {
            get {
                return ResourceManager.GetString("COUNT_BY_ID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT value count(1) FROM c where c.{0} = &apos;{1}&apos;.
        /// </summary>
        public static string COUNT_BY_NAMEVALUE {
            get {
                return ResourceManager.GetString("COUNT_BY_NAMEVALUE", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT value count(1) FROM c where c.{0} = &apos;{1}&apos;  and c.id != &apos;{2}&apos;.
        /// </summary>
        public static string COUNT_BY_NAMEVALUE_AND_NOID {
            get {
                return ResourceManager.GetString("COUNT_BY_NAMEVALUE_AND_NOID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT value count(1) FROM c where c.IdProduct =&apos;{0}&apos;.
        /// </summary>
        public static string COUNT_DOSES_BY_PRODUCTID {
            get {
                return ResourceManager.GetString("COUNT_DOSES_BY_PRODUCTID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT value count(1) from c join dosesOrder in c.DosesOrder where dosesOrder.IdDoses = &apos;{0}&apos;.
        /// </summary>
        public static string COUNT_EXECUTION_OR_ORDERS_BY_DOSESID {
            get {
                return ResourceManager.GetString("COUNT_EXECUTION_OR_ORDERS_BY_DOSESID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT DISTINCT value c.id from c where c.IdProduct=&apos;{0}&apos; and c.Active=true and c.Default=true.
        /// </summary>
        public static string DEFAULTDOSESID_BY_PRODUCTID {
            get {
                return ResourceManager.GetString("DEFAULTDOSESID_BY_PRODUCTID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT value c.IdBarrack[0] FROM c where  c.id = &apos;{0}&apos;.
        /// </summary>
        public static string IDBARRACK_FROM_ORDERID {
            get {
                return ResourceManager.GetString("IDBARRACK_FROM_ORDERID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT DISTINCT value c.Email  FROM c join Rol in c.IdsRoles where Rol IN ({0}).
        /// </summary>
        public static string MAILUSERS_FROM_ROLES {
            get {
                return ResourceManager.GetString("MAILUSERS_FROM_ROLES", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT value max(c.ClientId) from c where c.IdProduct =&apos;{0}&apos;.
        /// </summary>
        public static string MAXCORRELATIVE_DOSES_BY_PRODUCTID {
            get {
                return ResourceManager.GetString("MAXCORRELATIVE_DOSES_BY_PRODUCTID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT value c.Name FROM c where  c.id = &apos;{0}&apos;.
        /// </summary>
        public static string NAME_BY_ID {
            get {
                return ResourceManager.GetString("NAME_BY_ID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT value c.SeasonId FROM c where c.id = &apos;{0}&apos;.
        /// </summary>
        public static string SEASONID_FROM_BARRACKID {
            get {
                return ResourceManager.GetString("SEASONID_FROM_BARRACKID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT value c.Abbreviation FROM c where  c.id = &apos;{0}&apos;.
        /// </summary>
        public static string SPECIEABBREVIATION_FROM_SPECIEID {
            get {
                return ResourceManager.GetString("SPECIEABBREVIATION_FROM_SPECIEID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT value c.IdSpecie FROM c where  c.id = &apos;{0}&apos;.
        /// </summary>
        public static string SPECIEID_FROM_VARIETYID {
            get {
                return ResourceManager.GetString("SPECIEID_FROM_VARIETYID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT value c.id FROM c where c.ObjectIdAAD = &apos;{0}&apos;.
        /// </summary>
        public static string USERID_FROM_IDAAD {
            get {
                return ResourceManager.GetString("USERID_FROM_IDAAD", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT value c.IdVariety FROM c where  c.id = &apos;{0}&apos;.
        /// </summary>
        public static string VARIETYID_FROM_BARRACKID {
            get {
                return ResourceManager.GetString("VARIETYID_FROM_BARRACKID", resourceCulture);
            }
        }
    }
}
