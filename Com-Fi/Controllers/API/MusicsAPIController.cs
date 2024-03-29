﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Com_Fi.Data;
using Com_Fi.Models;
using System.Text;
using System.Net;

namespace Com_Fi.Controllers.API
{
    /// <summary>
    /// Web API controller that contains all entrypoints to handle Musics
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MusicsAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MusicsAPIController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GET: api/MusicsAPI
        /// Returns all musics
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MusicsViewModel>>> GetMusics()
        {
            // list of musics. each music follows MusicsViewModel structure
            return await _context.Musics
                                .Select(m => new MusicsViewModel
                                {
                                    Id = m.Id,
                                    Title = m.Title,
                                    ReleaseYear = m.ReleaseYear, 
                                    Genre = _context.Genres.Where(g => g.Id == m.GenreFK).FirstOrDefault(),
                                })
                                .ToListAsync();
        }

        // POST: api/MusicsAPI
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Musics>> PostMusic([FromForm] Musics music)
        {
            if (music.Title == null || music.Title.Trim() == "")
            {
                return BadRequest("Título Inválido");
            }

            if (music.ReleaseYear < 0 || music.ReleaseYear > DateTime.Now.Year)
            {
                return BadRequest("Ano de lançamento inválido");
            }

            var genre = await _context.Genres.Where(g => g.Id == music.GenreFK).FirstOrDefaultAsync();
            if (genre == null)
            {
                return BadRequest("Género inválido");
            }

            try
            {
                _context.Musics.Add(music);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return BadRequest("Não foi possível criar música");
            }

            return CreatedAtAction("GetMusics", new { id = music.Id }, music);
        }
    }
}
