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
    /// <summary>
    /// Web API controller that contains all entrypoints to handle Genres
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class GenresAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GenresAPIController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GET: api/GenresAPI
        /// Returns all genres
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GenresViewModel>>> GetGenres()
        {
            // list of genres. each album follows GenresViewModel structure
            return await _context.Genres
                                .Select(g => new GenresViewModel
                                {
                                    Id = g.Id,
                                    Title = g.Title
                                })
                                .ToListAsync();
        }
    }
}
