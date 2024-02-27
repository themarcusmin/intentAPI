using IntentAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IntentAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        AppDbContext dbContext = new AppDbContext();

        [HttpPost("register")]
        public IActionResult Register([FromQuery] User user)
        {
            dbContext.Add<User>(user);
            try
            {
                dbContext.SaveChanges();
                return Ok(new
                {
                    Message = "User successfully registered!"
                });
            }
            catch (DbUpdateException)
            {
                return Conflict(new
                {
                    Message = "Email already exists!"
                });
            }
        }
    }
}
