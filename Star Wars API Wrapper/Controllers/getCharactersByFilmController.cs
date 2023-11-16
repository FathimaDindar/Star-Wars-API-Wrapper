using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Star_Wars_API_Wrapper.Service;

namespace Star_Wars_API_Wrapper.Controllers
{
    [ApiController]
    [Route("/")]
    [Tags("Retrieve a list of Characters for a given Film")]
    public class getCharactersByFilmController : Controller
    {
        private readonly IMemoryCache _memoryCache;

        private readonly filmService _filmService;

        public getCharactersByFilmController(IMemoryCache memoryCache, filmService filmService)
        {
            _memoryCache = memoryCache;
            _filmService = filmService;

        }


        //Retreives a list of characters in a film specified by the id
        [HttpGet("films/{id}/characters")]
        public async Task<IActionResult> GetCharactersForFilm(int id)
        {
            try
            {
                // Returns list if the list is already in memory
                if (_memoryCache.TryGetValue($"Film_{id}_Characters", out List<string> characters))
                {
                    return Ok(characters);
                }

                // Cache miss: fetch data from the external API
                var filmsJson = await _filmService.GetSpecifiedJson($"films/{id}/");

                if (filmsJson != null)
                {
                    var film = JsonConvert.DeserializeObject<dynamic>(filmsJson);
                    var charactersUrls = ((IEnumerable<dynamic>)film.characters).Select(character => character.ToString());

                    // Retrieve details for each character
                    characters = new List<string>();
                    foreach (var characterUrl in charactersUrls)
                    {
                        var characterJson = await _filmService.GetSpecifiedJson(characterUrl);
                        var character = JsonConvert.DeserializeObject<dynamic>(characterJson);
                        characters.Add(character.name.ToString());
                    }

                    // Store data in cache for future use
                    _memoryCache.Set($"Film_{id}_Characters", characters, TimeSpan.FromMinutes(5));

                    return Ok(characters);
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
