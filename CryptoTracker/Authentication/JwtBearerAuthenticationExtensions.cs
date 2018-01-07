using CryptoTracker.ServiceInterfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTracker.Authentication
{
    public static class JwtBearerAuthenticationExtensions
    {
        public static void AddJwt(this IServiceCollection services, string secretKey, string issuer, string audience)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));
                var tokenValidationParameters = new TokenValidationParameters
                {
                    // The signing key must match!
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signingKey,

                    // Validate the JWT Issuer (iss) claim
                    ValidateIssuer = true,
                    ValidIssuer = issuer,

                    // Validate the JWT Audience (aud) claim
                    ValidateAudience = true,
                    ValidAudience = audience,

                    // Validate the token expiry
                    ValidateLifetime = true,

                    // If you want to allow a certain amount of clock drift, set that here:
                    ClockSkew = TimeSpan.Zero
                };

                o.Events = new JwtBearerEvents
                {
                    OnTokenValidated = async (context) =>
                    {
                        var authService = context.HttpContext.RequestServices.GetRequiredService<IAuthenticationService>();

                        var result = await authService.ValidateTokenAsync(context.Principal);

                        if (!result)
                        {
                            context.Fail("Token invalid");
                        }
                    }
                };

                o.TokenValidationParameters = tokenValidationParameters;
            });
        }
    }
}
