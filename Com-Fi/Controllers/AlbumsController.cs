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

namespace Com_Fi.Controllers
{
    // [Authorize]
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

            var album = await _context.Albums
                .FirstOrDefaultAsync(m => m.Id == id);
            if (album == null)
            {
                return NotFound();
            }

            return View(album);
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
        public async Task<IActionResult> Create([Bind("Id,Title,ReleaseYear,Cover")] Albums album, IFormFile albumCover) {
            if (albumCover == null) {
                album.Cover = "defaultCover.jpg";
            } else {
                if (!(albumCover.ContentType == "image/jpeg" || albumCover.ContentType == "image/png")) {
                    // write the error message
                    ModelState.AddModelError("", "Please, provide an image of one of the these types: .jpeg or .png.");
                    // resend control to view, with data provided by user
                    return View(album);
                } else {
                    // define image's name
                    Guid g;
                    g = Guid.NewGuid();
                    string imageName = album.Id + "_" + g.ToString();
                    string extensionOfImage = Path.GetExtension(albumCover.FileName).ToLower();
                    imageName += extensionOfImage;
                    // add image name to album's data
                    album.Cover = imageName;
                }
            }

            // validate if data provided by user is good...
            if (ModelState.IsValid)
            {
                // add album data to database
                _context.Add(album);
                // commit
                await _context.SaveChangesAsync();

                // save image file to disk
                // ********************************
                // ask the server what address it wants to use
                if (album.Cover != "defaultCover.jpg")
                {
                    string addressToStoreFile = _webHostEnvironment.WebRootPath;
                    string newImageLocalization = Path.Combine(addressToStoreFile, "Photos/Albums", album.Cover);
                    // save image file to disk
                    using var stream = new FileStream(newImageLocalization, FileMode.Create);
                    await albumCover.CopyToAsync(stream);
                }
                return RedirectToAction(nameof(Index));
            }

            return View(album);
        }

        // GET: Albums/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Albums == null)
            {
                return NotFound();
            }

            var album = await _context.Albums.FindAsync(id);
            if (album == null)
            {
                return NotFound();
            }
            return View(album);
        }

        // POST: Albums/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ReleaseYear,Cover")] Albums album, IFormFile albumCover)
        {
            /*
             * "Clones" the current data on the database to this object.
             * This needs to be done to compare whether the image was or wasn't changed
             */
            var albumData = _context.Albums.AsNoTracking().SingleOrDefault(alb => alb.Id == id);

            if (id != album.Id)
            {
                return NotFound();
            }

            if (albumCover != null)
            {
                if (!(albumCover.ContentType == "image/jpeg" || albumCover.ContentType == "image/png"))
                {
                    // write the error message
                    ModelState.AddModelError("", "Por favor, introduza ficheiros do tipo \".jpeg\" ou \".png.\"");
                    // resend control to view, with data provided by user
                    return View(album);
                } else
                {
                    // define new image's name
                    Guid g;
                    g = Guid.NewGuid();
                    string imageName = album.Id + "_" + g.ToString();
                    string extensionOfImage = Path.GetExtension(albumCover.FileName).ToLower();
                    imageName += extensionOfImage;
                    // add image name to album's data
                    album.Cover = imageName;
                }
            } else
            {
                // if no image was selected on edit, then it is meant to maitain the same image
                album.Cover = albumData.Cover;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(album);
                    await _context.SaveChangesAsync();

                    string addressToStoreFile = _webHostEnvironment.WebRootPath; // where to store the img (general path)

                    // save image file to disk
                    // ********************************
                    // ask the server what address it wants to use
                    if (album.Cover != "defaultCover.jpg" && albumCover != null)
                    {
                        // gets the localization of the "previous" image in order to delete it.
                        // whether the image inserted was the same as before, we delete the image
                        // because there is no way to compare its name with the current one.
                        // (if there is, it is overcomplicated.)
                        string imageLocalization = Path.Combine(addressToStoreFile, "Photos/Albums", albumData.Cover);

                        // deletes "current image" (as in: the image that was stored at the moment of insert or last edited)
                        if (albumData.Cover != "defaultCover.jpg" && System.IO.File.Exists(imageLocalization))
                        {
                            try
                            {
                                System.IO.File.Delete(imageLocalization);
                            }
                            catch (Exception)
                            {

                                throw;
                            }
                        }

                        string newImageLocalization = Path.Combine(addressToStoreFile, "Photos/Albums", album.Cover);
                        // save image file to disk
                        using var stream = new FileStream(newImageLocalization, FileMode.Create);
                        await albumCover.CopyToAsync(stream);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlbumsExists(album.Id))
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

            return View(album);
        }

        // GET: Albums/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Albums == null)
            {
                return NotFound();
            }

            var album = await _context.Albums
                .FirstOrDefaultAsync(m => m.Id == id);
            if (album == null)
            {
                return NotFound();
            }

            return View(album);
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
            var album = await _context.Albums.FindAsync(id);
            if (album != null)
            {
                string addressToStoreFile = _webHostEnvironment.WebRootPath;

                // If, for some reason, one album has the cover as null,
                // sets its cover to the default so the application does not stop working
                if (album.Cover == null)
                {
                    album.Cover = "defaultCover.jpg";
                }

                string imageLocalization = Path.Combine(addressToStoreFile, "Photos/Albums", album.Cover);
                
                if (album.Cover != "defaultCover.jpg" && System.IO.File.Exists(imageLocalization))
                {
                    try
                    {
                        System.IO.File.Delete(imageLocalization);
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
                _context.Albums.Remove(album);
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
