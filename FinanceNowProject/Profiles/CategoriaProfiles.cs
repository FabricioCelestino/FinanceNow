using AutoMapper;
using FinanceNow.API.DTOs.CategoriaDTOs;
using FinanceNow.Modelos.Models;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace FinanceNow.API.Profiles;

internal class CategoriaProfiles : Profile
{
    public CategoriaProfiles()
    {
        CreateMap<Categoria, ReadCategoriaDto>();
        CreateMap<CreateCategoriaDto, Categoria>();
        CreateMap<UpdateCategoriaDto, Categoria>();
    }
}