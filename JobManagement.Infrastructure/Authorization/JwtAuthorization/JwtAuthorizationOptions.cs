using System.ComponentModel.DataAnnotations;

namespace JobManagement.Infrastructure.Authorization.JwtAuthorization;

public class JwtAuthorizationOptions
{

    [Required]
    public required string PrivateKey { get; init; }

    [Required]
    public required string Issuer { get; set; }

    [Required]
    public required string Audience { get; set; }

    [Required]
    [Range(5, 60)]
    public required int TimeoutInMinutes { get; set; }

}
