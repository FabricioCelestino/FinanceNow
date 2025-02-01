using AutoMapper;
using FinanceNow.API.DTOs.TransacaoDTOs;
using FinanceNow.Modelos.Models;

namespace FinanceNow.API.Profiles
{
    public class TransacaoProfiles : Profile
    {
        public TransacaoProfiles()
        {
            CreateMap<CreateTransacaoDTO, Transacao>();
            CreateMap<UpdateTransacaoDTO, Transacao>();
            CreateMap<Transacao, ReadTransacaoDTO>().ForMember(readTransacaoDTO => readTransacaoDTO.ReadCategoriaDTO
            , opt => opt.MapFrom(transacao => transacao.Categoria));
                
        }
    }
}
