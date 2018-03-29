using AutoMapper;
using CAS.Entity.DBEntity;
using FxtDataAcquisition.Domain.DTO;
using FxtDataAcquisition.Domain.Models;

namespace FxtDataAcquisition.Web.Common
{
    public class MapperBinder
    {
        public static void Binder()
        {
            Mapper.CreateMap<DATProject, Project>();
            Mapper.CreateMap<House, HouseDetails>();
            Mapper.CreateMap<HouseDetails, HouseDetailsDto>();
            Mapper.CreateMap<HouseDetailsDto, HouseDetails>();
            Mapper.CreateMap<House, HouseDto>();
            Mapper.CreateMap<SYS_Menu, MenuDto>();
        }
    }
}