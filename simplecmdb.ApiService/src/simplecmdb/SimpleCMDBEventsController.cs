using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using simplecmdb.SharedModels.models;
using System.Threading.Tasks;
using static simplecmdb.SharedModels.models.SimpleCMDBEvent;
using simplecmdb.SharedModels.storage;

namespace simplecmdb.ApiService.src
{
    // CRUD on a specific SimpleCMDBEvent
    [Route("api/[controller]")]
    [ApiController]
    public class SimpleCMDBEventsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SimpleCMDBEventsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/webhookevents
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<SimpleCMDBEvent>>> GetSimpleCMDBEvents(Guid id)
        {
            var webhookevents = await _context.SimpleCMDBEvents.ToListAsync();
            return webhookevents;
        }

        // GET: api/webhookevents/:id
        [HttpGet("{id}")]
        public async Task<ActionResult<SimpleCMDBEvent>> GetSimpleCMDBEvent(Guid id)
        {
            var webhookevent = await _context.SimpleCMDBEvents.FirstOrDefaultAsync(w => w.Id == id);
            if (webhookevent == null)
            {
                return NotFound();
            }
            return webhookevent;
        }

        // GET: api/webhookevents/bywebhook/{webhookId}
        [HttpGet("bywebhook/{webhookId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetSimpleCMDBEventsBySimpleCMDBId(Guid webhookId)
        {
            var webhookEvents = await _context.SimpleCMDBEvents
                .Where(we => we.SimpleCMDBId == webhookId)
                .Select(we => new
                {
                    we.Id,
                    we.SimpleCMDBId,
                    we.CreatedAt,
                    we.UpdatedAt,
                    Status = we.Status.ToString(), // Convert enum to string
                    SubStatus = we.SubStatus.ToString(), // Convert enum to string
                    we.StatusResultText
                })
                .ToListAsync();

            return Ok(webhookEvents);
        }

        // GET: api/webhookevents/receive/{webhookId}
        [HttpGet("receive/{webhookId}")]
        public async Task<ActionResult<SimpleCMDBEvent>> ReceiveSimpleCMDBEvent(Guid webhookId)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Get the oldest webhook event that has not been received
                    var webhookEvent = await _context.SimpleCMDBEvents
                        .Where(we => we.SimpleCMDBId == webhookId && we.Status == SimpleCMDBEventStatus.New)
                        .OrderBy(we => we.CreatedAt)
                        .FirstOrDefaultAsync();

                    if (webhookEvent == null)
                    {
                        // return a 204 No Content if no webhook event is found
                        return NoContent();
                    }

                    // Mark it as received
                    webhookEvent.Status = SimpleCMDBEventStatus.Received;
                    webhookEvent.UpdatedAt = DateTime.UtcNow;

                    // Save changes
                    await _context.SaveChangesAsync();

                    // Commit transaction
                    await transaction.CommitAsync();

                    return Ok(webhookEvent);
                }
                catch (Exception)
                {
                    // Rollback transaction if any error occurs
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }


        [HttpPut("return/{webhookEventId}")]
        public async Task<ActionResult<SimpleCMDBEvent>> ReturnSimpleCMDBEvent(Guid webhookEventId, [FromBody] SimpleCMDBEventWorkResponse wewr)
        {
            // update webhook event status to completed + status and add result text
            var webhookEvent = await _context.SimpleCMDBEvents.FirstOrDefaultAsync(we => we.Id == webhookEventId);
            if (webhookEvent == null)
            {
                return NotFound();

            }
            webhookEvent.Status = SimpleCMDBEventStatus.Processed;
            webhookEvent.SubStatus = wewr.Status;
            webhookEvent.StatusResultText = wewr.ResultText;
            webhookEvent.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return Ok(webhookEvent);
        }
    }

}
