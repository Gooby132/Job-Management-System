using System.ComponentModel.DataAnnotations;

namespace JobManagement.Persistence.Users;

public class UserSeedOptions
{

    public const string Key = "Admin";

    [Required(AllowEmptyStrings = true)]
    public required string UserName { get; init; }

    [Required(AllowEmptyStrings = true)]
    public required string Password { get; init; }

}
