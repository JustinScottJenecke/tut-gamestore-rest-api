using GameStore.Api.Data;
using GameStore.Api.Dto;
using GameStore.Api.Endpoint;

var builder = WebApplication.CreateBuilder(args);

var dbConnString = "Data Source=GameStore.db";
builder.Services.AddSqlite<GameStoreContext> (dbConnString);

var app = builder.Build();

// Base Endpoint
app.MapGet("/", () => "API: \n"
    + "Method - 'Uri' - Functionality \n"
    + "---------------------------- \n"
    + "GET - '/game' - Read all games \n"
    + "GET - '/game/id' - Read game by id \n"
    + "POST - '/game' - Create new game \n"
    + "PUT - '/game/id' - Update game by id \n"
    + "DELETE - '/game/id' - Delete game by id \n"
);

app.MapGamesEndpoints();

app.Run();
