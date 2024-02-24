using Microsoft.AspNetCore.Mvc;
using RequestBalmin.Models;
using RequestBalmin.Service;
using RequestBalmin.Services;
using System.Text.Json;

namespace RequestBalmin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestBalminController(FakeDataService fakeDataService) : ControllerBase
    {

        private readonly FakeDataService _fakeDataService = fakeDataService;

        [HttpGet("dataFake")]
        public IActionResult GetDataFake()
        {
            var fakeName = _fakeDataService.GenerateFakeName();
            var fakeEmail = _fakeDataService.GenerateFakeEmail();
            var fakeUserName = _fakeDataService.GenerateFakeName();

            var fakeData = new
            {
                Name = fakeName,
                UserName = fakeUserName,
                Email = fakeEmail
            };

            return Ok(fakeData);
        }

        [HttpPost("receiveData")]
        public IActionResult ReceiveData([FromBody] JsonElement requestData)
        {
            return Ok(requestData);
        }

        [HttpPost("receiveFile")]
        public IActionResult ReceiveFile([FromForm] RequestFile requestFile)
        {
            var filePath = Path.Combine("Storage", requestFile.File.FileName);

            using Stream fileStream = new FileStream(filePath, FileMode.Create);

            requestFile.File.CopyTo(fileStream);

            return Ok("Arquivo enviado com sucesso!");
        }

        [HttpGet("readFile/{fileName}/{key}/{value}")]
        public IActionResult ReadFile(string fileName, string key, string value)
        {
            string filePath = Path.Combine("Storage", fileName);

            if (System.IO.File.Exists($"{filePath}.json"))
            {
                string jsonContent = System.IO.File.ReadAllText($"{filePath}.json");

                string dataFile = ReadFileService.GetDataOfFile(jsonContent, key, value);

                return Content(dataFile, "application/json");

            }

            return BadRequest("Arquivo não encontrado!");
        }
    }
}
