using System.Text.RegularExpressions;
using GameStore.Api.Data;
using GameStore.Api.Mapping;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Endpoint;

public static class GenreEndpoints
{
    public static RouteGroupBuilder MapGenreEndpoints(this WebApplication webApplication) 
    {
        var group = webApplication.MapGroup("/genre");

        group.MapGet("/", async (GameStoreContext gameStoreDbContext) => {
            return await gameStoreDbContext.Genres.Select(genre => genre.MapToDto()).AsNoTracking().ToListAsync();
        });

        return group;
    }
}
