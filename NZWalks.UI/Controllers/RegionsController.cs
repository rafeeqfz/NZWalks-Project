using Microsoft.AspNetCore.Mvc;
using NZWalks.UI.Models;
using NZWalks.UI.Models.DTO;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace NZWalks.UI.Controllers
{
    public class RegionsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public RegionsController(IHttpClientFactory httpClientFactory)
        {
            this._httpClientFactory = httpClientFactory; 
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<RegionsDto> response = new List<RegionsDto>();
            var client = _httpClientFactory.CreateClient();

          var httpResponseMessage=  await client.GetAsync("https://localhost:7188/api/Regions");

            httpResponseMessage.EnsureSuccessStatusCode();

           response.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<RegionsDto>>());

            
            return View(response);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddRegionVewModel model)
        {
            var client = _httpClientFactory.CreateClient();

            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://localhost:7188/api/Regions"),
                Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json")
            };
           var httpResponseMessage= await client.SendAsync(httpRequestMessage);

            httpResponseMessage.EnsureSuccessStatusCode();

          var response= await httpResponseMessage.Content.ReadFromJsonAsync<RegionsDto>();

            if(response != null)
            {
               return RedirectToAction("Index","Regions");
            }

            return View();

        }
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var client = _httpClientFactory.CreateClient();

            var response= await client.GetFromJsonAsync<RegionsDto>($"https://localhost:7188/api/Regions/{id}");

            if(response != null) 
            {
                return View(response);
            }
            return View(null);

        }

        [HttpPost]
        public async Task<IActionResult> Edit(RegionsDto requset)
        {
            var client = _httpClientFactory.CreateClient();

            var httpRequest = new HttpRequestMessage()
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"https://localhost:7188/api/Regions/{requset.Id}"),
                Content = new StringContent(JsonSerializer.Serialize(requset), Encoding.UTF8, "application/json")
            };

           var httpResponseMessage= await client.SendAsync(httpRequest);
                
             httpResponseMessage.EnsureSuccessStatusCode();

            var response = await httpResponseMessage.Content.ReadFromJsonAsync<RegionsDto>();

            if(response != null)
            {
                return RedirectToAction("Edit", "Regions");

            }

            return View();
        }

        public async Task<IActionResult> Delete(RegionsDto requset)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();

                var httpResponseMessage = await client.DeleteAsync($"https://localhost:7188/api/Regions/{requset.Id}");

                httpResponseMessage.EnsureSuccessStatusCode();
                return RedirectToAction("Index", "Regions");
            }
            catch(Exception ex)
            {
                return View("Edit");
            }

        }
    }
}
