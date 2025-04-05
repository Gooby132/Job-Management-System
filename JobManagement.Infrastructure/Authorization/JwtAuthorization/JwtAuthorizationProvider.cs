using JobManagement.Domain.Users;
using JobManagement.Infrastructure.Authorization.Core;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JobManagement.Infrastructure.Authorization.JwtAuthorization;

public class JwtAuthorizationProvider : IAuthorizationProvider
{

    #region Fields

    private readonly ILogger<JwtAuthorizationProvider> _logger;
    private readonly JwtAuthorizationOptions _config;
    private readonly SigningCredentials _credentials;

    #endregion

    public JwtAuthorizationProvider(
        ILogger<JwtAuthorizationProvider> logger,
        IOptions<JwtAuthorizationOptions> options)
    {
        _logger = logger;
        _config = options.Value;

        _credentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.PrivateKey)),
                SecurityAlgorithms.HmacSha256);
    }

    public string AuthorizeAdmin(User user) => new JwtSecurityTokenHandler()
        .WriteToken(
            new JwtSecurityToken(
            issuer: _config.Issuer,
            audience: _config.Audience,
            expires: DateTime.UtcNow.AddMinutes(_config.TimeoutInMinutes),
            signingCredentials: _credentials,
            claims: new[]
            {
                new Claim(ClaimTypes.Role, user.Role.Name),
                new Claim(ClaimTypes.NameIdentifier, user.Name.Value),
                new Claim(ClaimTypes.Name, user.Name.Value)
            }
        ));
}
