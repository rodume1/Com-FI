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
    public class AlbumsAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AlbumsAPIController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Albums
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AlbumsViewModel>>> GetAlbums()
        {
            // list of albums. each album follows AlbumsViewModel structure
            return await _context.Albums
                                 .Include(a => a.AlbumMusics)
                                 .Select(a => new AlbumsViewModel
                                 {
                                     Id = a.Id,
                                     Title = a.Title,
                                     ReleaseYear = a.ReleaseYear,
                                     AlbumMusics = a.AlbumMusics.ToList()
                                 })
                                 .ToListAsync();
        }

        // GET: api/Albums/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AlbumsViewModel>> GetAlbum(int id)
        {
            // album that follows AlbumsViewModel structure
            var album = await _context.Albums
                                      .Include(a => a.AlbumMusics)
                                      .Select(a => new AlbumsViewModel
                                      {
                                          Id = a.Id,
                                          Title = a.Title,
                                          ReleaseYear = a.ReleaseYear,
                                          AlbumMusics = a.AlbumMusics.ToList()
                                      })
                                      .Where(a => a.Id == id)
                                      .FirstOrDefaultAsync();

            if (album == null)
            {
                return NotFound();
            }

            return album;
        }

        // PUT: api/Albums/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAlbum(int id, Albums album)
        {
            if (id != album.Id)
            {
                return BadRequest();
            }

            _context.Entry(album).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AlbumExists(id))
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

        // POST: api/Albums
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Albums>> PostAlbum([FromForm] Albums album)
        {
            // creates a new list to store musics IDs
            List<int> auxMusics = new List<int>();

            // list all musics and stores its ID
            foreach (var music in album.AlbumMusics)
            {
                auxMusics.Add(music.Id);
            }

            // clears all album's musics
            album.AlbumMusics.Clear();

            // repopulates albums with musics
            foreach (var music in auxMusics)
            {
                album.AlbumMusics.Add(_context.Musics.Where(m => m.Id == music).FirstOrDefault());
            }

            try
            {
                album.Cover = "defaultCover.jpg";
                _context.Albums.Add(album);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return BadRequest("Erro ao criar novo álbum");
            }

            return CreatedAtAction("GetAlbums", new { id = album.Id }, album);
        }

        // DELETE: api/Albums/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAlbum(int id)
        {
            var album = await _context.Albums.FindAsync(id);
            if (album == null)
            {
                return NotFound();
            }

            _context.Albums.Remove(album);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AlbumExists(int id)
        {
            return _context.Albums.Any(e => e.Id == id);
        }
    }
}
