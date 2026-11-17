using Microsoft.AspNetCore.Identity;
namespace Web_Final_Project_sem_5.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
