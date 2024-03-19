using System.ComponentModel.DataAnnotations;

namespace Restaurants.Application.Restaurants.Dtos;

public class CreateRestaurantDto
{
    [StringLength(100, MinimumLength = 3)]
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    [Required(ErrorMessage = "Insert a valid Category")]

    public string Category { get; set; } = default!;
    public bool HasDelivery { get; set; }

    [EmailAddress(ErrorMessage = "please provide a valid Email address")]
    public string? ContactEmail { get; set; }
    [Phone(ErrorMessage = "please provide a valid Phone number")]
    public string? ContactNumber { get; set; }

    public string? City { get; set; }
    public string? Street { get; set; }

    [RegularExpression(@"^\d{2}-\d{3}$", ErrorMessage = "Please provide a valid Postal Code (XX-XXX).")]
    public string? PostalCode { get; set; }
}