using GameStore.Api.Dto;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// in memory datastore
List<GameDto> gameDatastore = [
    new GameDto(1, "SF2", "Fighting", 199.99M, new DateOnly(1992, 7, 15)),
    new GameDto(2, "FFXIV", "RPG", 699.99M, new DateOnly(2010, 9, 30)),
    new GameDto(3, "FIFA17", "Sport", 899.99M, new DateOnly(2016, 8, 13))
];

// Base Endpoint
app.MapGet("/", () => "Hello World!");

// GET - All Games
app.MapGet("/game", () => gameDatastore);

// GET - game by id
app.MapGet("/game/{id}", (int id) => gameDatastore.Find(game => game.Id == id) );

app.Run();
