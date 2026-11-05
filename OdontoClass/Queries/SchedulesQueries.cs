using OdontoClass.Models;

namespace OdontoClass.Queries;

public static class SchedulesQueries
{
    public static IQueryable<Schedule> FilterByDate(this IQueryable<Schedule> query, int year, int month)
    {
        if (year < 2000 || month < 1 || month > 12)
            return query;

        return query.Where(x => x.DateOfSchedule.Year == year && x.DateOfSchedule.Month == month);
    }

}
