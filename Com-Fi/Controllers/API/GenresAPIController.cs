﻿using System;
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
    public class GenresAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GenresAPIController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/GenresAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Genres>>> GetGenres()
        {
            return await _context.Genres.ToListAsync();
        }

        // GET: api/GenresAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Genres>> GetGenre(int id)
        {
            var genre = await _context.Genres.FindAsync(id);

            if (genre == null)
            {
                return NotFound();
            }

            return genre;
        }
    }
}
