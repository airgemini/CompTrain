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
    public class WodService : IWodService
    {
        private readonly ILogger<WodService> _logger;
        private readonly ApplicationDbContext _context;

        public WodService(ILogger<WodService> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IList<Wod>> GetWodsByDate(DateTime Start, DateTime End)
        {
            try
            {
                return await _context.Wods
                    .AsNoTracking()
                    .Where(x => x.Date >= Start && x.Date <= End)
                    .OrderBy(x => x.Date)
                    .Include(x => x.Workouts)
                    .ThenInclude(i => i.Workouttype)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetWodsByDate() {ex.Message}");
                return null;
            }
        }

        public async Task<Wod> GetWodByDate(DateTime Date)
        {
            try
            {
                return await _context.Wods
                    .AsNoTracking()
                    .Where(x => x.Date == Date)
                    .Include(x => x.Workouts)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetWodByDate() {ex.Message}");
                return null;
            }
        }

        public async Task<Wod> Add(Wod wod)
        {
            try
            {
                await _context.Wods.AddAsync(wod);
                await _context.SaveChangesAsync();
                return wod;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Add() {ex.Message}");
                return null;
            }
        }

        public async Task<bool> Delete(Wod wod)
        {
            try
            {
                if (wod == null)
                    return false;

                _context.Wods.Remove(wod);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Delete() {ex.Message}");
                return false;
            }
        }

        public async Task<Wod> Get(int WodId)
        {
            try
            {
                return await _context.Wods
                    .AsNoTracking()
                    .Where(x => x.Id == WodId)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Get() {ex.Message} - WodId: {WodId}");
                return null;
            }
        }

    }
}
