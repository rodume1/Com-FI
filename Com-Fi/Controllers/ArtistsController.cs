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
    [Authorize(Roles = "Artist")]
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
        public async Task<IActionResult> Index()
        {
              return View(await _context.Artists.ToListAsync());
        }

        // GET: Artists/Details/5
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
        public IActionResult Create()
        {
            return View();
        }

        // POST: Artists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Artists artists)
        {
            if (ModelState.IsValid)
            {
                _context.Add(artists);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(artists);
        }

        // GET: Artists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Artists == null)
            {
                return NotFound();
            }

            var artists = await _context.Artists.FindAsync(id);
            if (artists == null)
            {
                return NotFound();
            }
            return View(artists);
        }

        // POST: Artists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Artists artists)
        {
            // get user ID
            string userID = _userManager.GetUserId(User);

            // artist data that represents the logged in user
            var artistData = _context.Artists
                                     .AsNoTracking()
                                     .Where(a => a.UserId == userID)
                                     .SingleOrDefault(a => a.Id == id);

            // guarantees data's persistency
            if (id != artists.Id)
            {
                return NotFound();
            }

            // only the user can edit their own details
            if (artistData == null) 
            { 
                return NotFound(); 
            }

            // updates the value of the user's name on the user table
            // get ApplicationUser with logged in user's ID
            ApplicationUser user = _context.Users.Where(u => u.Id == userID).SingleOrDefault(u => u.Id == userID);

            try 
            {
                // set new UserName
                user.Name = artists.Name;

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
                    artists.UserId = artistData.UserId;
                    _context.Update(artists);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArtistsExists(artists.Id))
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
            return View(artists);
        }

        // GET: Artists/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Artists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
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
                return NotFound();
            }            

            if (_context.Artists == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Artists'  is null.");
            }
            var artists = await _context.Artists.FindAsync(id);
            if (artists != null)
            {
                _context.Artists.Remove(artists);
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
