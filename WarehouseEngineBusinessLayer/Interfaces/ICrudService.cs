using EfCoreDataAccessLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseEngineBusinessLayer.Interfaces
{
    public interface ICrudService<TEntity, TViewModel>  where TEntity : class, IDbEntity
    {
        Task<TViewModel> GetByIdAsync(int id);
        Task<IEnumerable<TViewModel>> GetAllAsync();
        Task CreateAsync(TViewModel entity);
        Task UpdateAsync(int id, TViewModel viewModel);
        Task DeleteAsync(int id);
    }
}
