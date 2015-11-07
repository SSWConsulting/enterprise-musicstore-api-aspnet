using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SSW.MusicStore.API.Helpers
{
    public static class ClaimExtensions
    {
        /// <summary>
        /// Gets the name identifier from user claims.
        /// </summary>
        /// <param name="claims">The claims.</param>
        /// <returns></returns>
        public static string GetNameIdentifier(this IEnumerable<Claim> claims)
        {
            return
                claims.FirstOrDefault(
                    c =>
                        c.Type.Equals(
                            "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier",
                            StringComparison.OrdinalIgnoreCase))?.Value;
        }
    }
}
