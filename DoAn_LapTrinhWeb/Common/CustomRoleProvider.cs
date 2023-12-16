using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace DoAn_LapTrinhWeb.Common
{
	public class CustomRoleProvider : RoleProvider
	{
		public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		public override void AddUsersToRoles(string[] usernames, string[] roleNames)
		{
			throw new NotImplementedException();
		}

		public override void CreateRole(string roleName)
		{
			throw new NotImplementedException();
		}

		public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
		{
			throw new NotImplementedException();
		}

		public override string[] FindUsersInRole(string roleName, string usernameToMatch)
		{
			throw new NotImplementedException();
		}

		public override string[] GetAllRoles()
		{
			throw new NotImplementedException();
		}

		public override string[] GetRolesForUser(string userData)
		{
			var loggedUserData = JsonConvert.DeserializeObject<LoggedUserData>(userData);
			if (loggedUserData.RoleCode == Const.ROLE_ADMIN_CODE)
			{
				return new string[] { Const.ROLE_ADMIN_NAME };
			}
			else if (loggedUserData.RoleCode == Const.ROLE_MOD_CODE)
            {
				return new string[] { Const.ROLE_MOD_NAME };
			}
			else
			{
				return new string[] { Const.ROLE_MEMBER_NAME };
			}
		}

		public override string[] GetUsersInRole(string roleName)
		{
			throw new NotImplementedException();
		}

		public override bool IsUserInRole(string username, string roleName)
		{
			throw new NotImplementedException();
		}

		public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
		{
			throw new NotImplementedException();
		}

		public override bool RoleExists(string roleName)
		{
			throw new NotImplementedException();
		}
	}
}