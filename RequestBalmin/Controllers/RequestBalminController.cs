using Microsoft.AspNetCore.Mvc;
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
    }
}
