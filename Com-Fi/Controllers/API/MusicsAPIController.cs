using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Com_Fi.Data;
using Com_Fi.Models;

namespace Com_Fi.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class MusicsAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MusicsAPIController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/MusicsAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MusicsViewModel>>> GetMusics()
        {
            return await _context.Musics
                                .Select(m => new MusicsViewModel
                                {
                                    Id = m.Id,
                                    Title = m.Title,
                                    ReleaseYear = m.ReleaseYear,
                                    GenreFK = m.GenreFK,                                    
                                })
                                .ToListAsync();
        }

        // GET: api/MusicsAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MusicsViewModel>> GetMusic(int id)
        {
            var music = await _context.Musics                                
                                .Select(m => new MusicsViewModel
                                {
                                    Id = m.Id,
                                    Title = m.Title,
                                    ReleaseYear = m.ReleaseYear,
                                    GenreFK = m.GenreFK,
                                })
                                .Where(m => m.Id == id)
                                .FirstOrDefaultAsync();

            if (music == null)
            {
                return NotFound();
            }

            return music;
        }

        // PUT: api/MusicsAPI/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMusic(int id, Musics music)
        {
            if (id != music.Id)
            {
                return BadRequest();
            }

            _context.Entry(music).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MusicExists(id))
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

        // POST: api/MusicsAPI
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Musics>> PostMusic(Musics music)
        {
            _context.Musics.Add(music);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMusics", new { id = music.Id }, music);
        }

        // DELETE: api/MusicsAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMusic(int id)
        {
            var music = await _context.Musics.FindAsync(id);
            if (music == null)
            {
                return NotFound();
            }

            _context.Musics.Remove(music);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MusicExists(int id)
        {
            return _context.Musics.Any(e => e.Id == id);
        }
    }
}
