using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Payroll
{
    public static class ExtensionMethods
    {
        public static int GetNIK(this ClaimsPrincipal user)
        {
            return Convert.ToInt32(GetClaim(user, "NIK"));
        }

        public static object GetClaim(this ClaimsPrincipal user, string type)
        {
            List<Claim> claims = user.Claims.ToList();
            var claimValue = claims.Where(claim => claim.Type.Contains(type)).FirstOrDefault().Value;
            return claimValue != null ? claimValue : "";
        }

    }
}
