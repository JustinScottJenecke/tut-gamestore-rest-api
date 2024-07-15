using GameStore.Api.Dto;
using GameStore.Api.Entity;

namespace GameStore.Api.Mapping;

public static class GameMapping
{
    public static Game MapToEntity(this CreateGameDto gameDto) 
    {
        return new Game() 
        {
                Name = gameDto.Name,
                GenreId = gameDto.GenreId,
                Price = gameDto.Price,
                ReleaseDate = gameDto.ReleaseDate  
        };
    }

    public static GameSummaryDto MapToGameSummaryDto(this Game gameEntity)
    {
        return new GameSummaryDto (
                gameEntity.Id,
                gameEntity.Name,
                gameEntity.Genre!.Name,
                gameEntity.Price,
                gameEntity.ReleaseDate
            );
    }
}
