using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompTrain.Server.Data;
using CompTrain.Shared.Models.Data;
using CompTrain.Shared.Models.Wod;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CompTrain.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using CompTrain.Server.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace CompTrain.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WodsController : ControllerBase
    {
        private readonly ILogger<WodsController> _logger;
        private readonly IWorkouttypeService _workouttypesService;
        private readonly IResulttypeService _resulttypeService;
        private readonly IWodService _wodService;
        private readonly IRestDayService _restdayService;
        public WodsController(ILogger<WodsController> logger,
                                IWorkouttypeService workouttypesService,
                                IResulttypeService resulttypeService,
                                IWodService wodService,
                                IRestDayService restdayService)
        {
            _logger = logger;
            _workouttypesService = workouttypesService;
            _resulttypeService = resulttypeService;
            _wodService = wodService;
            _restdayService = restdayService;
        }

        [HttpGet("[action]/{date}")]
        [Authorize(Roles = "Admin,Owner")]
        public async Task<IActionResult> Add(string date = null)
        {
            try
            {
                DateTime Date;
                if (String.IsNullOrWhiteSpace(date) || !DateTime.TryParse(date, out Date) || Date == DateTime.MinValue)
                    Date = DateTime.Now;

                WodRequest request = new WodRequest()
                {
                    Date = Date,
                    Name = $"WOD-{Date.ToString("yyyyMMdd")}",
                    Workouttypes = await _workouttypesService.GetWorkouttypes(),
                    Resulttypes = await _resulttypeService.GetResulttypes()
                };

                Wod wod = await _wodService.GetWodByDate(Date);
                if (wod == null)
                {
                    foreach (var workouttype in request.Workouttypes.Where(x => x.IsAutomatic == true))
                    {
                        if (request.WorkoutRequests == null)
                            request.WorkoutRequests = new List<WorkoutRequest>();

                        request.WorkoutRequests.Add(new WorkoutRequest
                        {
                            Workouttype = workouttype
                        });
                    }
                } else
                {
                    request.Name = wod.Name;
                    request.Description = wod.Description;
                    request.Wod = wod;
                    foreach (var workout in wod.Workouts)
                    {
                        if (request.WorkoutRequests == null)
                            request.WorkoutRequests = new List<WorkoutRequest>();

                        request.WorkoutRequests.Add(new WorkoutRequest
                        {
                            Workouttype = request.Workouttypes.Where(x => x.Id == workout.WorkouttypeId).FirstOrDefault(),
                            Description = workout.Description,
                            ResulttypeId = workout.ResulttypeId

                        });
                    }
                }
                return Ok(request);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Get error: {ex.Message}");
                return BadRequest();
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Owner")]
        public async Task<IActionResult> Post([FromBody]WodRequest request)
        {
            try
            {
                Wod wod = new Wod()
                {
                    Date = request.Date.Value,
                    Name = request.Name,
                    Description = request.Description,
                    Workouts = request.WorkoutRequests.Select(x => new Workout()
                    {
                        Description = x.Description,
                        ResulttypeId = x.ResulttypeId,
                        WorkouttypeId = x.Workouttype.Id

                    }).ToList()
                };

                if (request.Wod != null)
                {
                    await _wodService.Delete(request.Wod);
                }

                wod = await _wodService.Add(wod);
                request.IsSaved = wod != null;

                return Ok(request);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Post error: {ex.Message} - Name: {request.Name}");
                return BadRequest();
            }
        }


        [HttpGet("[action]/{date}")]
        public async Task<IActionResult> Week(string date)
        {
            DateTime currentDate;

            if (String.IsNullOrEmpty(date) || !DateTime.TryParse(date, out currentDate))
                currentDate = DateTime.Now;

            currentDate = currentDate.StartOfWeek(DayOfWeek.Monday);

            try
            {
                IList<Wod> wods = await _wodService.GetWodsByDate(currentDate, currentDate.AddDays(7));


                List<ShowResponse> showResponses = new List<ShowResponse>();
                for(int i=0; i<7; i++)
                {
                    showResponses.Add(new ShowResponse()
                        {
                            Date = currentDate.AddDays(i),
                            IsRest = await _restdayService.IsRestDay(currentDate.AddDays(i)),
                            Wod = wods.FirstOrDefault(x=>x.Date == currentDate.AddDays(i))
                        }
                    );
                }

                return Ok(showResponses);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Week error: {ex.Message} - Date: {currentDate.ToLongDateString()}");
                return BadRequest();
            }
        }

        [HttpGet("[action]/{datestart}")]
        [Authorize(Roles = "Admin,Owner")]
        public async Task<IActionResult> Agenda(string datestart = null)
        {
            DateTime DateStart;
            if (String.IsNullOrWhiteSpace(datestart) || !DateTime.TryParse(datestart, out DateStart) || DateStart == DateTime.MinValue)
                DateStart = DateTime.Now;

            try
            {
                List<AgendaResponse> agendaResponses = new List<AgendaResponse>();
                for (int i = 0; i <= 7; i++)
                {
                    agendaResponses.Add(new AgendaResponse()
                    {
                        Date = DateStart.AddDays(i),
                        Wod = await _wodService.GetWodByDate(DateStart.AddDays(i)),
                        IsRest = await _restdayService.IsRestDay(DateStart.AddDays(i))
                    });
                }

                agendaResponses = agendaResponses.OrderBy(x => x.Date).ToList();
                return Ok(agendaResponses);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Agenda error: {ex.Message} - Date: {DateStart.ToLongDateString()}");
                return BadRequest();
            }
        }

        [HttpGet("[action]/{WodId}")]
        [Authorize(Roles = "Admin,Owner")]
        public async Task<IActionResult> Delete(int WodId)
        {
            try
            {
                Wod wod = await _wodService.Get(WodId);
                return Ok(await _wodService.Delete(wod));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Delete error: {ex.Message} - WodId: {WodId}");
                return BadRequest();
            }
        }
    }
}
