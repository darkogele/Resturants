using FluentValidation;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommandValidator : AbstractValidator<CreateRestaurantCommand>
{
    private readonly string[] _categories =
    [
        "FastFood", "Traditional", "Vegetarian", "Vegan", "Italian", "Mexican", "Chinese", "Japanese", "American"
    ];

    public CreateRestaurantCommandValidator()
    {
        RuleFor(x => x.Name)
            .Length(3, 100).NotEmpty();

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.");

        RuleFor(x => x.Category)
            .NotEmpty()
            .Must(_categories.Contains)
            .WithMessage("Invalid category, Please choose from the valid categories.");
        //.Custom((value, context) => // First approach
        //{
        //    if (!_categories.Contains(value))
        //        context.AddFailure("Category", "Invalid category, Please choose from the valid categories.");
        //});

        RuleFor(x => x.ContactEmail)
            .EmailAddress()
            .WithMessage("Please provide a valid email address");

        RuleFor(x => x.PostalCode)
            .Matches(@"^\d{2}-\d{3}$")
            .WithMessage("Please provide a valid Postal Code (XX-XXX).");
    }
}