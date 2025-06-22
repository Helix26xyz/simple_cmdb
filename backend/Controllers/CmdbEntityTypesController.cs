using backend.Data;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/CmdbEntityType")]
    public class CmdbEntityTypeController : ControllerBase
    {
        private readonly CmdbContext _context;

        public CmdbEntityTypeController(CmdbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CmdbEntityType>>> GetAll()
        {
            return await _context.CmdbEntityTypes.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CmdbEntityType>> Get(Guid id)
        {
            var type = await _context.CmdbEntityTypes.FindAsync(id);
            if (type == null) return NotFound();
            return type;
        }

        [HttpPost]
        public async Task<ActionResult<CmdbEntityType>> Create(CmdbEntityType type)
        {
            type.Id = Guid.NewGuid();
            _context.CmdbEntityTypes.Add(type);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = type.Id }, type);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, CmdbEntityType type)
        {
            if (id != type.Id) return BadRequest();
            _context.Entry(type).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.CmdbEntityTypes.Any(e => e.Id == id))
                    return NotFound();
                throw;
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var type = await _context.CmdbEntityTypes.FindAsync(id);
            if (type == null) return NotFound();
            _context.CmdbEntityTypes.Remove(type);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
