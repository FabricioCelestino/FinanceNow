using FinanceNow.API.DTOs.CategoriaDTOs;
using FinanceNow.Data.DataBase;
using FinanceNow.Modelos.Models;
using Microsoft.AspNetCore.Mvc;

namespace FinanceNow.API.Controllers
{
    [ApiController]
    [Route("Api/[Controller]")]
    public class CategoriaController(Context context) : ControllerBase
    {
        private readonly Context _context = context;

        /// <summary>
        /// Adicionar categoria ao banco de dados
        /// </summary>
        /// <param name="categoriaDTO"></param>
        /// <returns>IactionResult</returns>
        /// <Response code="201">Caso a insersão seja feita com sucesso</Response>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody] CreateCategoriaDTO categoriaDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Categoria categoria = new() 
            { 
                Name = categoriaDTO.Nome,
                Tipo = categoriaDTO.TipoDeTransacao,
            };


            await _context.Categorias.AddAsync(categoria);
            await _context.SaveChangesAsync();

            return Ok(categoria);

        }

    }
}
