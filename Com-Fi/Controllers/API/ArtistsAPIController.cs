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
    /// <summary>
    /// Web API controller that contains all entrypoints to handle Artists
    /// </summary>
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

        /// <summary>
        /// GET: api/ArtistsAPI
        /// Returns all artists
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// GET: api/ArtistsAPI/5
        /// Returns one specific artist's info. Artist is identified by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

            // if artist does not exists, returns a NotFound
            if (artist == null)
            {
                return NotFound();
            }

            return artist;
        }

        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// PUT: api/ArtistsAPI/5
        /// Edits one artist. Artist is identified by ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="artistVM"></param> artistVM is of type ArtistsViewModel so we can receive email in the request
        /// An artist does not have email, but an user does. One user and one artist are correlated, that's why we need to change both
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArtist([FromForm] int id, [FromForm] ArtistsViewModel artistVM)
        {
            if (id != artistVM.Id)
            {
                return BadRequest();
            }

            // gets user assigned to artist by ID
            var user = await _userManager.FindByIdAsync(artistVM.UserId);

            // if user does not exist, then it returns a BadRequest
            if (user == null)
            {
                return BadRequest("Utilizador não encontrado");
            }

            // re-assigns to the user the new artists values
            user.Name = artistVM.Name;
            user.UserName = artistVM.Email;
            user.Email = artistVM.Email;

            // updates the user values if successful
            // if not, returns a BadRequest
            try
            {
                await _userManager.UpdateAsync(user);
            }
            catch (Exception)
            {
                return BadRequest("Erro ao editar utilizador/artista");
            }

            // gets the artist by ID
            Artists artist = await _context.Artists
                                   .SingleOrDefaultAsync(a => a.Id == id);

            // if artist does not exist, then it returns a badRequest
            if (artist == null)
            {
                return BadRequest("Artista não encontrado");
            }

            // re-assigns to the artists its new valeus
            artist.Name = artistVM.Name;

            // updates the artist values if successful
            // if not, returns a BadRequest
            try
            {
                _context.Update(artist);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArtistExists(id))
                {   
                    return BadRequest("Este artista não existe");
                }
                else
                {
                    return BadRequest("Erro ao editar utilizador/artista");
                }
            }

            return NoContent();
        }

        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// POST: api/ArtistsAPI
        /// Adds a new artist.
        /// </summary>
        /// <param name="artistVM"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Artists>> PostArtist([FromForm] ArtistsViewModel artistVM)
        {
            // create new user, because an user and an artist are correlated
            var user = new ApplicationUser();
            // assigns to the user the artists values
            user.UserName = artistVM.Email;
            user.Name = artistVM.Name;
            user.Email = artistVM.Email;
            user.RegistrationDate = DateTime.Now;
            // we don't have anyway of confirming account via email, and we need this field
            // to be holding "true" value in order to access the account
            user.EmailConfirmed = true;

            // creates one user
            var result = await _userManager.CreateAsync(user, artistVM.Password);

            // new artist (because what this api entrypoint expects is an "artist" of the type ArtistsViewModel)
            var artist = new Artists();
            // assigns the artist its values
            artist.Name = artistVM.Name;
            artist.UserId = user.Id;

            // if it was possible to create an user, then assigns to it the role of Artist
            // if not, returns a BadRequest
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Artist");

                // creates one artist, if successful
                // if not, returns a BadRequest
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

        /// <summary>
        /// DELETE: api/ArtistsAPI/5
        /// Deletes one artist, identified by ID
        /// Deletes also its "assigned" user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArtist([FromForm] int id)
        {
            // finds artist by id
            var artist = await _context.Artists.FindAsync(id);
            // finds user by userID stored in the artist
            var user = await _userManager.FindByIdAsync(artist.UserId);

            // if artist or user are not found, returns a BadRequest
            if (artist == null || user == null)
            {
                return BadRequest("Artista/Utilizador não encontrado");
            }

            // Deletes artist and user, if successful
            // if not, returns a BadRequest
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

        // checks if one certain artist does exists
        private bool ArtistExists(int id)
        {
            return _context.Artists.Any(e => e.Id == id);
        }
    }
}
