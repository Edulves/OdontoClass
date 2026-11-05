using OdontoClass.Data.ResultPattern.Base;
using OdontoClass.DTOs.Requests.Schedule;
using OdontoClass.DTOs.Responses.Schedule;

namespace OdontoClass.Services.Interfaces;

public interface IWorkSpaceService
{
    Task<ResultPattern<List<GetWorkSpacesResponse>>> GetWorkSpacesAsync(GetWorkSpacesRequest request);
    Task<ResultPattern<string>> PostWorkSpacesAsync(List<PostWorkSpaceRequest> request);
}
