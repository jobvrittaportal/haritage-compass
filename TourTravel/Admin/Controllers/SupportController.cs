using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using TourTravel.Models;

[Route("admin/Support")]
public class SupportController : Controller
{
  private readonly IHttpClientFactory _httpClientFactory;
  private readonly ISupportService _supportService;

  public SupportController(IHttpClientFactory httpClientFactory, ISupportService supportService)
  {
    _httpClientFactory = httpClientFactory;
    _supportService = supportService;
  }

  private string GetDefaultLazyParams()
  {
    var obj = new
    {
      first = 0,
      rows = 25,
      page = 1,
      sortField = "id",
      sortOrder = -1
    };

    return JsonSerializer.Serialize(obj);
  }

  private async Task<string> GetHrlenseToken()
  {
    using var client = _httpClientFactory.CreateClient();

    var empcode = User.FindFirst("EmployeeCode")?.Value;

    if (string.IsNullOrEmpty(empcode))
      throw new Exception("EmployeeCode not found for logged-in user.");

    var response = await client.PostAsync(
        $"https://hrms-demo.jobvritta.com/api/DropDown/generateToken?employee_Code={empcode}",
        null
    );

    if (!response.IsSuccessStatusCode)
      throw new Exception("Failed to generate HRMS token");

    var json = await response.Content.ReadFromJsonAsync<JsonElement>();
    return json.GetProperty("token").GetString()!;
  }


  private async Task<TicketResponse> GetTickets(string? filter = null, string? lazyParams = null, int? ticketId = null)
  {
    string token = await GetHrlenseToken();

    using var client = _httpClientFactory.CreateClient();
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

    if (string.IsNullOrWhiteSpace(lazyParams))
    {
      lazyParams = GetDefaultLazyParams();
    }

    filter ??= "";

    var urlBuilder = new StringBuilder("https://hrms-demo.jobvritta.com/api/Support/getTicket");

    var queryParams = new List<string>();

    if (!string.IsNullOrEmpty(filter))
      queryParams.Add($"filter={Uri.EscapeDataString(filter)}");

    if (!string.IsNullOrEmpty(lazyParams))
      queryParams.Add($"lazyParams={Uri.EscapeDataString(lazyParams)}");

    if (ticketId.HasValue)
      queryParams.Add($"ticket_Id={ticketId}");

    if (queryParams.Count > 0)
      urlBuilder.Append("?" + string.Join("&", queryParams));

    string url = urlBuilder.ToString();

    Console.WriteLine($"Request URL: {url}");

    var response = await client.GetAsync(url);
    var responseContent = await response.Content.ReadAsStringAsync();
    Console.WriteLine($"Response Content: {responseContent}");

    if (!response.IsSuccessStatusCode)
    {
      throw new Exception($"API request failed with status {response.StatusCode}: {responseContent}");
    }

    var options = new JsonSerializerOptions
    {
      PropertyNameCaseInsensitive = true
    };

    var ticketResponse = JsonSerializer.Deserialize<TicketResponse>(responseContent, options);

    if (ticketResponse == null)
      throw new Exception("Failed to parse ticket response");

    return ticketResponse;
  }

  //[HttpGet("")]
  //public async Task<IActionResult> Index(string? filter, string? lazyParams)
  //{
  //  try
  //  {
  //    var tickets = await GetTickets(filter, lazyParams);
  //    return View("~/Views/Admin/Support/Index.cshtml", tickets);
  //  }
  //  catch (Exception ex)
  //  {
  //    ViewBag.ErrorMessage = ex.Message;
  //    return View("~/Views/Admin/Support/Index.cshtml");
  //  }
  //}

  [HttpGet("")]
  public async Task<IActionResult> Index(string? date, string? lazyParams)
  {
    try
    {
      ViewBag.Date = date;

      // Convert date to filter JSON expected by your API
      string filter = "";
      if (!string.IsNullOrEmpty(date))
      {
        var filterObj = new { date = date };
        filter = JsonSerializer.Serialize(filterObj);
      }

      var tickets = await GetTickets(filter, lazyParams);

      return View("~/Views/Admin/Support/Index.cshtml", tickets);
    }
    catch (Exception ex)
    {
      ViewBag.ErrorMessage = ex.Message;
      return View("~/Views/Admin/Support/Index.cshtml");
    }
  }


  [HttpGet("Create")]
  public async Task<IActionResult> Create()
  {
    return View("~/Views/Admin/Support/Create.cshtml");
  }


  // -------------------------------------------
  // SUBMIT CREATE TICKET FORM
  // -------------------------------------------


  [HttpGet("ApplicationGroup")]
  public async Task<IActionResult> GetApplicationGroup()
  {
    try
    {
      string token = await GetHrlenseToken();
      var client = _httpClientFactory.CreateClient();

      client.DefaultRequestHeaders.Authorization =
          new AuthenticationHeaderValue("Bearer", token);

      var response = await client.GetAsync(
          "https://hrms-demo.jobvritta.com/api/DropDown/ApplicationGroup"
      );

      string json = await response.Content.ReadAsStringAsync();

      if (!response.IsSuccessStatusCode)
        return StatusCode((int)response.StatusCode, json);

      return Ok(JsonSerializer.Deserialize<object>(json));
    }
    catch (Exception ex)
    {
      return StatusCode(500, new { error = ex.Message });
    }
  }


  [HttpGet("AssignGroup")]
  public async Task<IActionResult> GetAssignGroup()
  {
    try
    {
      string token = await GetHrlenseToken();

      var client = _httpClientFactory.CreateClient();
      client.DefaultRequestHeaders.Authorization =
          new AuthenticationHeaderValue("Bearer", token);

      var response = await client.GetAsync(
          "https://hrms-demo.jobvritta.com/api/DropDown/supportAssignGroup"
      );

      string json = await response.Content.ReadAsStringAsync();

      if (!response.IsSuccessStatusCode)
        return StatusCode((int)response.StatusCode, json);

      return Ok(JsonSerializer.Deserialize<object>(json));
    }
    catch (Exception ex)
    {
      return StatusCode(500, new { error = ex.Message });
    }
  }

  [HttpPost("Create")]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Create(TicketCreateModel model)
  {
    try
    {
      string token = await GetHrlenseToken();
      var client = _httpClientFactory.CreateClient();

      var formData = new MultipartFormDataContent();

      formData.Add(new StringContent(model.Application_Group_Id.ToString()), "application_Id");
      formData.Add(new StringContent(model.Priority), "priority");
      formData.Add(new StringContent(model.Impact), "impact");
      formData.Add(new StringContent(model.Assigned_Group_Id.ToString()), "assign_group_Id");
      formData.Add(new StringContent(model.Subject), "subject");
      formData.Add(new StringContent(model.Description ?? ""), "desc");

      if (model.Screenshot != null)
      {
        var stream = model.Screenshot.OpenReadStream();
        var fileContent = new StreamContent(stream);
        fileContent.Headers.ContentType =
            new MediaTypeHeaderValue(model.Screenshot.ContentType);

        formData.Add(fileContent, "screenshot", model.Screenshot.FileName);
      }

      client.DefaultRequestHeaders.Authorization =
          new AuthenticationHeaderValue("Bearer", token);

      var response = await client.PostAsync(
          "https://hrms-demo.jobvritta.com/api/Support/CreateTicket",
          formData
      );

      string hrmsResponse = await response.Content.ReadAsStringAsync();

      if (!response.IsSuccessStatusCode)
      {
        return Json(new { success = false, message = hrmsResponse });
      }

      return Json(new
      {
        success = true,
        message = "Ticket Created Successfully",
        response = hrmsResponse
      });
    }
    catch (Exception ex)
    {
      return Json(new { success = false, message = ex.Message });
    }
  }

  [HttpGet("getTicketHistory")]
  public async Task<IActionResult> GetTicketHistory(int ticket_Id)
  {
    try
    {
      string token = await GetHrlenseToken();

      using var client = new HttpClient();
      client.DefaultRequestHeaders.Authorization =
          new AuthenticationHeaderValue("Bearer", token);

      string url = $"https://hrms-demo.jobvritta.com/api/Support/getTicketHistory?ticket_Id={ticket_Id}";

      var response = await client.GetAsync(url);
      string jsonString = await response.Content.ReadAsStringAsync();

      var json = JsonDocument.Parse(jsonString).RootElement;
      return Ok(json);
    }
    catch (Exception ex)
    {
      return BadRequest(new { message = ex.Message });
    }
  }





  [HttpPost("addNote")]
  public async Task<IActionResult> addNote([FromForm] AddNoteDto model)
  {
    try
    {
      string token = await GetHrlenseToken();
      var client = _httpClientFactory.CreateClient();
      var formData = new MultipartFormDataContent();
      formData.Add(new StringContent(model.Ticket_Id.ToString()), "Ticket_Id");
      formData.Add(new StringContent(model.Note_Text), "Note_Text");


      if (model.Attachment != null)
      {
        var stream = model.Attachment.OpenReadStream();
        var fileContent = new StreamContent(stream);
        fileContent.Headers.ContentType = new MediaTypeHeaderValue(model.Attachment.ContentType);

        formData.Add(fileContent, "attachment", model.Attachment.FileName);
      }

      client.DefaultRequestHeaders.Authorization =
          new AuthenticationHeaderValue("Bearer", token);

      var response = await client.PostAsync(
          "https://hrms-demo.jobvritta.com/api/Support/addNote",
          formData
      );

      string hrmsResponse = await response.Content.ReadAsStringAsync();

      if (!response.IsSuccessStatusCode)
        return StatusCode((int)response.StatusCode, hrmsResponse);

      return Ok(new { message = " Created Successfully", response = hrmsResponse });
    }
    catch (Exception ex)
    {
      return StatusCode(500, new { error = ex.Message });
    }
  }

  [HttpGet("getNotes")]
  public async Task<IActionResult> GetNote(int ticket_Id)
  {
    try
    {
      string token = await GetHrlenseToken();

      using var client = new HttpClient();
      client.DefaultRequestHeaders.Authorization =
          new AuthenticationHeaderValue("Bearer", token);

      string url = $"https://hrms-demo.jobvritta.com/api/Support/getNotes?ticket_Id={ticket_Id}";

      var response = await client.GetStringAsync(url);

      var json = JsonDocument.Parse(response).RootElement;

      // CASE 1: HRMS API returns a plain array
      if (json.ValueKind == JsonValueKind.Array)
      {
        return Ok(new
        {
          totalCount = json.GetArrayLength(),
          result = json
        });
      }

      // CASE 2: HRMS API returns { totalCount, result }
      if (json.TryGetProperty("result", out var result))
      {
        return Ok(new
        {
          totalCount = json.GetProperty("totalCount").GetInt32(),
          result = result
        });
      }

      // Invalid unexpected format
      return BadRequest(new { message = "Invalid format received from HRMS API." });
    }
    catch (Exception ex)
    {
      return BadRequest(new { message = ex.Message });
    }
  }

  [HttpGet("NoteFile/{*filename}")]
  public async Task<IActionResult> GetExternalNoteFile(string filename)
  {
    try
    {
      if (string.IsNullOrEmpty(filename))
        return BadRequest("Filename required");

      string token = await GetHrlenseToken();

      string url = $"https://hrms-demo.jobvritta.com/api/Support/NoteFile/{Uri.EscapeDataString(filename)}";

      using var client = new HttpClient();
      client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

      var response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);

      if (!response.IsSuccessStatusCode)
      {
        var msg = await response.Content.ReadAsStringAsync();
        return StatusCode((int)response.StatusCode, msg);
      }

      var stream = await response.Content.ReadAsStreamAsync();
      var contentType = response.Content.Headers.ContentType?.MediaType ?? "application/octet-stream";

      return File(stream, contentType, filename);
    }
    catch (Exception ex)
    {
      return StatusCode(500, new { error = ex.Message });
    }
  }



  [HttpGet("supportStatus")]
  public async Task<IActionResult> getsupportStatus()
  {
    try
    {
      string token = await GetHrlenseToken();
      var client = _httpClientFactory.CreateClient();

      client.DefaultRequestHeaders.Authorization =
          new AuthenticationHeaderValue("Bearer", token);

      var response = await client.GetAsync(
          "https://hrms-demo.jobvritta.com/api/DropDown/supportStatus"
      );

      string json = await response.Content.ReadAsStringAsync();

      if (!response.IsSuccessStatusCode)
        return StatusCode((int)response.StatusCode, json);

      return Ok(JsonSerializer.Deserialize<object>(json));
    }
    catch (Exception ex)
    {
      return StatusCode(500, new { error = ex.Message });
    }
  }

  [HttpPut("changeTicketStatus")]
  public async Task<IActionResult> ChangeTicketStatus([FromBody] ChangeStatusDto model)
  {
    try
    {
      string token = await GetHrlenseToken();
      var client = _httpClientFactory.CreateClient();

      var json = JsonSerializer.Serialize(model);
      var content = new StringContent(json, Encoding.UTF8, "application/json");

      var request = new HttpRequestMessage(HttpMethod.Put,
          "https://hrms-demo.jobvritta.com/api/Support/changeTicketStatus");

      request.Content = content;
      request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

      var response = await client.SendAsync(request);
      string hrmsResponse = await response.Content.ReadAsStringAsync();

      if (!response.IsSuccessStatusCode)
        return StatusCode((int)response.StatusCode, hrmsResponse);

      return Ok(new { message = "Ticket status Updated Successfully", hrmsResponse });
    }
    catch (Exception ex)
    {
      return StatusCode(500, new { error = ex.Message });
    }
  }


  [HttpPut("updateTicket")]
  public async Task<IActionResult> UpdateTicket([FromBody] TicketUpdateDto model)
  {
    try
    {
      // Get token for external API authorization
      string token = await GetHrlenseToken();

      var client = _httpClientFactory.CreateClient();

      // Serialize your update data
      var json = JsonSerializer.Serialize(model);
      var content = new StringContent(json, Encoding.UTF8, "application/json");

      // Create PUT request to external API
      var request = new HttpRequestMessage(HttpMethod.Put, "https://hrms-demo.jobvritta.com/api/Support/updateTicket")
      {
        Content = content
      };

      request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

      var response = await client.SendAsync(request);
      var hrmsResponse = await response.Content.ReadAsStringAsync();

      if (!response.IsSuccessStatusCode)
      {
        // You can parse hrmsResponse for error details if needed
        return StatusCode((int)response.StatusCode, new { error = hrmsResponse });
      }

      return Ok(new { message = "Ticket Updated Successfully", hrmsResponse });
    }
    catch (Exception ex)
    {
      // Log exception ex.Message somewhere if you have logging setup
      return StatusCode(500, new { error = ex.Message });
    }
  }
  [HttpGet("Edit/{ticketNumber}")]
  public async Task<IActionResult> Edit(int ticketNumber)
  {
    try
    {
      // Fetch tickets filtered by ticket_Number
      var ticketsResponse = await GetTickets(filter: $"ticket_Number={ticketNumber}");

      if (ticketsResponse == null || ticketsResponse.result == null || ticketsResponse.result.Count == 0)
        return NotFound("Ticket not found");

      var ticket = ticketsResponse.result.FirstOrDefault();

      if (ticket == null)
        return NotFound("Ticket not found");

      // Map Ticket to TicketCreateModel
      var ticketCreateModel = new TicketCreateModel
      {
        Ticket_Id = int.TryParse(ticket.ticket_Id, out var tid) ? tid : 0,  // ticket_Id is string in Ticket, so try parse
        Ticket_Number = ticket.ticket_Number,
        Application_Group_Id = ticket.application_Group_Id,
        Assigned_Group_Id = ticket.assigned_Group_Id,
        Priority = ticket.priority,
        Impact = ticket.impact,
        Subject = ticket.subject,
        Description = ticket.description
      };

      return View("~/Views/Admin/Support/Edit.cshtml", ticketCreateModel);
    }
    catch (Exception ex)
    {
      return BadRequest(ex.Message);
    }
  }

  [HttpGet("getTicketDetails")]
  public async Task<IActionResult> getTicketDetails(int ticket_Number)
  {
    try
    {
      var ticketResponse = await GetTickets(ticketId: ticket_Number);

      var ticket = ticketResponse.result?.FirstOrDefault();

      if (ticket == null)
        return NotFound(new { message = "Ticket not found" });

      return Ok(ticket);
    }
    catch (Exception ex)
    {
      return BadRequest(new { message = ex.Message });
    }
  }








  public class AddNoteDto
  {
    public int Ticket_Id { get; set; }
    public string Note_Text { get; set; }
    public IFormFile? Attachment { get; set; }
  }


  public class TicketUpdateDto
  {
    public int Ticket_Id { get; set; }
    public int Application_Id { get; set; }
    public int Assign_Group_Id { get; set; }
    public string Priority { get; set; }
    public string Impact { get; set; }
    public string Subject { get; set; }
    public string Desc { get; set; }
    public List<TicketChangeDto> Changes { get; set; }
  }

  public class TicketChangeDto
  {
    public string Field_Name { get; set; }
    public string Old_Value { get; set; }
    public string New_Value { get; set; }
  }

  public class ChangeStatusDto
  {
    public int Ticket_Id { get; set; }
    public int Status { get; set; }
    public string Remark { get; set; }
  }

}
