using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinanceNow.Data.DataBase;
using FinanceNow.Modelos.Models;
using FinanceNow.API.DTOs.TransacaoDTOs;
using FinanceNow.Modelos.Models.Enums;
using AutoMapper;

namespace FinanceNow.API.Controllers
{
    [ApiController]
    [Route("Api/[controller]")]
    public class TransacoesController(Context context, IMapper mapper) : ControllerBase
    {
        private readonly Context _context = context;
        private readonly IMapper _mapper = mapper;

        [HttpGet("{ano}/{mes}/{tipo}")]

        public async Task<IActionResult> Index(int ano, int mes, string tipo)
        {
            if (!Enum.TryParse<TipoDeTransacao>(tipo, true, out var tipoDeTransacao))
            {
                return BadRequest("Tipo de transação inválido. Use 'Receita' ou 'Despesa'.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ICollection<ReadTransacaoDTO> listaDeTransacao = _mapper.Map<ICollection<ReadTransacaoDTO>>(await _context.Transacoes.Include(t => t.Categoria).Where(t => t.DataDeVencimento.Year == ano &&
            t.DataDeVencimento.Month == mes && t.Tipo == tipoDeTransacao).ToListAsync());

            return Ok(listaDeTransacao);


        }


        /// <summary>
        /// Detalhes da transação
        /// </summary>
        /// <param name="id"></param>
        /// <returns>IActionResult</returns>
        /// <Response code="200">Caso a insersão seja feita com sucesso</Response>
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var transacao = await _context.Transacoes
                .Include(t => t.Categoria)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transacao is null)
            {
                return NotFound();
            }

            ReadTransacaoDTO readTransacaoDTO = _mapper.Map<ReadTransacaoDTO>(transacao);


            return Ok(readTransacaoDTO);
        }

        /// <summary>
        /// Adicionar transação ao banco de dados
        /// </summary>
        /// <param name="transacaoDTO"></param>
        /// <returns>IActionResult</returns>
        /// <Response code="201">Caso a insersão seja feita com sucesso</Response>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTransacaoDTO transacaoDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }



            var transacao = _mapper.Map<Transacao>(transacaoDTO);

            await _context.AddAsync(transacao);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Details), new { id = transacao.Id }, transacao);
        }

        [HttpPut("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int? id, [FromBody] UpdateTransacaoDTO transacaoDTO)
        {
            if (id is null)
            {
                return BadRequest();
            }

            var transacao = await _context.Transacoes.FindAsync(id);

            if (transacao is null)
            {
                return NotFound("Transação não encontrada");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _mapper.Map<UpdateTransacaoDTO, Transacao>(transacaoDTO, transacao);

            _context.Transacoes.Update(transacao);
            await _context.SaveChangesAsync();

            return NoContent();

        }

        [HttpDelete("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletarTransacao(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }

            var transacao = await _context.Transacoes.FindAsync(id);

            if (transacao is null)
            {
                return NotFound("Transação não encontrada");
            }

            _context.Transacoes.Remove(transacao);
            await _context.SaveChangesAsync();

            return NoContent();

        }

    }
}
