using Microsoft.AspNetCore.Identity;

namespace Common.IdentityEntities
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public ApplicationRole() 
        {
        }
    }
}
