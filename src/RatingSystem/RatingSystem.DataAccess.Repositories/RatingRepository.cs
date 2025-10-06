using Microsoft.EntityFrameworkCore;
using RatingSystem.DataAccess.Context;
using RatingSystem.DataAccess.Models.Converters;
using RatingSystem.Domain.Exceptions.Repositories;
using RatingSystem.Domain.Interfaces.Repositories;
using RatingSystem.Domain.Models;

namespace RatingSystem.DataAccess.Repositories;

public class RatingRepository(RatingSystemContext context) : IRatingRepository
{
    private readonly RatingSystemContext _context = context ?? throw new ArgumentNullException(nameof(context));

    public async Task<Rating> GetRatingByUsernameAsync(string username)
    {
        try
        {
            var ratingDb = await _context.Ratings
                .Where(r => r.Username == username)
                .FirstAsync();
            
            return ratingDb.ToDomain();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new RatingRepositoryException($"There was an error while getting the rating by username = {username}.", e);
        }
    }

    public async Task<int> GetRatingCountAsync()
    {
        try
        {
            return await _context.Ratings.CountAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new RatingRepositoryException("There was an error while getting the rating count.", e);
        }
    }

    public async Task AddRatingAsync(Rating rating)
    {
        try
        {
            int id;
            if (await _context.Ratings.CountAsync() == 0)
                id = 1;
            else
                id = _context.Ratings.Max(r => r.Id) + 1;
            
            var ratingDb = rating.ToDb(id);
            await _context.Ratings.AddAsync(ratingDb);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new RatingRepositoryException("There was an error while adding the rating.", e);
        }
    }

    public async Task UpdateRatingAsync(string username, int starDifference)
    {
        try
        {
            var rating = await _context.Ratings
                .Where(r => r.Username == username)
                .FirstAsync();
            
            rating.Stars += starDifference;
            
            if (rating.Stars < 0)
                rating.Stars = 0;
            if (rating.Stars > 100)
                rating.Stars = 100;
            
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new RatingRepositoryException($"There was an error while updating the rating for user = {username}.", e);
        }
    }
}