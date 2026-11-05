using OdontoClass.Data.ResultPattern.Base;
using OdontoClass.DTOs.Requests.Schedule;
using OdontoClass.DTOs.Responses.Schedule;
using OdontoClass.Models;
using OdontoClass.Repositories.Interfaces;
using OdontoClass.Services.Interfaces;

namespace OdontoClass.Services;


public class ScheduleService(IScheduleRepository _repository, IWorkSpaceRepository _workSpaceRepo, IGenericOperationsRepositosy _genericRepo, ICurrentUserService _currentUser) : IScheduleService
{
    public async Task<ResultPattern<List<GetSchedulesResponse>>> GetSchedulesAsync(GetSchedulesRequest request)
    {
        var result = await _repository.GetScheduleAsync(request);

        return ResultPattern<List<GetSchedulesResponse>>.Success(result);
    }

    public async Task<ResultPattern<string>> PostSchedulesAsync(List<PostSchedulesRequest> request)
    {
        var canGetUser = _currentUser.GetCurrentUser();
        if (!canGetUser.IsSuccess)
            return ResultPattern<string>.Failure(canGetUser.Detail);

        var newSchedules = new List<Schedule>();

        var workSpaceExist = await _workSpaceRepo.GetWorkSpaceByIdAsync(request.FirstOrDefault()!.WorkSpaceId);

        if (workSpaceExist is null)
            return ResultPattern<string>.Failure("WorkSpace não existe.", "WorkSpace não encontrado", StatusCodes.Status404NotFound);

        foreach(var schedule in request)
        {
            var newSchedule = new Schedule
            { 
                DateOfSchedule = schedule.DateOfSchedule,
                PacientName = schedule.PacienteName,
                PacientInfo = schedule.PacientInfo,
                Subject = schedule.Subject,
                Procedure = schedule.Procedure,
                CreatedByUserId = canGetUser.Value,
                WorkSpaceId = schedule.WorkSpaceId,
            };

            newSchedules.Add(newSchedule);
        }

        var result = await _genericRepo.CreateAsync(newSchedules);

        return ResultPattern<string>.Success("Agendamentos criados!");
    }
}
