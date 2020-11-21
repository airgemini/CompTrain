using CompTrain.Server.Data;
using CompTrain.Server.Interfaces;
using CompTrain.Shared.Models.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompTrain.Server.Services
{
    public class RestDayService : IRestDayService
    {
        private readonly ILogger<RestDayService> _logger;
        private readonly ApplicationDbContext _context;

        public RestDayService(ILogger<RestDayService> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<bool> IsRestDay(DateTime dateTime)
        {
            try
            {
                IList<RestDay> restDays = await GetRestdays();
                foreach (var restday in restDays)
                {
                    if (restday.DayOfWeek.HasValue)
                    {
                        if (dateTime.DayOfWeek == restday.DayOfWeek)
                            return true;
                    }
                    else
                    {
                        DateTime dateCheck = new DateTime(
                                restday.Year.HasValue ? restday.Year.Value : dateTime.Year,
                                restday.Month.HasValue ? restday.Month.Value : dateTime.Month,
                                restday.Day.HasValue ? restday.Day.Value : dateTime.Day
                            );

                        if (dateCheck == dateTime)
                            return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"IsRestDay() {ex.Message}");
                return false;
            }
        }

        public async Task<IList<RestDay>> GetRestdays()
        {
            try
            {
                return await _context.RestDays.OrderBy(x => x.DayOfWeek).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetRestdays() {ex.Message}");
                return null;
            }
        }
    }
}
