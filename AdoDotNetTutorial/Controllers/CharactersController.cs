using AdoDotNetTutorial.Data;
using AdoDotNetTutorial.Models;
using Microsoft.AspNetCore.Mvc;

namespace AdoDotNetTutorial.Controllers
{
    [Route("api/[action]")]
    [ApiController]
    public class CharactersController() : ControllerBase()
    {
        [HttpGet]
        public async Task<ActionResult<List<Character>>> GetAllCharacters()
        {
            CharacterDb db = new CharacterDb();
            var chars = await db.GetAllCharacters();

            return Ok(chars);
        }

        [HttpPost]
        public async Task<ActionResult<List<Character>>> GetCharacterById([FromBody] Character character )
        {
            CharacterDb db = new CharacterDb();
            var chars = await db.GetCharacterById(character.Id);

            return Ok(chars);
        }

        [HttpGet("{fromDate}&{toDate}")]
        public async Task<ActionResult<List<Character>>> GetBranchTrnSummaryDataSet(string fromDate, string toDate)
        {
            SposDb db = new SposDb();

            var summary = await db.GetBranchTrnSummaryUsingDataset(fromDate, toDate);

            return Ok(summary);
        }

        [HttpGet]
        public async Task<ActionResult<List<Character>>> GetBranchTrnSummaryReader()
        {
            SposDb db = new SposDb();

            var summary = await db.GetBranchTrnSummaryUsingReader();

            return Ok(summary);
        }

        [HttpGet]
        public async Task<ActionResult<List<Character>>> GetBranchTrnDapper()
        {
            SposDb db = new SposDb();

            var summary = await db.GetAllBranchTrnDapper();

            return Ok(summary);
        }
    }
}
