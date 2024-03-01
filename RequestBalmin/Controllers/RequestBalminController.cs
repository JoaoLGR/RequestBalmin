using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using RequestBalmin.Models;
using RequestBalmin.Services;
using System.Text.Json;

namespace RequestBalmin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestBalminController() : ControllerBase
    {
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

        [HttpGet("readFile/{fileName}/{key?}/{value?}")]
        public IActionResult ReadFile(string fileName, string? key, string? value)
        {
            string filePath = Path.Combine("Storage", fileName);

            if (System.IO.File.Exists($"{filePath}.json"))
            {
                string jsonContent = System.IO.File.ReadAllText($"{filePath}.json");

                try
                {
                    JToken.Parse(jsonContent);

                    string dataFile = ReadFileService.GetDataOfFile(jsonContent, key, value);

                    if (dataFile == "Não existem dados com essas especificações")
                    {
                        return BadRequest("Não existem dados com essas especificações");
                    }

                    return Content(dataFile, "application/json");
                }
                catch (JsonReaderException ex)
                {
                    return BadRequest($"Erro no processamento do arquivo {fileName}: {ex.Message}");
                }
            }

            return BadRequest("Arquivo não encontrado!");
        }

        [HttpDelete("removeInFile/{fileName}")]
        public IActionResult RemoveInFile(string fileName, string key, string value)
        {
            string filePath = Path.Combine("Storage", fileName);

            if (System.IO.File.Exists($"{filePath}.json"))
            {
                string jsonContent = System.IO.File.ReadAllText($"{filePath}.json");

                try
                {
                    JToken.Parse(jsonContent);

                    string dataFile = ReadFileService.RemoveDataOfFile(jsonContent, key, value);

                    System.IO.File.WriteAllText($"{filePath}.json", dataFile);

                    return Content(dataFile, "application/json");
                }
                catch (JsonReaderException ex)
                {
                    return BadRequest($"Erro no processamento do arquivo {fileName}: {ex.Message}");
                }
            }

            return BadRequest("Arquivo não encontrado!");
        }
    }
}
