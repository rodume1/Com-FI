using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Com_Fi.Data;
using Com_Fi.Models;
using Microsoft.AspNetCore.Authorization;

namespace Com_Fi.Controllers
{
    /// <summary>
    /// Music controller, with all the methods used for albums (CRUD)
    /// </summary>
    public class MusicsController : Controller
    {
        /// <summary>
        /// reference the application database
        /// </summary>
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// gets all data from authenticated user
        /// </summary>
        private readonly UserManager<ApplicationUser> _userManager;

        public MusicsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: Musics
        [Authorize(Roles = "User,Artist")]
        public async Task<IActionResult> Index()
        {
            var musics = _context.Musics.Include(m => m.Genre);
            // returns all musics
            return View(await musics.ToListAsync());
        }

        // GET: Musics/Create
        [Authorize(Roles = "Artist")]
        public IActionResult Create()
        {
            ViewData["GenreFK"] = new SelectList(_context.Genres.OrderBy(g => g.Title), "Id", "Title");
            return View();
        }

        // POST: Musics/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Artist")]
        public async Task<IActionResult> Create([Bind("Id,Title,ReleaseYear,GenreFK")] Musics music)
        {
            // gets all the information about the logged (authenticated) user
            // string userID = _userManager.GetUserId(User);


            if (ModelState.IsValid)
            {
                _context.Add(music);
                await _context.SaveChangesAsync();

                // save music on one album of this artist

                return RedirectToAction(nameof(Index));
            }

            ViewData["GenreFK"] = new SelectList(_context.Genres.OrderBy(g => g.Title), "Id", "Title");
            return View(music);
        }
    }
}
