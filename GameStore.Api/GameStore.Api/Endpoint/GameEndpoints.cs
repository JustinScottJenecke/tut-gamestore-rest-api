using GameStore.Api.Data;
using GameStore.Api.Dto;
using GameStore.Api.Entity;
using GameStore.Api.Mapping;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Endpoint;

public static class GameEndpoints
{

    // constant name for get single game
    const string GET_GAME_ENDPOINT_NAME = "GetGameById";
    

    // in memory datastore
    private static readonly List<GameDto> gameDatastore = [
        new GameDto(1, "SF2", "Fighting", 199.99M, new DateOnly(1992, 7, 15)),
        new GameDto(2, "FFXIV", "RPG", 699.99M, new DateOnly(2010, 9, 30)),
        new GameDto(3, "FIFA17", "Sport", 899.99M, new DateOnly(2016, 8, 13))
    ];

    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {
        var endpointGroup = app.MapGroup("/game")
            .WithParameterValidation();

        // GET - Read Games
        endpointGroup.MapGet("/", () => gameDatastore);

        // GET - Read by id
        endpointGroup.MapGet("/{id}", (int id) =>
        {
            GameDto? readGame = gameDatastore.Find(game => game.Id == id);

            return readGame is null ? Results.NotFound() : Results.Ok(readGame);
        })
            .WithName(GET_GAME_ENDPOINT_NAME);

        // POST - Create
        endpointGroup.MapPost("/", (CreateGameDto gameDto, GameStoreContext dbContext) =>
        {
            // Map CreateGameDto to Game Entity class
            // Game newGame = new Game() {
            //     Name = gameDto.Name,
            //     GenreId = gameDto.GenreId,
            //     Genre = dbContext.Genres.Find(gameDto.GenreId),
            //     Price = gameDto.Price,
            //     ReleaseDate = gameDto.ReleaseDate  
            // };

            Game newGame = gameDto.MapToEntity();
            newGame.Genre = dbContext.Genres.Find(gameDto.GenreId);

            // Add created game to Games DbSet
            dbContext.Games.Add(newGame);
            dbContext.SaveChanges();

            // transform game back into DTO since we never send internal models/entity to client. only Dto
            // GameDto returnedDto = new GameDto (
            //     newGame.Id,
            //     newGame.Name,
            //     newGame.Genre!.Name,
            //     newGame.Price,
            //     newGame.ReleaseDate
            // );
            
            GameDto returnedDto = newGame.MapToGameDto();

            // return created Game as response to request
            // 1st arg - location property
            // 2nd arg - path param value
            // 3rd arg - request body
            return Results.CreatedAtRoute(GET_GAME_ENDPOINT_NAME, new {id = newGame.Id}, returnedDto);

        });

        // PUT - Update
        endpointGroup.MapPut("/{id}", (int id, UpdateGameDto updatedGame) =>
        {
            // filters games and returns index if id exists
            var index = gameDatastore.FindIndex(game => game.Id == id);

            if (index == -1)
                return Results.NotFound();

            // create new game record
            gameDatastore[index] = new GameDto(
                id,
                updatedGame.Name,
                updatedGame.Genre,
                updatedGame.Price,
                updatedGame.ReleaseDate
            );

            return Results.NoContent();
        });

        // DELETE - Delete by ID
        endpointGroup.MapDelete("/{id}", (int id) =>
        {
            var deleted = gameDatastore.RemoveAll(game => game.Id == id);

            return deleted >= 1 ? Results.Accepted() : Results.NotFound();
        });

        return endpointGroup;
    }

}
