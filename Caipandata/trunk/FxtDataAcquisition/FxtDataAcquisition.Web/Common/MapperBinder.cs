using AutoMapper;
namespace FxtDataAcquisition.Web.Common
{
    using CAS.Entity.DBEntity;
    using FxtDataAcquisition.Domain.DTO;
    using FxtDataAcquisition.Domain.Models;

    public class MapperBinder
    {
        public static void Binder()
        {
            Mapper.CreateMap<Project, ProjectDto>();
            Mapper.CreateMap<Building, BuildingDto>();
            Mapper.CreateMap<PCompany, PCompanyDto>();
            
            Mapper.CreateMap<DATProject, Project>();
            Mapper.CreateMap<DATBuilding, Building>();
            Mapper.CreateMap<House, HouseDetails>();
            Mapper.CreateMap<HouseDetails, HouseDetailsDto>();
            Mapper.CreateMap<HouseDetailsDto, HouseDetails>();
            Mapper.CreateMap<House, HouseDto>();
            Mapper.CreateMap<SYS_Menu, MenuDto>();

            Mapper.CreateMap<Templet, TempletDto>();
            Mapper.CreateMap<FieldGroup, FieldGroupDto>();
            Mapper.CreateMap<Field, FieldDto>();

        }
    }
}