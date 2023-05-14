using AutoMapper;
using EfCoreDataAccessLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseEngineBusinessLayer.Interfaces;

namespace WarehouseEngineBusinessLayer.Services
{
    public class UserService : IUserService
    {
        private readonly WarehouseEngineDbContext _dbContext;

        public UserService(WarehouseEngineDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> FindUserAsync(string username, string password)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(x => x.Username == username && x.Password == password) == null ? false : true;
        }
    }
}
