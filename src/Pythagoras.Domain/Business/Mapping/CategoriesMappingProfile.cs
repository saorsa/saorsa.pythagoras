using AutoMapper;
using Saorsa.Pythagoras.Domain.Model;
using Saorsa.Pythagoras.Domain.Model.Categories;
using Saorsa.Pythagoras.Persistence.Model;

namespace Saorsa.Pythagoras.Domain.Business.Mapping;

public class CategoriesMappingProfile : Profile
{
    public CategoriesMappingProfile()
    {
        CreateMap<Category, CategoryListItem>()
            .ForMember(
                m => m.Permissions, 
                m => m.MapFrom((source) =>
                    new Acl(source.User, source.Group, source.UserMask, source.GroupMask, source.OtherMask)
                ));
    }
}
