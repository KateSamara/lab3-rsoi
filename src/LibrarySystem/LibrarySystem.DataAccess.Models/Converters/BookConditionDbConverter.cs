using LibrarySystem.Domain.Models;
using LibrarySystem.Domain.Models.Books;

namespace LibrarySystem.DataAccess.Models.Converters;

public static class BookConditionDbConverter
{
    public static BookConditionDb ToDb(this BookCondition bookCondition)
    {
        return bookCondition switch
        {
            BookCondition.EXCELLENT => BookConditionDb.EXCELLENT,
            BookCondition.GOOD => BookConditionDb.GOOD,
            BookCondition.BAD => BookConditionDb.BAD,
            _ => BookConditionDb.EXCELLENT
        };
    }
    
    public static BookCondition ToDomain(this BookConditionDb bookCondition)
    {
        return bookCondition switch
        {
            BookConditionDb.EXCELLENT => BookCondition.EXCELLENT,
            BookConditionDb.GOOD => BookCondition.GOOD,
            BookConditionDb.BAD => BookCondition.BAD,
            _ => BookCondition.EXCELLENT
        };
    }
}