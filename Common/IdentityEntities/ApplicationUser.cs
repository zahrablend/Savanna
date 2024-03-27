using Microsoft.AspNetCore.Identity;

namespace Common.IdentityEntities;

public class ApplicationUser : IdentityUser<Guid>
{
    public string? NickName { get; set; }
}
