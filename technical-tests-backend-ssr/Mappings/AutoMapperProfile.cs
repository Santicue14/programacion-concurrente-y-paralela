using AutoMapper;
using technical_tests_backend_ssr.Models;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // Creación de map para cliente y clienteDTO
        CreateMap<Cliente, ClienteDTO>().ReverseMap();
        CreateMap<ClienteDTO, Cliente>().ForMember(dest => dest.Id, opt => opt.Ignore());

        // Creación de map para vehículo y vehiculoDTO
        CreateMap<Vehiculo, VehiculoDTO>().ReverseMap();
        CreateMap<VehiculoDTO, Vehiculo>().ForMember(dest => dest.Id, opt => opt.Ignore());
    }
}

