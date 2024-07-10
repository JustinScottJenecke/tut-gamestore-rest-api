using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.Dto;

public record class CreateGameDto
( 
    [Required][StringLength(50)] string Name, 
    [Required] int GenreId, 
    [Range(10, 1000)] decimal Price,
    DateOnly ReleaseDate
);
