using GameStore.Api.Data;
using GameStore.Api.Endpoint;

var builder = WebApplication.CreateBuilder(args);

var dbConnString = builder.Configuration.GetConnectionString("GameStoreDbConn");
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

// create mappings for Game endpoints
app.MapGamesEndpoints();

// spin up application
app.Run();