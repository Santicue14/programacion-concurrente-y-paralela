using AutoMapper;
using technical_tests_backend_ssr.Models;
using technical_tests_backend_ssr.Models.DTOs;

/// <summary>
/// Clase de perfil de AutoMapper.
/// </summary>
public class AutoMapperProfile : Profile
{
    /// <summary>
    /// Constructor de la clase AutoMapperProfile.
    /// </summary>
    public AutoMapperProfile()
    {
        // Creación de map para cliente y clienteDTO
        CreateMap<Cliente, ClienteDTO>().ReverseMap();
        CreateMap<ClienteDTO, Cliente>().ForMember(dest => dest.Id, opt => opt.Ignore());

        // Creación de map para vehículo y vehiculoDTO
        CreateMap<Vehiculo, VehiculoDTO>().ReverseMap();
        CreateMap<VehiculoDTO, Vehiculo>().ForMember(dest => dest.Id, opt => opt.Ignore());

        // Creación de map para venta y ventaDTO
        CreateMap<Venta, VentaDTO>().ReverseMap();
        CreateMap<VentaDTO, Venta>().ForMember(dest => dest.Id, opt => opt.Ignore());

        // Marcas dentro de catalogo
        CreateMap<Marca, MarcaDTO>().ReverseMap();
        CreateMap<MarcaDTO, Marca>().ForMember(dest => dest.Id, opt => opt.Ignore());

        // Tipos de Servicio
        CreateMap<TipoServicio, TipoServicioDTO>().ReverseMap();
        CreateMap<TipoServicioDTO, TipoServicio>().ForMember(dest => dest.Id, opt => opt.Ignore());

        // Modelos
        CreateMap<Modelo, ModeloDTO>().ReverseMap();
        CreateMap<ModeloDTO, Modelo>().ForMember(dest => dest.Id, opt => opt.Ignore());
    }
}

