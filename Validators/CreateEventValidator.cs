﻿using FluentValidation;
using IntentAPI.DTO;
using IntentAPI.Models;

namespace IntentAPI.Validators
{
    public class CreateEventValidator: AbstractValidator<CreateEventDTO>
    {
        public CreateEventValidator()
        {
            RuleFor(createEventDTO => createEventDTO.Title)
                .NotNull()
                .NotEmpty();
            RuleFor(createEventDTO => createEventDTO.EventStartTime)
                .NotNull().NotEmpty();
            RuleFor(createEventDTO => createEventDTO.EventEndTime)
                .NotNull()
                .NotEmpty();
            RuleFor(createEventDTO => createEventDTO.EventEndTime)
                .Must((createEventDTO, EventEndTime) => DateTime.Compare(createEventDTO.EventStartTime, EventEndTime) < 0)
                .WithMessage("EventStartTime must be earlier than EventEndTime.");   
            RuleFor(createEventDTO => createEventDTO.RepeatMode)
                .NotNull()
                .NotEmpty()
                .IsEnumName(typeof(RecurringMode))
                .WithMessage("RepeatMode must be never, daily, weekly, monthly, or yearly.");
            RuleFor(createEventDTO => createEventDTO.RepeatEndDate)
                .NotNull()
                .NotEmpty()
                .When(createEventDTO => createEventDTO.RepeatMode != RecurringMode.never.ToString());
            RuleFor(createEventDTO => createEventDTO.RepeatEndDate)
                .Must((createEventDTO, RepeatEndDate) => DateTime.Compare(createEventDTO.EventEndTime, (DateTime)RepeatEndDate!) < 0)
                .When(createEventDTO => createEventDTO.RepeatEndDate != null)
                .WithMessage("RepeatEndDate must be later than EventEndTime.");
        }
    }
}