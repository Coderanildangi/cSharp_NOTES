using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Note_Server.Data;
using Note_Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Note_Server.Controllers
{
    [ApiController]
    [Route("api/notes")]
    public class NotesController : ControllerBase
    {
        // For demonstration, keep the PIN hardcoded here, though in a real app, it would be in configuration/secrets.
        public const string CONFIDENTIAL_PIN = "1234";
        private readonly NotesDbContext _context;

        // Inject DbContext via constructor
        public NotesController(NotesDbContext context)
        {
            _context = context;
        }

        // GET: /api/notes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Note>>> GetNotes()
        {
            // Fetch all notes, ordered by last update time
            var notesList = await _context.Notes
                .OrderByDescending(n => n.UpdatedAt)
                .ToListAsync();

            // Apply redaction logic in the application layer
            var redactedList = notesList
                .Select(n => n.IsConfidential ? n.RedactedCopy() : n)
                .ToList();

            return Ok(redactedList);
        }

        // GET: /api/notes/{id}?pin=...
        [HttpGet("{id}")]
        public async Task<ActionResult<Note>> GetNote(string id, [FromQuery] string? pin)
        {
            var note = await _context.Notes.FindAsync(id);

            if (note == null)
            {
                return NotFound();
            }

            if (note.IsConfidential)
            {
                if (pin == CONFIDENTIAL_PIN)
                {
                    // PIN correct, return the full note
                    return Ok(note);
                }
                else
                {
                    // PIN incorrect or missing, return unauthorized
                    return Unauthorized();
                }
            }

            // Not confidential, return the full note
            return Ok(note);
        }

        // POST: /api/notes
        [HttpPost]
        public async Task<ActionResult<Note>> CreateNote(Note newNote)
        {
            // The request body provides the initial data, but the server controls the ID and timestamps
            var createdNote = new Note
            {
                Id = Guid.NewGuid().ToString(), // Ensure new ID
                Title = newNote.Title,
                Content = newNote.Content,
                Tag = newNote.Tag,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Notes.Add(createdNote);
            await _context.SaveChangesAsync();

            // Use CreatedAtAction for standard REST response
            return CreatedAtAction(nameof(GetNote), new { id = createdNote.Id }, createdNote);
        }

        // PUT: /api/notes/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNote(string id, Note updatedNote)
        {
            if (id != updatedNote.Id)
            {
                // Basic check for ID consistency
                return BadRequest("Note ID mismatch.");
            }

            var existingNote = await _context.Notes.FindAsync(id);

            if (existingNote == null)
            {
                return NotFound();
            }

            // Update fields and timestamp
            existingNote.Title = updatedNote.Title;
            existingNote.Content = updatedNote.Content;
            existingNote.Tag = updatedNote.Tag;
            existingNote.UpdatedAt = DateTime.UtcNow;

            _context.Entry(existingNote).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Notes.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // DELETE: /api/notes/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNote(string id)
        {
            var existingNote = await _context.Notes.FindAsync(id);

            if (existingNote == null)
            {
                return NotFound();
            }

            _context.Notes.Remove(existingNote);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}