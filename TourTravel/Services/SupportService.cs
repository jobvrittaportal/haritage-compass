using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using TourTravel.Models;

public class SupportService : ISupportService
{
  private readonly IHttpClientFactory _httpClientFactory;

  public SupportService(IHttpClientFactory httpClientFactory)
  {
    _httpClientFactory = httpClientFactory;
  }

  // -------------------------------------------
  // GENERATE TOKEN
  // -------------------------------------------
  private async Task<string> GetHrlenseToken()
  {
    using var client = _httpClientFactory.CreateClient();

    var response = await client.PostAsync(
        "https://hrms-demo.jobvritta.com/api/DropDown/generateToken?employee_Code=120",
        null
    );

    if (!response.IsSuccessStatusCode)
      throw new Exception("Failed to generate HRMS token");

    var json = await response.Content.ReadFromJsonAsync<JsonElement>();
    return json.GetProperty("token").GetString()!;
  }

  // -------------------------------------------
  // GET APPLICATION DROPDOWN
  // -------------------------------------------
  public async Task<List<ApplicationModel>> GetApplications()
  {
    string token = await GetHrlenseToken();
    using var client = _httpClientFactory.CreateClient();

    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

    var response = await client.GetAsync(
        "https://hrms-demo.jobvritta.com/api/Support/getApplications"
    );

    if (!response.IsSuccessStatusCode)
      return new List<ApplicationModel>();

    var json = await response.Content.ReadAsStringAsync();

    return JsonSerializer.Deserialize<List<ApplicationModel>>(json,
        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
    ) ?? new List<ApplicationModel>();
  }

  // -------------------------------------------
  // GET ASSIGN GROUP DROPDOWN
  // -------------------------------------------
  public async Task<List<AssignGroupModel>> GetAssignGroups()
  {
    string token = await GetHrlenseToken();
    using var client = _httpClientFactory.CreateClient();

    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

    var response = await client.GetAsync(
        "https://hrms-demo.jobvritta.com/api/Support/getAssignGroups"
    );

    if (!response.IsSuccessStatusCode)
      return new List<AssignGroupModel>();

    var json = await response.Content.ReadAsStringAsync();

    return JsonSerializer.Deserialize<List<AssignGroupModel>>(json,
        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
    ) ?? new List<AssignGroupModel>();
  }

  // -------------------------------------------
  // CREATE TICKET
  // -------------------------------------------
  public async Task<bool> CreateTicket(TicketCreateModel model)
  {
    string token = await GetHrlenseToken();
    using var client = _httpClientFactory.CreateClient();

    client.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue("Bearer", token);

    string json = JsonSerializer.Serialize(model);
    var content = new StringContent(json, Encoding.UTF8, "application/json");

    var response = await client.PostAsync(
        "https://hrms-demo.jobvritta.com/api/Support/createTicket",
        content
    );

    var resp = await response.Content.ReadAsStringAsync();
    Console.WriteLine("CreateTicket Response: " + resp);

    return response.IsSuccessStatusCode;
  }
}
