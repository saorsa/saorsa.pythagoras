using AutoMapper;

namespace Saorsa.Pythagoras.Domain.Business;

public interface IPythagorasMapperProvider
{
    IMapper Mapper { get; }
}
