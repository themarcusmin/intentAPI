using FluentValidation.Results;
using IntentAPI.DTO;
using IntentAPI.Models;
using IntentAPI.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
                    Recurring = createEventDTO.RepeatMode == RecurringMode.NEVER.ToString() ? null : new Recurring
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

        [HttpGet]
        [Authorize]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetEvents([FromQuery] GetEventDTO getEventDTO)
        {
            GetEventValidator validator = new GetEventValidator();
            ValidationResult result = validator.Validate(getEventDTO);

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
                var events = context.Events
                    .Where(e =>
                                e.FirebaseUserId == firebaseUserid &&
                                (
                                    (e.StartTime >= getEventDTO.EventStartTime &&
                                    e.EndTime <= getEventDTO.EventEndTime)
                                    ||
                                    (e.Recurring.EndDate >= getEventDTO.EventStartTime &&
                                    e.Recurring.EndDate <= getEventDTO.EventEndTime)
                                ))
                    .GroupJoin(context.Recurrings, e => e.EventId, r => r.EventId,
                                (e, r) => new
                                {
                                    Event = e,
                                })
                    .Select(x => new
                    {
                        x.Event.EventId,
                        x.Event.Title,
                        x.Event.Description,
                        x.Event.Location,
                        x.Event.StartTime,
                        x.Event.EndTime,
                        RecurringMode = x.Event.Recurring == null
                                    ? null
                                    : x.Event.Recurring.RecurringMode.ToString(),
                        RecurringEndDate = (DateTime?)x.Event.Recurring.EndDate
                    })
                    .ToList();

                return Ok(events);
            };
        }

        [HttpDelete]
        [Authorize]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DeleteEvent([FromQuery] DeleteEventDTO deleteEventDTO)
        {
            DeleteEventValidator validator = new DeleteEventValidator();
            ValidationResult result = validator.Validate(deleteEventDTO);

            if (!result.IsValid)
            {
                return BadRequest(new
                {
                    Message = result.Errors[0].ErrorMessage
                });
            }

            using (var context = new AppDbContext())
            {
                int Id = int.Parse(deleteEventDTO.Id);
                context.Events.Where(e=> e.EventId == Id).ExecuteDelete();
                return NoContent();
            }
        }
    }
}
