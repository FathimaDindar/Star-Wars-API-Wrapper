using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Star_Wars_API_Wrapper.Models;
using Star_Wars_API_Wrapper.Service;

namespace Star_Wars_API_Wrapper.Controllers
{
    [ApiController]
    [Route("/")]
    [Tags("Retreive a list of Starships for a given Film")]
    public class getStarshipsByFilmController : Controller
    {
        private readonly IMemoryCache _memoryCache;

        private readonly filmService _filmService;

        public getStarshipsByFilmController(IMemoryCache memoryCache, filmService filmService)
        {
            _memoryCache = memoryCache;
            _filmService = filmService;

        }

        [HttpGet("films/{id}/starships")]
        public async Task<IActionResult> GetStarshipsForFilm(int id)
        {
            try
            {
                // Returns list if the list is already in memory
                if (_memoryCache.TryGetValue($"Film_{id}_Starships", out List<string> starships))
                {
                    return Ok(starships);
                }

                var cacheKey = $"FilmCharacters_{id}";

                var filmsJson = await _filmService.GetSpecifiedJson($"films/{id}/");

                if (filmsJson != null)
                {
                    var film = JsonConvert.DeserializeObject<dynamic>(filmsJson);

                    var starshipsUrls = ((IEnumerable<dynamic>)film.starships).Select(starship => starship.ToString());

                    // Retrieve details for each character
                    starships = new List<string>();
                    foreach (var starshipUrl in starshipsUrls)
                    {
                        var starshipJson = await _filmService.GetSpecifiedJson(starshipUrl);
                        var starship = JsonConvert.DeserializeObject<dynamic>(starshipJson);
                        starships.Add(starship.name.ToString());
                    }

                    _memoryCache.Set($"Film_{id}_Characters", starships, TimeSpan.FromMinutes(5));

                    return Ok(starships);

                }
                else
                {
                    return NotFound($"Film with ID '{id}' not found.");
                }
            }

            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }
}
