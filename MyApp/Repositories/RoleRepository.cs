using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MyApp.Data;
using MyApp.Models;

namespace MyApp.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly RoleManager<Role> roleManager;
        private readonly IMapper mapper;

        public RoleRepository(RoleManager<Role> roleManager, IMapper mapper)
        {
            this.roleManager = roleManager;
            this.mapper = mapper;
        }

        public async Task CreateRole()
        {
            var roles = new List<Role>
            {
                new Role {Name = "Admin"},
                new Role {Name = "NormalUser"}
            };
            
            foreach (var role in roles)
            {
                var savedRole = await roleManager.FindByNameAsync(role.Name);
                if (savedRole == null)
                {
                    var roleToSave = mapper.Map<Role>(role);

                    await roleManager.CreateAsync(roleToSave);
                }
            }
        }
    }
}
