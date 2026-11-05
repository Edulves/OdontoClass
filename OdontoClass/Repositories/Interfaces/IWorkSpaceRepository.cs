using OdontoClass.DTOs.Requests.Schedule;
using OdontoClass.DTOs.Responses.Schedule;
using OdontoClass.Models;

namespace OdontoClass.Repositories.Interfaces;

public interface IWorkSpaceRepository
{
    Task<List<GetWorkSpacesResponse>> GetWorkSpacesAsync(GetWorkSpacesRequest request);
    Task<WorkSpace?> GetWorkSpaceByIdAsync(int id);
}
