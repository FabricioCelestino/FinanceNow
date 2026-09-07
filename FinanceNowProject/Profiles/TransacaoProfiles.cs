using AutoMapper;
using FinanceNow.API.DTOs.TransacaoDTOs;
using FinanceNow.Modelos.Models;

namespace FinanceNow.API.Profiles;

/// <summary>
/// 
/// </summary>
public class TransacaoProfiles : Profile
{
    /// <summary>
    /// 
    /// </summary>
    public TransacaoProfiles()
    {
        CreateMap<CreateTransacaoDto, Transacao>();
        CreateMap<UpdateTransacaoDto, Transacao>();
            
        CreateMap<Transacao, ReadTransacaoDto>().ForMember(readTransacaoDto => readTransacaoDto.Categoria
            , opt => opt.MapFrom(transacao => transacao.Categoria));
                
    }
}