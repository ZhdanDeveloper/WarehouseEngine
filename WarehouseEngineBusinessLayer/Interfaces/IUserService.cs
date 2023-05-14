using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseEngineBusinessLayer.Interfaces
{
    public interface IUserService
    {
        public Task<bool> FindUserAsync(string username, string password);
    }
}
