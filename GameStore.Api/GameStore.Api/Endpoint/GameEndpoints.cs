﻿using GameStore.Api.Dto;

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
        endpointGroup.MapPost("/", (CreateGameDto newGame) =>
        {
            GameDto game = new(
                gameDatastore.Count + 1,
                newGame.Name,
                newGame.Genre,
                newGame.Price,
                newGame.ReleaseDate
            );

            gameDatastore.Add(game);

            return Results.CreatedAtRoute(GET_GAME_ENDPOINT_NAME, new { id = game.Id }, game);

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