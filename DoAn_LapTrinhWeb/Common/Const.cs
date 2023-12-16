using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAn_LapTrinhWeb.Common
{

    public class Const
    {
        // Admin quyền là "0", thành viên quyền là "1"

        public const string ROLE_ADMIN_NAME = "Admin";
        public const int ROLE_ADMIN_CODE = 0;

        public const string ROLE_MEMBER_NAME = "Member";
        public const int ROLE_MEMBER_CODE = 1;

        public const string ROLE_MOD_NAME = "Mod";
        public const int ROLE_MOD_CODE = 2;
    }

}