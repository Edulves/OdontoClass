using Microsoft.EntityFrameworkCore;
using OdontoClass.Data.Context;
using OdontoClass.DTOs.Requests.Schedule;
using OdontoClass.DTOs.Responses.Schedule;
using OdontoClass.Models;
using OdontoClass.Queries;
using OdontoClass.Repositories.Interfaces;

namespace OdontoClass.Repositories;

public class WorkSpaceRepository(ApplicationDbContext _context) : IWorkSpaceRepository
{
    public async Task<List<GetWorkSpacesResponse>> GetWorkSpacesAsync(GetWorkSpacesRequest request)
    {
        return await _context.WorkSpaceUser
            .FilterByUserId(request.UserId)
            .Select(x =>
                new GetWorkSpacesResponse
                    (x.WorkSpace.Id, x.WorkSpace.Name, x.WorkSpace.CreatedByUser.UserName ?? ""))
            .ToListAsync();
    }

    public async Task<WorkSpace?> GetWorkSpaceByIdAsync(int id)
    {
        return await _context.WorkSpace.FindAsync(id);
    }
}
