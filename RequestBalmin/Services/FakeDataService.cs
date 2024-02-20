namespace RequestBalmin.Service;
using Bogus;
using System.Text.Json;

public class FakeDataService
{
    private readonly Faker _faker;

    public FakeDataService()
    {
        _faker = new Faker();
    }

    public string GenerateFakeName()
    {
        return _faker.Name.FullName();
    }

    public string GenerateFakeEmail()
    {
        return _faker.Internet.Email();
    }

    public string GenerateFakeUserName()
    {
        return _faker.Internet.UserName();
    }

    public JsonElement GenerateFakeJson()
    {
        var jsonString = @"{
            ""key1"": ""Dados fakes"",
            ""key2"": ""Adicione dados no body para retorno"",
            ""key3"": """ + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss") + @"""
        }";

        using (JsonDocument doc = JsonDocument.Parse(jsonString))
        {
            return doc.RootElement.Clone();
        }
    }
}

