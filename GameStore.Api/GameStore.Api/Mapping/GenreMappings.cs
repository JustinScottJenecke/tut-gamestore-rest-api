using GameStore.Api.Dto;
using GameStore.Api.Entity;

namespace GameStore.Api.Mapping;

public static class GenreMappings
{
    public static GenreDto MapToDto(this Genre genre) {
        return new GenreDto(genre.Id, genre.Name);
    }
}
