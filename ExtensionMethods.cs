using Payroll.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Payroll
{
    public static class ExtensionMethods
    {
        public static int GetEmployeeId(this ClaimsPrincipal user)
        {
            return int.Parse(GetClaim(user, "Id").ToString());
        }

        public static string GetRole(this ClaimsPrincipal user)
        {
            return GetClaim(user, "role").ToString();
        }

        public static string GetName(this ClaimsPrincipal user)
        {
            return GetClaim(user, "name").ToString();
        }

        public static UserIdentity GetUserIdentity(this ClaimsPrincipal user)
        {
            UserIdentity userIdentity = new UserIdentity();
            userIdentity.EmployeeId = GetEmployeeId(user);
            userIdentity.Name = GetName(user);
            userIdentity.Role = GetRole(user);
            return userIdentity;
        }

        public static object GetClaim(this ClaimsPrincipal user, string type)
        {
            List<Claim> claims = user.Claims.ToList();
            var claimValue = claims.Where(claim => claim.Type.Contains(type)).FirstOrDefault().Value;
            return claimValue != null ? claimValue : "";
        }

    }
}
