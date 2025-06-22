using backend.Data;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/CmdbEntity")]
    public class CmdbEntityController : ControllerBase
    {
        private readonly CmdbContext _context;

        public CmdbEntityController(CmdbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CmdbEntity>>> GetAll()
        {
            return await _context.CmdbEntities.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CmdbEntity>> Get(Guid id)
        {
            var entity = await _context.CmdbEntities.FindAsync(id);
            if (entity == null) return NotFound();
            return entity;
        }

        [HttpPost]
        public async Task<ActionResult<CmdbEntity>> Create(CmdbEntity entity)
        {
            entity.Id = Guid.NewGuid();
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;
            _context.CmdbEntities.Add(entity);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = entity.Id }, entity);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, CmdbEntity entity)
        {
            if (id != entity.Id) return BadRequest();
            entity.UpdatedAt = DateTime.UtcNow;
            _context.Entry(entity).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.CmdbEntities.Any(e => e.Id == id))
                    return NotFound();
                throw;
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var entity = await _context.CmdbEntities.FindAsync(id);
            if (entity == null) return NotFound();
            _context.CmdbEntities.Remove(entity);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
