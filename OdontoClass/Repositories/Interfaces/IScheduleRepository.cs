using OdontoClass.DTOs.Requests.Schedule;
using OdontoClass.DTOs.Responses.Schedule;

namespace OdontoClass.Repositories.Interfaces;

public interface IScheduleRepository
{
    Task<List<GetSchedulesResponse>> GetScheduleAsync(GetSchedulesRequest request);
}
