using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using simplecmdb.SharedModels.models;
using simplecmdb.SharedModels.storage;

namespace simplecmdb.ApiService.src
{
    [Route("api/[controller]")]
    [ApiController]
    public class CmdbRecordsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CmdbRecordsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CmdbRecord>>> GetCmdbRecords()
        {
            return await _context.CmdbRecords.Include(r => r.CmdbType).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CmdbRecord>> GetCmdbRecord(Guid id)
        {
            var record = await _context.CmdbRecords.Include(r => r.CmdbType).FirstOrDefaultAsync(r => r.Id == id);
            if (record == null) return NotFound();
            return record;
        }

        [HttpPost]
        public async Task<ActionResult<CmdbRecord>> PostCmdbRecord(CmdbRecord record)
        {
            record.CreatedAt = DateTime.UtcNow;
            record.UpdatedAt = DateTime.UtcNow;
            _context.CmdbRecords.Add(record);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCmdbRecord), new { id = record.Id }, record);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCmdbRecord(Guid id, CmdbRecord record)
        {
            if (id != record.Id) return BadRequest();
            record.UpdatedAt = DateTime.UtcNow;
            _context.Entry(record).State = EntityState.Modified;
            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.CmdbRecords.Any(e => e.Id == id)) return NotFound();
                else throw;
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCmdbRecord(Guid id)
        {
            var record = await _context.CmdbRecords.FindAsync(id);
            if (record == null) return NotFound();
            _context.CmdbRecords.Remove(record);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
