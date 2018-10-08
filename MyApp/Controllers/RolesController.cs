using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyApp.Dtos;
using MyApp.Repositories;

namespace MyApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class RolesController : ControllerBase
    {
        private readonly IRoleRepository roleRepo;

        public RolesController(IRoleRepository roleRepo)
        {
            this.roleRepo = roleRepo;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create()
        {
            try
            {
                await roleRepo.CreateRole();
                return Ok("Role has been added");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
