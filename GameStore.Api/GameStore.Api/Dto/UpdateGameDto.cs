using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.Dto;

public record class UpdateGameDto
(
    [Required][StringLength(50)] string Name,
    [Required][StringLength(20)] string Genre,
    [Range(10, 1000)] decimal Price,
    DateOnly ReleaseDate
);
