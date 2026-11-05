namespace OdontoClass.DTOs.Responses.Schedule;

public sealed record GetSchedulesResponse(DateTime DateOfSchedule, string Subject, string Procedure, string PacienteName, string PacientInfo);
