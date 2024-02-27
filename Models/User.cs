using Microsoft.EntityFrameworkCore;

namespace IntentAPI.Models
{
    [Index(nameof(Email), IsUnique = true)]
    public class User
    {
        public int UserId { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        
        public string Email { get; set; }
    }

}