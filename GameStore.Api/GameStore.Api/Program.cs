using GameStore.Api.Dto;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// in memory datastore
List<GameDto> gameDatastore = [
    new GameDto(1, "SF2", "Fighting", 199.99M, new DateOnly(1992, 7, 15)),
    new GameDto(1, "FFXIV", "RPG", 699.99M, new DateOnly(2010, 9, 30)),
    new GameDto(1, "FIFA17", "Sport", 899.99M, new DateOnly(2016, 8, 13))
];


app.MapGet("/", () => "Hello World!");

app.MapGet("/game", () => gameDatastore);

app.Run();
