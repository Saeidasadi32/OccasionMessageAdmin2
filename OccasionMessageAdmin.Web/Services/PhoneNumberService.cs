using SharedComponents.Services;
using SharedFormComponents.Models;
using System.Text.Json;

namespace OccasionMessageAdmin.Web.Services;

public class PhoneNumberService(IHttpClientFactory factory) : IPhoneNumberService
{
    private readonly HttpClient _http = factory.CreateClient("SharedComponents");
    private readonly string _countriesJsonPath = "_content/SharedComponents/data/countries.json";

    public async Task<List<CountryItem>> GetCountriesAsync()
    {
        var json = await _http.GetStringAsync(_countriesJsonPath);
        return JsonSerializer.Deserialize<List<CountryItem>>(json) ?? [];
    }
}
