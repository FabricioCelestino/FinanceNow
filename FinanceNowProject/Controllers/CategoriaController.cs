using AutoMapper;
using FinanceNow.API.DTOs.CategoriaDTOs;
using FinanceNow.Data.DataBase;
using FinanceNow.Modelos.Models;
using FinanceNow.Modelos.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinanceNow.API.Controllers;

/// <summary>
/// </summary>
/// <param name="context"></param>
/// <param name="mapper"></param>
[ApiController]
//[Authorize]
[Route("Api/[Controller]")]
public class CategoriaController(Context context, IMapper mapper) : ControllerBase
{
    /// <summary>
    ///     obter todas as categorias
    /// </summary>
    /// <returns></returns>
    [HttpGet("Tipo/{tipo}")]
    public async Task<IActionResult> GetByTipoAsync(string tipo)
    {
        if (!Enum.TryParse<TipoDeTransacao>(tipo, true, out var tipoDeTransacao)) return BadRequest();

        var categorias = await context.Categorias.Where(c => c.Tipo == tipoDeTransacao).ToListAsync();

        if (categorias is null) throw new ArgumentNullException(nameof(categorias));

        var categoriasDto = mapper.Map<List<ReadCategoriaDto>>(categorias);

        return Ok(categoriasDto);
    }

    /// <summary>
    ///     obter categoria por id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetByIdAsync(int? id)
    {
        if (id is null) return BadRequest();

        var categoria = await context.Categorias.FindAsync(id);

        if (categoria is null) return NotFound();

        return Ok(categoria);
    }

    /// <summary>
    ///     Adicionar categoria ao banco de dados
    /// </summary>
    /// <param name="categoriaDto"></param>
    /// <returns>IActionResult</returns>
    /// <Response code="201">Caso a inserção seja feita com sucesso</Response>
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateCategoriaDto categoriaDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        Categoria categoria = new()
        {
            Nome = categoriaDto.Nome,
            Tipo = categoriaDto.Tipo
        };


        await context.Categorias.AddAsync(categoria);
        await context.SaveChangesAsync();

        return StatusCode(201);
    }


    /// <summary>
    ///     Deletar categoria do banco de dados
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeletarAsync(int? id)
    {
        if (id is null) return BadRequest();

        var categoria = await context.Categorias.FindAsync(id);

        if (categoria is null) return NotFound();

        context.Categorias.Remove(categoria);
        await context.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>
    /// Atualizar categoria
    /// </summary>
    /// <param name="id"></param>
    /// <param name="categoriaDto"></param>
    /// <returns></returns>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> EditarAsync(int? id, [FromBody] UpdateCategoriaDto categoriaDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        if (id is null) return BadRequest();
        
        var categoria = await context.Categorias.FindAsync(id);
        
        if (categoria is null) return NotFound();
       

        mapper.Map(categoriaDto, categoria);

        context.Categorias.Update(categoria);
        await context.SaveChangesAsync();
        return NoContent();
    }
}