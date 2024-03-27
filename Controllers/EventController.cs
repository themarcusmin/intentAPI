using FluentValidation.Results;
using IntentAPI.DTO;
using IntentAPI.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace IntentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        [HttpPost("create")]
        [Authorize]
        public IActionResult CreateEvent([FromBody] CreateEventDTO createEventDTO)
        {
            CreateEventValidator validator = new CreateEventValidator();
            ValidationResult result = validator.Validate(createEventDTO);

            if (!result.IsValid)
            {
                return BadRequest(new
                {
                    Message = result.Errors[0].ErrorMessage
                });
            }
            //
            //CreateEventValidator validator = new CreateEventValidator();
            //ValidationResult results = validator.Validate(createEventDTO);
            return Ok(
                new
                {
                    Message = "here"
                });
            //var firebaseUserid = HttpContext.Items["FirebaseUserId"];
            //using (var context = new AppDbContext())
            //{
            //    //var user = context.Users.Single(u => u.Auth0UserId == JWTAuth0UserId);
            //    var user = context.Users.Single(u => u.FirebaseUserId == firebaseUserid);
            //    var newEvent = new Event
            //    {
            //        User = user,
            //        Title = createEventDTO.Title,
            //        Description = createEventDTO.description,
            //        Location = createEventDTO.location,
            //        StartTime = createEventDTO.eventStartTime,
            //        EndTime = createEventDTO.eventEndTime,
            //        IsRecurring = createEventDTO.repeatMode == RecurringMode.never ? false : true,
            //        Recurring = createEventDTO.repeatMode == RecurringMode.never ? null : new Recurring
            //        {
            //            RecurringMode = createEventDTO.repeatMode,
            //            EndDate = (DateTime)createEventDTO.repeatEndDate
            //        }
            //    };
            //    user.Events.Add(newEvent);
            //    context.SaveChanges();
            //}
            //return Ok();
        }
    }
}
