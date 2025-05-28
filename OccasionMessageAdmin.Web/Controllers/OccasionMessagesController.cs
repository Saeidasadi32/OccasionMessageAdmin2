using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OccasionMessageAdmin.Shared.Models;
using OccasionMessageAdmin.Web.Data;

namespace OccasionMessageAdmin.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OccasionMessagesController : ControllerBase
    {
        private readonly OccasionDbContext _context;

        public OccasionMessagesController(OccasionDbContext context)
        {
            _context = context;
        }

        // GET: api/OccasionMessages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OccasionMessage>>> GetOccasionMessages()
        {
            return await _context.OccasionMessages.ToListAsync();
        }

        // GET: api/OccasionMessages/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OccasionMessage>> GetOccasionMessage(int id)
        {
            var message = await _context.OccasionMessages.FindAsync(id);
            if (message == null)
                return NotFound();

            return message;
        }

        // POST: api/OccasionMessages
        [HttpPost]
        public async Task<ActionResult<OccasionMessage>> PostOccasionMessage(OccasionMessage message)
        {
            _context.OccasionMessages.Add(message);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOccasionMessage), new { id = message.MessageID }, message);
        }

        // PUT: api/OccasionMessages/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOccasionMessage(int id, OccasionMessage message)
        {
            if (id != message.MessageID)
                return BadRequest();

            _context.Entry(message).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OccasionMessageExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // DELETE: api/OccasionMessages/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOccasionMessage(int id)
        {
            var message = await _context.OccasionMessages.FindAsync(id);
            if (message == null)
                return NotFound();

            _context.OccasionMessages.Remove(message);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OccasionMessageExists(int id)
        {
            return _context.OccasionMessages.Any(e => e.MessageID == id);
        }
    }
}
