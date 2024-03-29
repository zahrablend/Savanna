using Microsoft.AspNetCore.Identity;

namespace Common.Identity;

public class ApplicationUser : IdentityUser<Guid>
{
    public string? NickName { get; set; }
}
