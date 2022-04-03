using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Automaton.Studio.Server.Auth
{
    public static class AuthenticationServiceCollectionExtensions
    {
        public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration, TokenValidationParameters parameters = null)
        {
            services.AddHttpContextAccessor();

            var authSettings = new AuthenticationSettings();
            configuration.GetSection(nameof(AuthenticationSettings)).Bind(authSettings);

            var jwtOptions = new JwtOptions();
            configuration.GetSection(nameof(JwtOptions)).Bind(jwtOptions);
            jwtOptions.SigningCredentials = GetCredentialSigningKey(authSettings);

            services.TryAddSingleton(jwtOptions);
            services.TryAddSingleton(authSettings);
            services.TryAddSingleton<IJwtService, JwtService>();

            var validationParameters = new TokenValidationParameters
            {
                // Ensure the token was issued by a trusted authorization server (default true)
                ValidateIssuer = jwtOptions.ValidateIssuer,
                ValidIssuer = jwtOptions.Issuer,
                // Ensure the token audience matches our audience value (default true)
                ValidateAudience = jwtOptions.ValidateAudience,
                ValidAudience = jwtOptions.Audience,
                // Specify the key used to sign the token
                ValidateIssuerSigningKey = true,
                RequireSignedTokens = true,
                IssuerSigningKey = jwtOptions.SigningCredentials.Key,
                // Ensure the token hasn't expired
                RequireExpirationTime = true,
                ValidateLifetime = true,
                // Clock skew compensates for server time drift.
                ClockSkew = TimeSpan.FromMinutes(5)
            };

            services.AddAuthorization(options =>
            {
                options.AddPolicy(JwtBearerDefaults.AuthenticationScheme, policy =>
                {
                    policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireClaim(ClaimTypes.NameIdentifier);
                });
            });

            services.AddAuthentication()
                .AddJwtBearer(options =>
                {
                    options.Events = new JwtBearerEvents()
                    {
                        OnTokenValidated = tokenValidationContext => { return Task.CompletedTask; },
                        OnMessageReceived = messageReceivedContext => { return Task.CompletedTask; },
                        OnAuthenticationFailed = authenticationFailedContext =>
                        {
                            if (authenticationFailedContext.Exception.GetType() ==
                                typeof(SecurityTokenExpiredException))
                            {
                                authenticationFailedContext.Response.Headers.Add("Token-Expired", "true");
                            }

                            return Task.CompletedTask;
                        }
                    };

                    options.RequireHttpsMetadata = jwtOptions.RequireHttpsMetadata;
                    /// Set SaveTokens to save tokens to the AuthenticationProperties
                    /// https://www.jerriepelser.com/blog/aspnetcore-jwt-saving-bearer-token-as-claim/
                    /// https://www.jerriepelser.com/blog/accessing-tokens-aspnet-core-2/
                    options.SaveToken = jwtOptions.SaveToken;
                    options.ClaimsIssuer = jwtOptions.Issuer;
                    options.TokenValidationParameters = validationParameters;
                });
        }


        public static void AddAccessTokenValidator(this IServiceCollection services)
        {
            services.AddSingleton<IAccessTokenManagerService, AccessTokenManagerService>();
            services.AddSingleton<AccessTokenMiddleware>();
        }

        public static long ToUnixEpochDate(this DateTime date)
            => (long) Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                .TotalSeconds);


        #region Private Methods

        private static SigningCredentials GetCredentialSigningKey(AuthenticationSettings setting)
        {
            if (setting.RsaSettings != null)
            {
                return GetRsaKey(setting);
            }

            return GetHmacKey(setting);
        }

        private static SigningCredentials GetHmacKey(AuthenticationSettings setting)
        {
            if (setting.HMacSettings == null)
            {
                return null;
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(setting.HMacSettings.SecretKey));
            return new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        }

        private static SigningCredentials GetRsaKey(AuthenticationSettings setting)
        {
            if (setting.RsaSettings == null)
                return null;

            if (setting.RsaSettings.IsIssuer)
            {
                if (string.IsNullOrWhiteSpace(setting.RsaSettings.RsaPrivateKey))
                {
                    return null;
                }

                RSA privateRsa = RSA.Create();
                var privateKeyXml = File.ReadAllText(setting.RsaSettings.RsaPrivateKey);
                privateRsa.FromXmlString(privateKeyXml);
                var privateKey = new RsaSecurityKey(privateRsa);
                return new SigningCredentials(privateKey, SecurityAlgorithms.RsaSha256);
            }
            else
            {
                RSA publicRsa = RSA.Create();
                var publicKeyXml = File.ReadAllText(setting.RsaSettings.RsaPublicKey);
                publicRsa.FromXmlString(publicKeyXml);
                var publicKey = new RsaSecurityKey(publicRsa);
                return new SigningCredentials(publicKey, SecurityAlgorithms.RsaSha256);
            }
        }

        #endregion
    }
}