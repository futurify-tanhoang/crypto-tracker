using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoTracker.Setup
{
    public class PermissionConstants
    {
        #region ROLE_PERMISSIONS
        public const string VIEW_ROLES = "VIEW_ROLES";
        public const string MODIFY_ROLE = "MODIFY_ROLE";
        public const string REMOVE_ROLE = "REMOVE_ROLE";
        #endregion

        #region CRYPTOCURRENCY_PERMISSIONS
        public const string VIEW_CRYPTOCURRENCY = "VIEW_CRYPTOCURRENCY";
        public const string MODIFY_CRYPTOCURRENCY = "MODIFY_CRYPTOCURRENCY";
        public const string REMOVE_CRYPTOCURRENCY = "REMOVE_CRYPTOCURRENCY";
        #endregion
    }
}
