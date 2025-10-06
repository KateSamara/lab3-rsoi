using System.ComponentModel.DataAnnotations;

namespace RatingSystem.DataAccess.Models;

public class RatingDb
{
    public int Id { get; set; }
    public string Username { get; set; }
    [Range(0, 120)]
    public int Stars { get; set; }

    public RatingDb(int id, string username, int stars)
    {
        Id = id;
        Username = username;
        Stars = stars;
    }

    public RatingDb()
    {
    }
}