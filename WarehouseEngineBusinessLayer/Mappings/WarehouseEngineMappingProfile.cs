using AutoMapper;
using EfCoreDataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseEngineBusinessLayer.ViewModels;

namespace WarehouseEngineBusinessLayer.Mappings
{
    public class WarehouseEngineMappingProfile : Profile
    {
        public WarehouseEngineMappingProfile()
        {
            CreateMap<Detail, DetailViewModel>().ReverseMap();
            CreateMap<Supplier, SupplierViewModel>().ReverseMap();
        }
    }
}
