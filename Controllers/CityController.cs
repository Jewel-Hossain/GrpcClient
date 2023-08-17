//In the name of Allah

using CityProto;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class CityController : ControllerBase
{
    private readonly CityService.CityServiceClient _client;

    public CityController()
    {
        var channel = GrpcChannel.ForAddress("http://localhost:5090");
        _client = new CityService.CityServiceClient(channel);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<City>> GetCity(int id)
    {
        var response = await _client.GetCityAsync(new GetCityRequest { Id = id });
        return response.City.ToCity();
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<City>>> GetCities()
    {
        var response = await _client.GetCitiesAsync(new GetCitiesRequest());
        return response.Cities.Select(c => c.ToCity()).ToList();
    }

    [HttpPost]
    public async Task<ActionResult<City>> CreateCity(City city)
    {
        var response = await _client.CreateCityAsync(new CreateCityRequest { Name = city.Name });
        return response.City.ToCity();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCity(int id, City city)
    {
        if (id != city.Id) return BadRequest();
        var response = await _client.UpdateCityAsync(new UpdateCityRequest { Id = id, Name = city.Name });
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCity(int id)
    {
        var response = await _client.DeleteCityAsync(new DeleteCityRequest { Id = id });
        return NoContent();
    }
}
