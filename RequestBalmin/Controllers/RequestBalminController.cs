using Microsoft.AspNetCore.Mvc;
using RequestBalmin.Models;
using RequestBalmin.Service;
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

        [HttpGet("readFile/{fileName}")]
        public IActionResult ReadFile(string fileName, int id)
        {
            string filePath = Path.Combine("Storage", fileName);
            string extension = Path.GetExtension(filePath);

            if (System.IO.File.Exists($"{filePath}.json"))
            {
                string jsonContent = System.IO.File.ReadAllText($"{filePath}.json");

                var json = JsonSerializer.Deserialize<JsonElement[]>(jsonContent);

                var resultElement = json?.FirstOrDefault(element => element.TryGetProperty("id", out var idField) && idField.GetInt32() == id);

                if (resultElement.HasValue)
                {

                    var resultSerial = JsonSerializer.Serialize(resultElement);
                    return Content(resultSerial, "application/json");
                }
            }

            return BadRequest("Arquivo não encontrado!");
        }
    }
}
