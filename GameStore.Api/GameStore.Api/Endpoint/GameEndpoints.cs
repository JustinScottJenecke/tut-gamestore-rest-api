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
    private static readonly List<GameSummaryDto> gameDatastore = [
        new (1, "SF2", "Fighting", 199.99M, new DateOnly(1992, 7, 15)),
        new (2, "FFXIV", "RPG", 699.99M, new DateOnly(2010, 9, 30)),
        new (3, "FIFA17", "Sport", 899.99M, new DateOnly(2016, 8, 13))
    ];

    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {
        var endpointGroup = app.MapGroup("/game")
            .WithParameterValidation();

        // GET - Read Games
        endpointGroup.MapGet("/", (GameStoreContext dbContext) => {
            return dbContext.Games
                .Include(game => game.Genre)
                .Select(game => game.MapToGameSummaryDto())
                .AsNoTracking();
        });

        // GET - Read by id
        endpointGroup.MapGet("/{id}", (int id, GameStoreContext dbContext) =>
        {
            Game? readGame = dbContext.Games.Find(id); // find game by id inside database

            return readGame is null ? Results.NotFound() : Results.Ok(readGame.MapToGameDetailsDto());
        })
        .WithName(GET_GAME_ENDPOINT_NAME);

        // POST - Create
        endpointGroup.MapPost("/", (CreateGameDto gameDto, GameStoreContext dbContext) =>
        {
            // map incoming dto to game entity object
            Game newGame = gameDto.MapToEntity();

            // can be removed since ef knows that genreid is foreign key and can connect tables for us
            // newGame.Genre = dbContext.Genres.Find(gameDto.GenreId);

            // Add created game to Games DbSet
            dbContext.Games.Add(newGame);
            dbContext.SaveChanges();

            // transform game back into DTO since we never send internal models/entity to client. only Dto           
            GameDetailsDto returnedDto = newGame.MapToGameDetailsDto();

            // return created Game as response to request
            // 1st arg - location property | 2nd arg - path param value | 3rd arg - request body
            return Results.CreatedAtRoute(GET_GAME_ENDPOINT_NAME, new {id = newGame.Id}, returnedDto);
        });

        // PUT - Update
        endpointGroup.MapPut("/{id}", (int id, UpdateGameDto updatedGame, GameStoreContext dbContext) =>
        {
            // filters games and returns index if id exists
            var existingGame = dbContext.Games.Find(id);

            if (existingGame is null)
                return Results.NotFound();

            // create new game record
            // gameDatastore[index] = new GameSummaryDto(
            //     id,
            //     updatedGame.Name,
            //     updatedGame.Genre,
            //     updatedGame.Price,
            //     updatedGame.ReleaseDate
            // );

            // update values directly in database using setValues() and UpdateGameDto 
            dbContext.Entry(existingGame).CurrentValues.SetValues(updatedGame.MapToEntity(id));

            dbContext.SaveChanges();

            return Results.NoContent();
        });

        // DELETE - Delete by ID
        endpointGroup.MapDelete("/{id}", (int id, GameStoreContext dbContext) =>
        {
            //var deleted = gameDatastore.RemoveAll(game => game.Id == id);
            dbContext.Games.Where(game => game.Id == id)
                .ExecuteDelete();

            // return deleted >= 1 ? Results.Accepted() : Results.NotFound();
            return Results.NoContent();
        });

        return endpointGroup;
    }

}
