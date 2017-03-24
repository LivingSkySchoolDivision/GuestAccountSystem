using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSKYGuestAccountControl.Model
{
    public class UserPermissionResponse
    {
        public bool CanUserUseSystem { get; set; }
        public bool CanUserBypassLimits { get; set; }
        public bool CanUserViewLog { get; set; }
        public bool CanUserCreateBatches { get; set; }
    }
}