﻿using BusinessObject.DTO.Request;
using BusinessObject.DTO.Response;

namespace BusinessObject.Manager.Interfaces;

public interface IMealManager
{
    Task<List<ServingDTO>> GetPersonalMonthlyStats(int uid);
    Task<List<ServingDTO>> GetCompanyDailyStats(DateTime bookedDate);
    Task<List<CompanyMonthlyStatsDTO>> GetCompanyMonthlyStats(DateTime requestDate);
    Task<List<ServingDTO>> FindExistingRegistration(int depID);
    Task<bool> RegisterPersonalMeal(FormDTO request);
    Task<bool> RegisterDepartmentMeal(FormDTO request);
}
