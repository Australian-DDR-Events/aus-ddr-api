using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AusDdrApi.Models.Responses;
using AusDdrApi.Persistence;
using AusDdrApi.Services.FileStorage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AusDdrApi.Controllers.Summer2021Event
{
    [ApiController]
    [Route("summer2021/[controller]")]
    public class IngredientsController : ControllerBase
    {
        private readonly ILogger<IngredientsController> _logger;
        private DatabaseContext _context;
        private IFileStorage _fileStorage;

        public IngredientsController(ILogger<IngredientsController> logger, DatabaseContext context, IFileStorage fileStorage)
        {
            _logger = logger;
            _context = context;
            _fileStorage = fileStorage;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<IngredientResponse>>> Get()
        {
            return _context
                .Ingredients
                .Include(s => s.Song)
                .Select(IngredientResponse.FromEntity)
                .ToArray();
        }
    }
}