using GameStore.Api.Dto;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// constant name for get single game
const string GET_GAME_ENDPOINT_NAME = "GetGameById";

// in memory datastore
List<GameDto> gameDatastore = [
    new GameDto(1, "SF2", "Fighting", 199.99M, new DateOnly(1992, 7, 15)),
    new GameDto(2, "FFXIV", "RPG", 699.99M, new DateOnly(2010, 9, 30)),
    new GameDto(3, "FIFA17", "Sport", 899.99M, new DateOnly(2016, 8, 13))
];

// Base Endpoint
app.MapGet("/", () => "API: \n"
    + "Method - 'Uri' - Functionality \n"
    + "---------------------------- \n"
    + "GET - '/game' - Read all games \n"
    + "GET - '/game/id' - Read game by id \n"
    + "POST - '/game' - Create new game \n"
    + "PUT - '/game/id' - Update game by id \n"
);

// GET - Read Games
app.MapGet("/game", () => gameDatastore);

// GET - Read by id
app.MapGet("/game/{id}", (int id) => gameDatastore.Find(game => game.Id == id) )
    .WithName(GET_GAME_ENDPOINT_NAME);

// POST - Create
app.MapPost("/game", ( CreateGameDto newGame ) => {

    GameDto game = new (
        gameDatastore.Count + 1,
        newGame.Name,
        newGame.Genre,
        newGame.Price,
        newGame.ReleaseDate
    );

    gameDatastore.Add(game);

    return Results.CreatedAtRoute(GET_GAME_ENDPOINT_NAME, new {id = game.Id}, game);
});

// PUT - Update
app.MapPut("/game/{id}", (int id, UpdateGameDto updatedGame) => {
    
    // filters games and returns index if id exists
    var index = gameDatastore.FindIndex(game => game.Id == id);

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

app.Run();
