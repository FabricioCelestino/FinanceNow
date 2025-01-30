
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinanceNow.Data.DataBase;
using FinanceNow.Modelos.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FinanceNow.API.DTOs.TransacaoDTOs;

namespace FinanceNow.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransacoesController : ControllerBase
    {
        private readonly Context _context;

        public TransacoesController(Context context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var listaDeTransacao = await _context.Transacoes.Include(t => t.Categoria).ToListAsync();
            return Ok(listaDeTransacao);

            
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transacao = await _context.Transacoes
                .Include(t => t.Categoria)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transacao == null)
            {
                return NotFound();
            }

            return Ok(transacao);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTransacaoDTO transacaoDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            Transacao transacao = new()
            {
                Valor = transacaoDTO.Valor,
                Descricao = transacaoDTO.Descricao,
                Tipo = transacaoDTO.Tipo,
                DataDeVencimento = transacaoDTO.DataDeVencimento,
                CategoriaId = transacaoDTO.CategoriaId,

            };



            await _context.AddAsync(transacao); 
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Details), new {id = transacao.Id}, transacao); 
        }

    }
}
