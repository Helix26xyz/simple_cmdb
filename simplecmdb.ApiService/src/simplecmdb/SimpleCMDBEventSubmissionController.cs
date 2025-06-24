using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using simplecmdb.SharedModels.models;
using System.Threading.Tasks;
using simplecmdb.SharedModels.storage;

namespace simplecmdb.ApiService.src
{
    [Route("api/wes")]
    [ApiController]
    public class SimpleCMDBEventsSubmissionController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SimpleCMDBEventsSubmissionController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/wes/:org/:project/:webhookSlug
        [HttpGet("{org}/{project}/{webhookSlug}")]
        public async Task<ActionResult<SimpleCMDBEvent>> GetSimpleCMDBEvent(String org, String project, String webhookSlug)
        {
            var webhook = await _context.SimpleCMDBs.FirstOrDefaultAsync(w => w.Slug == webhookSlug &&
            w.Owner == org &&
            w.Project == project &&
            w.Status != SimpleCMDBStatus.Disabled
            );
             if (webhook == null)
            {
                 return NotFound();
            }

            var webhookEvent = new SimpleCMDBEvent
            {
                SimpleCMDBId = webhook.Id,
                Status = SimpleCMDBEventStatus.New,
                SubStatus = SimpleCMDBEventSubStatus.Pending,
                Payload = string.Empty,
                SimpleCMDB = webhook

            };

            _context.SimpleCMDBEvents.Add(webhookEvent);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSimpleCMDBEvent), new { id = webhookEvent.Id }, webhookEvent);
        }

        // POST: api/webhook/:webhookSlug
        [HttpPost("{org}/{project}/{webhookSlug}")]
        public async Task<ActionResult<SimpleCMDBEvent>> PostSimpleCMDBEvent(String org, String project, String webhookSlug, [FromBody] object payload)
        {
            try{

            
            var webhook = await _context.SimpleCMDBs.FirstOrDefaultAsync(w => w.Slug == webhookSlug &&
            w.Owner == org &&
            w.Project == project &&
            w.Status != SimpleCMDBStatus.Disabled
            );
            if (webhook == null)
            {
                return NotFound();
            }


            var webhookEvent = new SimpleCMDBEvent
            {
                Payload = System.Text.Json.JsonSerializer.Serialize(payload),
                Status = SimpleCMDBEventStatus.New,
                SubStatus = SimpleCMDBEventSubStatus.Pending,
                SimpleCMDB = webhook
            };

            _context.SimpleCMDBEvents.Add(webhookEvent);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(PostSimpleCMDBEvent), new { id = webhookEvent.Id }, webhookEvent);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
