using FluentValidation;
using IntentAPI.DTO;

namespace IntentAPI.Validators
{
    public class GetEventValidator : AbstractValidator<GetEventDTO>
    {
        public GetEventValidator()
        {
            RuleFor(getEventDTO => getEventDTO.EventStartTime)
                .NotNull()
                .NotEmpty();
            RuleFor(getEventDTO => getEventDTO.EventEndTime)
                .NotNull()
                .NotEmpty();
            RuleFor(getEventDTO => getEventDTO.EventEndTime)
                .Must((getEventDTO, EventEndTime) => DateTime.Compare(getEventDTO.EventStartTime, EventEndTime) < 0)
                .WithMessage("EventStartTime must be earlier than EventEndTime.");
        }
    }
}