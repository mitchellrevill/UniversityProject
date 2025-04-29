using System.Collections.Generic;
using System.Security.Claims;
using UniversityProject.Interfaces;     
using UniversityProject;
using static MethodHandle;
public static class AuthRegistry
{
    
    public static IList<IAuthStrategy> Strategies { get; } = new List<IAuthStrategy>();

    public static void Initialize()
    {
        // 1. JWT strategy
        var signingKey = System.Text.Encoding.UTF8
            .GetBytes("YourVerySecureKeyEvenThisWasntLongEnoughBlyat123456789");
        Strategies.Add(new JwtAuthStrategy(
            issuer: "mitchellrevill",
            audience: "AccessAPI",
            signingKey: signingKey));

        // 2. API‐Key strategy (example keys—substitute your own)
        Strategies.Add(new ApiKeyAuthStrategy(new[] { "key-one", "key-two" }));
    }
}
