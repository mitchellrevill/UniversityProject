using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace UniversityProject.Interfaces
{
    public interface IAuthStrategy
    {
        /// <summary>
        /// Try to authenticate this request.
        /// </summary>
        /// <returns>
        ///   true if this strategy recognizes the request and produces a valid ClaimsPrincipal.
        /// </returns>
        bool Authenticate(HttpListenerRequest req, out ClaimsPrincipal claimsPrincipal);
    }
}
