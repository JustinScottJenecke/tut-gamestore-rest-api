using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.Entity;

public class Genre
{
    public int Id { get; set; }
    public required string Name { get; set; }
}
