using FinanceNow.Data.DataBase;
using Microsoft.AspNetCore.Mvc;

namespace FinanceNow.API.Controllers
{
    [ApiController]
    [Route("Api/[Controller]")]
    public class CategoriaController(Context context) : ControllerBase
    {
        private readonly Context _context = context;


    }
}
