using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Com_Fi.Data;
using Com_Fi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Com_Fi.Controllers
{
    public class ArtistsController : Controller
    {
        /// <summary>
        /// reference the application database
        /// </summary>
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// gets all data from authenticated user
        /// </summary>
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// manager that helps with loggin/logout in application
        /// </summary>
        private readonly SignInManager<ApplicationUser> _signInManager;

        public ArtistsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> SignInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = SignInManager;
        }

        // GET: Artists
        [Authorize(Roles = "User,Artist")]
        public async Task<IActionResult> Index(String errorMessage)
        {
            // if there is an error message, displays it to client
            if (errorMessage != null)
            {
                ModelState.AddModelError("CustomError", errorMessage);
            }

            return View(await _context.Artists.ToListAsync());
        }

        // GET: Artists/Details/5
        [Authorize(Roles = "User,Artist")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Artists == null)
            {
                return NotFound();
            }

            var artists = await _context.Artists
                .FirstOrDefaultAsync(m => m.Id == id);
            if (artists == null)
            {
                return NotFound();
            }

            return View(artists);
        }

        // GET: Artists/Create
        [Authorize(Roles = "Artist")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Artists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Artists artist)
        {
            if (ModelState.IsValid)
            {
                _context.Add(artist);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(artist);
        }

        // GET: Artists/Edit/5
        [Authorize(Roles = "Artist")]
        public async Task<IActionResult> Edit(int? id)
        {
            // get user ID
            string userID = _userManager.GetUserId(User);

            if (id == null || _context.Artists == null)
            {
                return NotFound();
            }

            Artists artist = await _context.Artists.FindAsync(id);

            // artist data that represents the logged in user
            var artistData = _context.Artists
                                     .AsNoTracking()
                                     .Where(a => a.UserId == userID)
                                     .SingleOrDefault(a => a.Id == id);

            if (artist == null)
            {
                return NotFound();
            }

            if (artistData == null)
            {
                // returns to "Index" with an error message
                return RedirectToAction("Index", new { ErrorMessage = "Não é possível editar este artista." });
            }
            return View(artist);
        }

        // POST: Artists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Artist")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Artists artist)
        {
            // get user ID
            string userID = _userManager.GetUserId(User);

            // artist data that represents the logged in user
            var artistData = _context.Artists
                                     .AsNoTracking()
                                     .Where(a => a.UserId == userID)
                                     .SingleOrDefault(a => a.Id == id);

            // guarantees data's persistency
            if (id != artist.Id)
            {
                return NotFound();
            }

            // only the user can edit their own details
            if (artistData == null) 
            {
                // returns to "Index" with an error message
                return RedirectToAction("Index", new { ErrorMessage = "Não é possível editar este album." });
            }

            // updates the value of the user's name on the user table
            // get ApplicationUser with logged in user's ID
            ApplicationUser user = _context.Users.Where(u => u.Id == userID).SingleOrDefault(u => u.Id == userID);

            try 
            {
                // set new UserName
                user.Name = artist.Name;

                // update User
                _context.Update(user);
            }
            catch (Exception ex)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {
                try
                {
                    artist.UserId = artistData.UserId;
                    _context.Update(artist);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArtistsExists(artist.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(artist);
        }

        // GET: Artists/Delete/5
        [Authorize(Roles = "Artist")]
        public async Task<IActionResult> Delete(int? id)
        {
            // get user ID
            string userID = _userManager.GetUserId(User);

            if (id == null || _context.Artists == null)
            {
                return NotFound();
            }

            Artists artist = await _context.Artists
                .FirstOrDefaultAsync(m => m.Id == id);

            // artist data that represents the logged in user
            var artistData = _context.Artists
                                     .AsNoTracking()
                                     .Where(a => a.UserId == userID)
                                     .SingleOrDefault(a => a.Id == id);

            // only the user can edit their own details
            if (artistData == null)
            {
                // returns to "Index" with an error message
                return RedirectToAction("Index", new { ErrorMessage = "Não é possível eliminar este artista." });
            }


            if (artist == null)
            {
                return NotFound();
            }

            return View(artist);
        }

        // POST: Artists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Artist")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // get user ID
            string userID = _userManager.GetUserId(User);

            // artist data that represents the logged in user
            var artistData = _context.Artists
                                     .AsNoTracking()
                                     .Where(a => a.UserId == userID)
                                     .SingleOrDefault(a => a.Id == id);

            // only the user can delete their own account
            if (artistData == null)
            {
                // returns to "Index" with an error message
                return RedirectToAction("Index", new { ErrorMessage = "Não é possível eliminar este artista." });
            }            

            if (_context.Artists == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Artists'  is null.");
            }
            var artist = await _context.Artists.FindAsync(id);
            if (artist != null)
            {
                _context.Artists.Remove(artist);
            }

            // get ApplicationUser with logged in user's ID
            ApplicationUser user = _context.Users.Where(u => u.Id == userID).SingleOrDefault(u => u.Id == userID);
            try
            {
                // logs out
                await _signInManager.SignOutAsync();
                // remove respective user
                _context.Users.Remove(user);                
            }
            catch (Exception ex) 
            {
                return NotFound();
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        private bool ArtistsExists(int id)
        {
          return _context.Artists.Any(e => e.Id == id);
        }
    }
}
