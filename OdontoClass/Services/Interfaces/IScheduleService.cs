using OdontoClass.Data.ResultPattern.Base;
using OdontoClass.DTOs.Requests.Schedule;
using OdontoClass.DTOs.Responses.Schedule;

namespace OdontoClass.Services.Interfaces;

public interface IScheduleService
{
    Task<ResultPattern<List<GetSchedulesResponse>>> GetSchedulesAsync(GetSchedulesRequest request);
    Task<ResultPattern<string>> PostSchedulesAsync(List<PostSchedulesRequest> request);
}
