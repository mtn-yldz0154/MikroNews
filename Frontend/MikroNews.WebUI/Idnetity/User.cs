using Microsoft.AspNetCore.Identity;

namespace MikroNews.WebUI.Idnetity
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
