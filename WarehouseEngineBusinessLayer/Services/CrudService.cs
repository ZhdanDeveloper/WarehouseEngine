using AutoMapper;
using EfCoreDataAccessLayer;
using EfCoreDataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseEngineBusinessLayer.Interfaces;

namespace WarehouseEngineBusinessLayer.Services
{
    public class CrudService<TEntity,TViewModel> : ICrudService<TEntity, TViewModel> where TEntity : class, IDbEntity 
    {
        private readonly WarehouseEngineDbContext _dbContext;
        private readonly IMapper _mapper;

        public CrudService(WarehouseEngineDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TViewModel>> GetAllAsync()
        {
            var entities = await _dbContext.Set<TEntity>().ToListAsync();
            var viewModelList = _mapper.Map<IEnumerable<TEntity>, IEnumerable<TViewModel>>(entities);
            return viewModelList;
        }

        public async Task<TViewModel> GetByIdAsync(int id)
        {
            var entity = await _dbContext.Set<TEntity>().FirstOrDefaultAsync(e => e.Id == id);
            var viewModel = _mapper.Map<TEntity, TViewModel>(entity);
            return viewModel;
        }

        public async Task CreateAsync(TViewModel viewModel)
        {
            var entity = _mapper.Map<TViewModel, TEntity>(viewModel);
            await _dbContext.Set<TEntity>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, TViewModel viewModel)
        {
            var entity = await _dbContext.Set<TEntity>().FirstOrDefaultAsync(e => e.Id == id);
            _mapper.Map(viewModel, entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _dbContext.Set<TEntity>().FirstOrDefaultAsync(e => e.Id == id);
            _dbContext.Set<TEntity>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
