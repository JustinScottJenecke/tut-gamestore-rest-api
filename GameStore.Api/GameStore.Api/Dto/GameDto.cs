namespace GameStore.Api.Dto;

public record class GameDto( 
    int Id, 
    string Name, 
    string Genere, 
    decimal Price,
    DateOnly ReleaseDate
);
