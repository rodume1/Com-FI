using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Com_Fi.Data;
using Com_Fi.Models;
using Microsoft.AspNetCore.Identity;

namespace Com_Fi.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistsAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ArtistsAPIController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/ArtistsAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArtistsViewModel>>> GetArtists()
        {
            // list of artists. each album follows ArtistViewModel structure
            return await _context.Artists
                                 .Select(a => new ArtistsViewModel
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                     UserId = a.UserId,
                                     Email = _context.Users.Where(u => u.Id == a.UserId).FirstOrDefault().Email,
                                     RegistrationDate = _context.Users.Where(u => u.Id == a.UserId).FirstOrDefault().RegistrationDate
                                 })
                                 .ToListAsync();
        }

        // GET: api/ArtistsAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ArtistsViewModel>> GetArtist(int id)
        {
            // artist that follows ArtistsViewModel structure
            var artist = await _context.Artists
                                       .Select(a => new ArtistsViewModel
                                       {
                                           Id = a.Id,
                                           Name = a.Name,
                                           UserId = a.UserId,
                                           Email = _context.Users.Where(u => u.Id == a.UserId).FirstOrDefault().Email,
                                           RegistrationDate = _context.Users.Where(u => u.Id == a.UserId).FirstOrDefault().RegistrationDate
                                       })
                                       .Where(a => a.Id == id)
                                       .FirstOrDefaultAsync();

            if (artist == null)
            {
                return NotFound();
            }

            return artist;
        }

        // PUT: api/ArtistsAPI/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArtist(int id, Artists artist)
        {
            if (id != artist.Id)
            {
                return BadRequest();
            }

            _context.Entry(artist).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArtistExists(id))
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

        // POST: api/ArtistsAPI
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Artists>> PostArtist([FromForm] ArtistsViewModel artistVM)
        {
            // create new user
            var user = new ApplicationUser();
            user.UserName = artistVM.Email;
            user.Name = artistVM.Name;
            user.Email = artistVM.Email;
            user.RegistrationDate = DateTime.Now;

            var result = await _userManager.CreateAsync(user, artistVM.Password);

            // new artist (because what this api entrypoint expects are strings)
            var artist = new Artists();
            artist.Name = artistVM.Name;
            artist.UserId = user.Id;

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Artist");

                try
                {
                    _context.Artists.Add(artist);
                    await _context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    return BadRequest("Não foi possível criar música");
                }
            } else
            {
                return BadRequest("Erro ao criar novo utilizador/artista.\nCertifique-se de que a palavra-chave contém (pelo menos) 1 caracter alfanumérico, 1 maíuscula e 1 minúscula.\nA palavra-chave tem de ter, pelo menos, 6 caracteres ");
            }

            return CreatedAtAction("GetArtists", new { id = artist.Id }, artist);
        }

        // DELETE: api/ArtistsAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArtist([FromForm] int id)
        {
            var artist = await _context.Artists.FindAsync(id);
            var user = await _userManager.FindByIdAsync(artist.UserId);

            if (artist == null || user == null)
            {
                return BadRequest("Artista/Utilizador não encontrado");
            }
            try
            {
                _context.Artists.Remove(artist);
                await _userManager.DeleteAsync(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return BadRequest("Erro ao eliminar artista/utilizador");
            }

            return NoContent();
        }

        private bool ArtistExists(int id)
        {
            return _context.Artists.Any(e => e.Id == id);
        }
    }
}
