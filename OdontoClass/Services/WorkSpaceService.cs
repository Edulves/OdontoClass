using OdontoClass.Data.ResultPattern.Base;
using OdontoClass.DTOs.Requests.Schedule;
using OdontoClass.DTOs.Responses.Schedule;
using OdontoClass.Models;
using OdontoClass.Repositories.Interfaces;
using OdontoClass.Services.Interfaces;

namespace OdontoClass.Services;

public class WorkSpaceService(IGenericOperationsRepositosy _genericRepo, IWorkSpaceRepository _repository,ICurrentUserService _currentUser) : IWorkSpaceService
{
    public async Task<ResultPattern<string>> PostWorkSpacesAsync(List<PostWorkSpaceRequest> request)
    {
        if (request.Count < 1)
            return ResultPattern<string>.Failure("É obrigatório pelo menos um item.");

        var canGetUser = _currentUser.GetCurrentUser();
        if (!canGetUser.IsSuccess)
            return ResultPattern<string>.Failure(canGetUser.Detail);

        var newWorkSpaces = new List<WorkSpace>();

        foreach (var item in request)
        {
            var newWorkSpace = new WorkSpace
            {
                Name = item.WorkSpaceName,
                CreatedByUserId = canGetUser.Value,
            };

            newWorkSpace.WorkSpaceMembers.Add(new WorkSpaceUser
            {
                UserId = canGetUser.Value,
            });

            newWorkSpaces.Add(newWorkSpace);
        }

        await _genericRepo.CreateAsync(newWorkSpaces);

        return ResultPattern<string>.Success("WorkSpaces criados!");
    }

    public async Task<ResultPattern<List<GetWorkSpacesResponse>>> GetWorkSpacesAsync(GetWorkSpacesRequest request)
    {
        var result = await _repository.GetWorkSpacesAsync(request);

        if (result.Count < 1)
            return ResultPattern<List<GetWorkSpacesResponse>>.Failure("Nenhum workspace cadastrado!", "Nenhum workspace cadastrado!", StatusCodes.Status404NotFound);

        return ResultPattern<List<GetWorkSpacesResponse>>.Success(result);
    }
}
