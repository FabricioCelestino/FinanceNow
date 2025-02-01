using AutoMapper;
using FinanceNow.API.DTOs.CategoriaDTOs;
using FinanceNow.Modelos.Models;

namespace FinanceNow.API.Profiles
{
    public class CategoriaProfiles : Profile
    {
        public CategoriaProfiles()
        {
            CreateMap<Categoria, ReadCategoriaDTO>();
            CreateMap<CreateCategoriaDTO, Categoria>();
            CreateMap<UpdateCategoriaDTO, Categoria>();
        }
    }
}
