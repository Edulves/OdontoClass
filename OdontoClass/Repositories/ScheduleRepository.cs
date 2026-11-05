using Microsoft.EntityFrameworkCore;
using OdontoClass.Data.Context;
using OdontoClass.DTOs.Requests.Schedule;
using OdontoClass.DTOs.Responses.Schedule;
using OdontoClass.Queries;
using OdontoClass.Repositories.Interfaces;

namespace OdontoClass.Repositories;

public class ScheduleRepository(ApplicationDbContext _context) : IScheduleRepository
{
    public async Task<List<GetSchedulesResponse>> GetScheduleAsync(GetSchedulesRequest request)
    {
        return await _context.WorkSpace
            .Where(s => s.Id == request.WorkSpaceId)
            .SelectMany(x => x.Schedules)
            .FilterByDate(request.Year, request.Month)
            .Select(x => new GetSchedulesResponse(
                x.DateOfSchedule,
                x.Subject,
                x.Procedure,
                x.PacientName,
                x.PacientInfo
            ))
            .ToListAsync();
    }
}
