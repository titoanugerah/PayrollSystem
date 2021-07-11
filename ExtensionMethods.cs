using System.Linq;
using System.Security.Claims;

namespace Payroll
{
    public static class ExtensionMethods
    {
        public static string GetEmail(this ClaimsPrincipal user)
        {
            var claims = user.Claims.ToList();
            var email = claims.Where(x => x.Type == "Email");
            return (email.Count() > 0) ? email.FirstOrDefault().Value : "";
        }

        public static object GetAllClaims(this ClaimsPrincipal user)
        {
            return user.Claims.ToList();
        }

    }
}
