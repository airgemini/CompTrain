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
    public class WorkouttypeService : IWorkouttypeService
    {
        private readonly ILogger<WorkouttypeService> _logger;
        private readonly ApplicationDbContext _context;

        public WorkouttypeService(ILogger<WorkouttypeService> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IList<Workouttype>> GetWorkouttypes()
        {
            try
            {
                return await _context.Workouttypes.OrderBy(x => x.OrderNum).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetWorkouttypes() {ex.Message}");
                return null;
            }
        }
    }
}
