using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using simplecmdb.SharedModels.models;
using System.Threading.Tasks;
using simplecmdb.SharedModels.storage;

namespace simplecmdb.ApiService.src
{
    [Route("api/[controller]")]
    [ApiController]
    public class SimpleCMDBsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SimpleCMDBsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/SimpleCMDBs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SimpleCMDB>>> GetSimpleCMDBs()
        {
            return await _context.SimpleCMDBs.ToListAsync();
        }

        // GET: api/SimpleCMDBs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SimpleCMDB>> GetSimpleCMDB(Guid id)
        {
            var webhook = await _context.SimpleCMDBs.FindAsync(id);

            if (webhook == null)
            {
                return NotFound();
            }

            return webhook;
        }

        // POST: api/SimpleCMDBs
        [HttpPost]
        public async Task<ActionResult<SimpleCMDB>> PostSimpleCMDB(SimpleCMDB webhook)
        {
            try
            {
                _context.SimpleCMDBs.Add(webhook);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetSimpleCMDB), new { id = webhook.Id }, webhook);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            }

        // PUT: api/SimpleCMDBs/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSimpleCMDB(Guid id, SimpleCMDB webhook)
        {
            if (id != webhook.Id)
            {
                return BadRequest();
            }

            var existingEntity = await _context.SimpleCMDBs.FindAsync(id);
            if (existingEntity != null)
            {
                _context.Entry(existingEntity).State = EntityState.Detached;
            }

            _context.Entry(webhook).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SimpleCMDBExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/SimpleCMDBs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSimpleCMDB(Guid id)
        {
            var webhook = await _context.SimpleCMDBs.FindAsync(id);
            if (webhook == null)
            {
                return NotFound();
            }

            _context.SimpleCMDBs.Remove(webhook);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SimpleCMDBExists(Guid id)
        {
            return _context.SimpleCMDBs.Any(e => e.Id == id);
        }
    }
}
