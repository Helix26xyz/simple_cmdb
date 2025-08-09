using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using simplecmdb.SharedModels.models;
using simplecmdb.SharedModels.storage;

namespace simplecmdb.ApiService.src
{
    [Route("api/[controller]")]
    [ApiController]
    public class CmdbTypesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CmdbTypesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CmdbType>>> GetCmdbTypes()
        {
            return await _context.CmdbTypes.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CmdbType>> GetCmdbType(Guid id)
        {
            var type = await _context.CmdbTypes.FindAsync(id);
            if (type == null) return NotFound();
            return type;
        }

        [HttpPost]
        public async Task<ActionResult<CmdbType>> PostCmdbType(CmdbType type)
        {
            _context.CmdbTypes.Add(type);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCmdbType), new { id = type.Id }, type);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCmdbType(Guid id, CmdbType type)
        {
            if (id != type.Id) return BadRequest();
            _context.Entry(type).State = EntityState.Modified;
            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.CmdbTypes.Any(e => e.Id == id)) return NotFound();
                else throw;
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCmdbType(Guid id)
        {
            var type = await _context.CmdbTypes.FindAsync(id);
            if (type == null) return NotFound();
            _context.CmdbTypes.Remove(type);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
