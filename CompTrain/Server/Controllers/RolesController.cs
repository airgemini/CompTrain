using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompTrain.Shared.Models.Data;
using CompTrain.Shared.Models.Role;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CompTrain.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class RolesController : ControllerBase
    {
        private readonly ILogger<RolesController> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<Athlete> _userManager;

        public RolesController(ILogger<RolesController> logger, RoleManager<IdentityRole> roleManager, UserManager<Athlete> userManager)
        {
            _logger = logger;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                List<RoleModel> roleResponses = _roleManager.Roles.Select(x => new RoleModel()
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList();

                return Ok(roleResponses);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Get error: {ex.Message}");
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]EditRoleRequest request)
        {
            EditRoleResponse response = new EditRoleResponse();
            try
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = request.Name
                };

                IdentityResult result = await _roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    identityRole = await _roleManager.FindByNameAsync(identityRole.Name);
                    response.Id = identityRole.Id;
                    response.IsSuccess = true;
                }
                else
                {
                    response.Errors = result.Errors.Select(x => x.Description);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Post error: {ex.Message} - Name: {request.Name}");
                return BadRequest();
            }
        }


        [HttpPut]
        public async Task<IActionResult> Put([FromBody]EditRoleRequest request)
        {
            EditRoleResponse response = new EditRoleResponse();
            try
            {
                IdentityRole identityRole = await _roleManager.FindByIdAsync(request.Id);
                if (identityRole == null)
                    throw new Exception("Role not found");

                identityRole.Name = request.Name;
                IdentityResult result = await _roleManager.UpdateAsync(identityRole);

                if (result.Succeeded)
                {
                    response.Id = identityRole.Id;
                    response.IsSuccess = true;
                }
                else
                {
                    response.Errors = result.Errors.Select(x => x.Description);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Post error: {ex.Message} - Name: {request.Name}");
                return BadRequest();
            }
        }

        [HttpGet("{id}", Name = "Get")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                IdentityRole identityRole = await _roleManager.FindByIdAsync(id);
                if (identityRole == null)
                    throw new Exception("Role not found");

                List<RoleUserResponse> roleUserResponses = new List<RoleUserResponse>();
                foreach (var user in await _userManager.Users.ToListAsync())
                {
                    var roleUserResponse = new RoleUserResponse
                    {
                        Id = user.Id,
                        Name = $"{user.FirstName} {user.LastName}",
                        OnRule = await _userManager.IsInRoleAsync(user, identityRole.Name)
                    };
                    roleUserResponses.Add(roleUserResponse);
                }

                return Ok(roleUserResponses);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Get error: {ex.Message} - Id: {id}");
                return BadRequest();
            }
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> AddRemove([FromBody]AddRemoveRoleRequest addRemoveRoleRequest)
        {
            try
            {
                IdentityRole role = await _roleManager.FindByIdAsync(addRemoveRoleRequest.RoleId);
                if (role == null)
                    throw new Exception("Role not found");

                var user = await _userManager.FindByIdAsync(addRemoveRoleRequest.UserId);

                if (user == null)
                    throw new Exception("User not found");

                if (addRemoveRoleRequest.Add)
                {
                    await _userManager.AddToRoleAsync(user, role.Name);
                } else
                {
                    await _userManager.RemoveFromRoleAsync(user, role.Name);
                }

                return Ok(true);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Get error: {ex.Message} - UserId: {addRemoveRoleRequest.UserId} - RoleId: {addRemoveRoleRequest.RoleId}");
                return BadRequest();
            }
        }
    }
}
