using AutoMapper;
using FinPlanner360.Api.ViewModels.Budget;
using FinPlanner360.Api.ViewModels.Category;
using FinPlanner360.Business.Models;

namespace FinPlanner360.Api.Configuration;

public class AutomapperConfig : Profile
{
    public AutomapperConfig()
    {
        CreateMap<Category, CategoryViewModel>().ReverseMap();
        
        CreateMap<BudgetViewModel,Budget>();
        CreateMap<Budget, BudgetViewModel>()
            .ForMember(dest => dest.Description, opt => opt.MapFrom(source => source.Category.Description));
    }
}