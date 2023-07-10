using BusinessObject.DTO.Request;

namespace BusinessObject.Manager.Interfaces;

public interface IMealManager
{
    Task<List<ServingDTO>> GetPersonalMonthlyStats(int uid);
    Task<List<ServingDTO>> GetDepartmentDailyStats(DateTime bookedDate);
}
