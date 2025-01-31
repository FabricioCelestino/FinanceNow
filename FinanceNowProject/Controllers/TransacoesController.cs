
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
    [Route("Api/[controller]")]
    public class TransacoesController(Context context) : ControllerBase
    {
        private readonly Context _context = context;

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

        [HttpPut("{id}")]
        public async Task<IActionResult> Editar(int? id, [FromBody] CreateTransacaoDTO transacaoDTO)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var transacao = await _context.Transacoes.FindAsync(id);

            if (transacao == null) 
            {
                return NotFound("Transação não encontrada");
            }

            transacao.Descricao = transacaoDTO.Descricao;
            transacao.Tipo = transacaoDTO.Tipo;
            transacao.DataDeVencimento = transacaoDTO.DataDeVencimento;
            transacao.CategoriaId = transacaoDTO.CategoriaId;
            transacao.Valor = transacaoDTO.Valor;

            _context.Transacoes.Update(transacao);
            await _context.SaveChangesAsync();

            return Ok(transacao);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarTransacao(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var transacao = await _context.Transacoes.FindAsync(id);

            if (transacao == null)
            {
                return NotFound("Transação não encontrada");
            }

            _context.Transacoes.Remove(transacao);
            await _context.SaveChangesAsync();

            return NoContent();

        }

    }
}
