using FluentValidation.Results;
using IntentAPI.DTO;
using IntentAPI.Models;
using IntentAPI.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace IntentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        [HttpPost("create")]
        [Authorize]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

            var firebaseUserid = HttpContext.Items["FirebaseUserId"];
            using (var context = new AppDbContext())
            {
                var user = context.Users.Single(u => u.FirebaseUserId == firebaseUserid.ToString());
                var newEvent = new Event
                {
                    User = user,
                    FirebaseUserId = firebaseUserid.ToString(),
                    Title = createEventDTO.Title,
                    Description = createEventDTO.Description,
                    Location = createEventDTO.Location,
                    StartTime = createEventDTO.EventStartTime,
                    EndTime = createEventDTO.EventEndTime,
                    Recurring = createEventDTO.RepeatMode == RecurringMode.never.ToString() ? null : new Recurring
                    {
                        RecurringMode = (RecurringMode)Enum.Parse(typeof(RecurringMode), createEventDTO.RepeatMode),
                        EndDate = (DateTime)createEventDTO.RepeatEndDate!
                    }
                };
                user.Events.Add(newEvent);
                context.SaveChanges();
                return CreatedAtAction(nameof(CreateEvent), new { id = newEvent.EventId });
            };
        }
    }
}
