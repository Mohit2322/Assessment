using Assessment1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace Assessment1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration iconfig;
        public HomeController(ILogger<HomeController> logger,IConfiguration config)
        {
            _logger = logger;
            iconfig = config;
        }

        public async Task<IActionResult> IndexAsync()
        {
            using (var httpClient = new HttpClient())
            {
                var jsn = await httpClient.GetStringAsync(iconfig.GetValue("url", ""));
                var temp = JsonSerializer.Deserialize<Dictionary<string, ClassModel>>(jsn);
                List<ClassModel> classes = temp.Values.ToList();
                float maxMath1Val = classes.Max(r => r.Math1);
                float minMath1Val = classes.Min(r => r.Math1);
                var tempres = temp.Where(x => x.Value.Math1 == maxMath1Val || x.Value.Math1 == minMath1Val).ToDictionary(x=>x.Key,x=>x.Value);
                ViewBag.result = JsonSerializer.Serialize(tempres);
            }
            return View();
        }

    }
}
