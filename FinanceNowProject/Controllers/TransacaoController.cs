using AutoMapper;
using FinanceNow.API.DTOs.TransacaoDTOs;
using FinanceNow.Modelos.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using FinanceNow.API.Services;
using System.ComponentModel.DataAnnotations;

namespace FinanceNow.API.Controllers;

/// <summary>
/// </summary>
/// <param name="repository"></param>
/// <param name="mapper"></param>
[ApiController]
[Route("Api/[controller]")]
public class TransacaoController(IMapper mapper, ILogger<TransacaoController> logger, ITransacaoService service) : ControllerBase
{

    /// <summary>
    ///     Adicionar transação ao banco de dados
    /// </summary>
    /// <param name="transacaoDto"></param>
    /// <returns>IActionResult</returns>
    /// <Response code="201">Caso a inserção seja feita com sucesso</Response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CreateTransacaoDto>> Create([FromBody] CreateTransacaoDto transacaoDto, CancellationToken ct)
    {
        if (transacaoDto is null) return BadRequest("Corpo da requisição não pode ser nulo.");
        var created = await service.CreateAsync(transacaoDto, ct);
        var readDto = mapper.Map<ReadTransacaoDto>(created);
        return CreatedAtAction(nameof(Read), new { id = readDto.Id }, readDto);

    }

    /// <summary>
    ///     Detalhes da transação
    /// </summary>
    /// <param name="id"></param>
    /// <returns>IActionResult</returns>
    /// <Response code="200">Caso a inserção seja feita com sucesso</Response>
    [HttpGet("{id:int?}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Read(int? id, CancellationToken ct)
    {

        var transacao = await service.GetByIdAsync(id, ct);
        if (transacao is null) return NotFound($"Transação {id} não encontrada.");

        var dto = mapper.Map<ReadTransacaoDto>(transacao);
        return Ok(dto);

    }

    /// <summary>
    /// Atualizar transação 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="transacaoDto"></param>
    /// <returns></returns>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UpdateTransacaoDto>> Update([FromBody] UpdateTransacaoDto transacaoDto, CancellationToken ct)
    {
        if (transacaoDto is null) return BadRequest("Corpo da requisição não pode ser nulo.");
        var updated = await service.UpdateAsync(transacaoDto, ct);
        if (!updated) return NotFound($"Transação {transacaoDto.Id} não encontrada.");
        return NoContent();
    }

    /// <summary>
    /// Exclui uma transação pelo Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id:int?}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Delete(int id, CancellationToken ct)
    {
        var removed = await service.DeleteAsync(id, ct);
        if (!removed) return NotFound($"Transação {id} não encontrada.");
        return NoContent();
    }

    /// <summary>
    /// </summary>
    /// <param name="ano"></param>
    /// <param name="mes"></param>
    /// <param name="tipo"></param>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<ReadTransacaoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetByPeriodo(
        [FromQuery, Range(2000, 3000, ErrorMessage = "O ano deve estar entre 2000 e 3000.")] ushort ano,
        [FromQuery, Range(1, 12, ErrorMessage = "O mês deve estar entre 1 e 12.")] byte mes,
        [FromQuery] string tipo,
        CancellationToken ct)
    {

        if (!Enum.TryParse<TipoDeTransacao>(tipo, true, out var tipoDeTransacao))
        {
            return Problem(
                title: "Tipo de transação inválido",
                detail: "Use 'Receita' ou 'Despesa' como tipo de transação.",
                statusCode: StatusCodes.Status400BadRequest);
        }
        var listaDeTransacoes = await service.GetByPeriodAsync(ano, mes, tipoDeTransacao, ct);
        var listaDeTransacaoDto = mapper.Map<IReadOnlyCollection<ReadTransacaoDto>>(listaDeTransacoes);
        return Ok(listaDeTransacaoDto);
    }
}
