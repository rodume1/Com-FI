using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Com_Fi.Data;
using Com_Fi.Models;

namespace Com_Fi.Controllers
{
    public class AlbumsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AlbumsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Albums
        public async Task<IActionResult> Index()
        {
              return View(await _context.Albums.ToListAsync());
        }

        // GET: Albums/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Albums == null)
            {
                return NotFound();
            }

            var albums = await _context.Albums
                .FirstOrDefaultAsync(m => m.Id == id);
            if (albums == null)
            {
                return NotFound();
            }

            return View(albums);
        }

        // GET: Albums/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Albums/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,ReleaseYear,Cover")] Albums albums, IFormFile albumCover) {
            if (albumCover == null) {
                albums.Cover = "defaultCover.png";
            } else {
                if (!(albumCover.ContentType == "image/jpeg" || albumCover.ContentType == "image/png")) {
                    // write the error message
                    ModelState.AddModelError("", "Please, provide an image of one of the these types: .jpeg or .png.");
                    // resend control to view, with data provided by user
                    return View(albums);
                } else {
                    // define image name
                    Guid g;
                    g = Guid.NewGuid();
                    string imageName = albums.Id + "_" + g.ToString();
                    string extensionOfImage = Path.GetExtension(albumCover.FileName).ToLower();
                    imageName += extensionOfImage;
                    // add image name to vet data
                    albums.Cover = imageName;
                }
            }

            if (ModelState.IsValid) {
                try {
                    _context.Add(albums);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                } catch(Exception) {
                    ModelState.AddModelError("", "Something went wrong. I can not store data on database");
                    return View(albums);
                }
            }

            // save image file to disk
            // ********************************
            if (albumCover != null)
            {
                // ask the server what address it wants to use
                string addressToStoreFile = _webHostEnvironment.WebRootPath;
                string newImageLocalization = Path.Combine(addressToStoreFile, "Photos");
                // see if the folder 'Photos' exists
                if (!Directory.Exists(newImageLocalization))
                {
                    Directory.CreateDirectory(newImageLocalization);
                }
                // save image file to disk
                newImageLocalization = Path.Combine(newImageLocalization, albums.Cover);
                using var stream = new FileStream(newImageLocalization, FileMode.Create);
                await albumCover.CopyToAsync(stream);
            }

            return View(albums);
        }

        // GET: Albums/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Albums == null)
            {
                return NotFound();
            }

            var albums = await _context.Albums.FindAsync(id);
            if (albums == null)
            {
                return NotFound();
            }
            return View(albums);
        }

        // POST: Albums/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ReleaseYear,Cover")] Albums albums)
        {
            if (id != albums.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(albums);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlbumsExists(albums.Id))
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
            return View(albums);
        }

        // GET: Albums/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Albums == null)
            {
                return NotFound();
            }

            var albums = await _context.Albums
                .FirstOrDefaultAsync(m => m.Id == id);
            if (albums == null)
            {
                return NotFound();
            }

            return View(albums);
        }

        // POST: Albums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Albums == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Albums'  is null.");
            }
            var albums = await _context.Albums.FindAsync(id);
            if (albums != null)
            {
                _context.Albums.Remove(albums);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AlbumsExists(int id)
        {
          return _context.Albums.Any(e => e.Id == id);
        }
    }
}
