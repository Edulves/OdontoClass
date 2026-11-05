using OdontoClass.Models;

namespace OdontoClass.Queries;

public static class WorkSpaceUserQueries
{
    public static IQueryable<WorkSpaceUser> FilterByUserId(this IQueryable<WorkSpaceUser> query, int userId)
    {
        if (userId < 1)
            return query;

        return query.Where(w => w.UserId == userId);
    }
}