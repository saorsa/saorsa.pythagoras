using AutoMapper;
using Saorsa.Pythagoras.Domain.Business.Mapping;

namespace Saorsa.Pythagoras.Domain.Business.Concrete;

public class DefaultPythagorasMapperProvider : IPythagorasMapperProvider
{
    public IMapper Mapper { get; init; }
    
    public MapperConfiguration Configuration { get; init; }

    public DefaultPythagorasMapperProvider()
    {
        Configuration = new MapperConfiguration(config =>
        {
            config.AddProfile<CategoriesMappingProfile>();
        });

        Mapper = Configuration.CreateMapper();
    }
}
