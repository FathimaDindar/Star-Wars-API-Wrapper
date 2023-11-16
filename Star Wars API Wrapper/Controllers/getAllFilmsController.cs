using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.OpenApi.Extensions;
using Newtonsoft.Json;
using Star_Wars_API_Wrapper.Models;
using Star_Wars_API_Wrapper.Service;
using System.IO;

namespace Star_Wars_API_Wrapper.Controllers
{
    [ApiController]
    [Route("/")]
    [Tags("Retrieve a List of Star Wars Films")]
    public class getAllFilmsController : Controller
    {

        private readonly IMemoryCache _memoryCache;

        private readonly filmService _filmService;

        public getAllFilmsController(IMemoryCache memoryCache,filmService filmService)
        {
            _memoryCache = memoryCache;
            _filmService = filmService;

        }

        // Retreives a list of Star Wars film titles
        [HttpGet("films")]
        public async Task<IActionResult> GetAllFilms()
        {
            try
            {
                if (_memoryCache.TryGetValue($"Film", out List<string> filmTitles))
                {
                    return Ok(filmTitles);
                }

                var filmsJson = await _filmService.GetSpecifiedJson($"films/");

                if (filmsJson != null)
                {
                    var swapiResponse = JsonConvert.DeserializeObject<dynamic>(filmsJson);

                    // Use dynamic type to navigate the JSON structure
                    filmTitles = new List<string>();
                    foreach (var film in swapiResponse.results)
                    {
                        filmTitles.Add("Episode " + film.episode_id.ToString() + ": " + film.title.ToString());
                    }

                    // Store data in cache for future use
                    _memoryCache.Set($"Film", filmTitles, TimeSpan.FromMinutes(5));

                    return Ok(filmTitles);
                }
                else
                {
                    return NotFound("Failed to retrieve film data.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }


        /*       [HttpGet]
       public async Task<IActionResult> GetAllFilmsResources()
       {
           try
           {
               var filmsJson = await _filmService.GetSomeData();

               if (filmsJson != null)
               {
                   return Content(filmsJson, "application/json");
               }
               else
               {
                   return NotFound("Failed to retrieve film data.");
               }
           }
           catch (Exception ex)
           {
               return StatusCode(500, $"Error: {ex.Message}");
           }
       }

*/
    }
}
