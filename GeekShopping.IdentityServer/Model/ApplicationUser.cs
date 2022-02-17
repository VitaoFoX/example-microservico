using Microsoft.AspNetCore.Identity;

namespace GeekShopping.IdentityServer.Model.Context
{
    public class ApplicationUser : IdentityUser
    {
        private string FristName { get; set; }  
        private string LastName { get; set; }   

    }
}
