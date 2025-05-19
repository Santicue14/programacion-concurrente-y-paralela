using Microsoft.EntityFrameworkCore;
using technical_tests_backend_ssr.Models;
using technical_tests_backend_ssr.Models.DTOs;
using technical_tests_backend_ssr.Repositories;
using AutoMapper;

namespace technical_tests_backend_ssr.Services;

public class CatalogoService : ICatalogoService
{
    private readonly ITipoServicioRepository _tipoServicioRepository;
    private readonly IModeloRepository _modeloRepository;
    private readonly IMarcaRepository _marcaRepository;
    private readonly IServicioPosventaRepository _servicioPosventaRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CatalogoService> _logger;

    public CatalogoService(
        ITipoServicioRepository tipoServicioRepository,
        IModeloRepository modeloRepository,
        IMarcaRepository marcaRepository,
        IServicioPosventaRepository servicioPosventaRepository,
        IMapper mapper,
        ILogger<CatalogoService> logger)
    {
        _tipoServicioRepository = tipoServicioRepository;
        _modeloRepository = modeloRepository;
        _marcaRepository = marcaRepository;
        _servicioPosventaRepository = servicioPosventaRepository;
        _mapper = mapper;
        _logger = logger;
    }

    #region Tipos de Servicio
    public async Task<IEnumerable<TipoServicioDTO>> GetTiposServicioAsync()
    {
        var tipos = await _tipoServicioRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<TipoServicioDTO>>(tipos);
    }

    public async Task<TipoServicioDTO> AddTipoServicioAsync(TipoServicioDTO tipoServicioDTO)
    {
        var tipoServicio = _mapper.Map<TipoServicio>(tipoServicioDTO);
        tipoServicio.Activo = true;
        tipoServicio.FechaCreacion = DateTime.UtcNow;

        await _tipoServicioRepository.AddAsync(tipoServicio);
        return _mapper.Map<TipoServicioDTO>(tipoServicio);
    }

    public async Task<TipoServicioDTO> UpdateTipoServicioAsync(int id, TipoServicioDTO tipoServicioDTO)
    {
        var tipoServicio = await _tipoServicioRepository.GetByIdAsync(id);
        if (tipoServicio == null)
            throw new KeyNotFoundException($"Tipo de servicio con ID {id} no encontrado");

        _mapper.Map(tipoServicioDTO, tipoServicio);
        await _tipoServicioRepository.UpdateAsync(tipoServicio);
        return _mapper.Map<TipoServicioDTO>(tipoServicio);
    }

    public async Task DeleteTipoServicioAsync(int id)
    {
        var tipoServicio = await _tipoServicioRepository.GetByIdAsync(id);
        if (tipoServicio == null)
            throw new KeyNotFoundException($"Tipo de servicio con ID {id} no encontrado");

        tipoServicio.Activo = false;
        await _tipoServicioRepository.UpdateAsync(tipoServicio);
    }
    #endregion

    #region Modelos
    public async Task<IEnumerable<ModeloDTO>> GetModelosAsync()
    {
        var modelos = await _modeloRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<ModeloDTO>>(modelos);
    }

    public async Task<ModeloDTO> AddModeloAsync(ModeloDTO modeloDTO)
    {
        var modelo = _mapper.Map<Modelo>(modeloDTO);
        await _modeloRepository.AddAsync(modelo);
        return _mapper.Map<ModeloDTO>(modelo);
    }
    #endregion

    #region Marcas
    public async Task<IEnumerable<MarcaDTO>> GetMarcasAsync()
    {
        var marcas = await _marcaRepository.GetAllAsync();
        if (marcas == null)
        {
            throw new Exception("No hay marcas disponibles");
        }

        return _mapper.Map<IEnumerable<MarcaDTO>>(marcas);
    }

    public async Task<MarcaDTO> AddMarcaAsync(MarcaDTO marcaDTO)
    {
        var marca = _mapper.Map<Marca>(marcaDTO);
        await _marcaRepository.AddAsync(marca);
        return _mapper.Map<MarcaDTO>(marca);
    }
    #endregion
} 