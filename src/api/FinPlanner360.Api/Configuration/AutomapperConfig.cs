using AutoMapper;
using FinPlanner360.Api.ViewModels.Category;
using FinPlanner360.Busines.Models;

namespace FinPlanner360.Api.Configuration;

public class AutomapperConfig : Profile
{
    public AutomapperConfig()
    {
        CreateMap<Category, CategoryViewModel>().ReverseMap();
    }
}