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
    public class ResulttypeService : IResulttypeService
    {
        private readonly ILogger<ResulttypeService> _logger;
        private readonly ApplicationDbContext _context;

        public ResulttypeService(ILogger<ResulttypeService> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IList<Resulttype>> GetResulttypes()
        {
            try
            {
                return await _context.Resulttypes.OrderBy(x => x.Name).OrderBy(x => x.OrderNum).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetResulttypes() {ex.Message}");
                return null;
            }
        }
    }
}
