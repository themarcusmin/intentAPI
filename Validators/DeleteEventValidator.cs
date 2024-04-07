using FluentValidation;
using IntentAPI.DTO;

namespace IntentAPI.Validators
{
    public class DeleteEventValidator: AbstractValidator<DeleteEventDTO>
    {
        public DeleteEventValidator()
        {
            RuleFor(DeleteEventDTO => DeleteEventDTO.IdAsInt)
                .NotNull()
                .NotEmpty();
        }
    }
}