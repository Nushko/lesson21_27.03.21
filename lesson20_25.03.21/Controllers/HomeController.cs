using Dapper;
using lesson20_25._03._21.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace lesson20_25._03._21.Controllers
{
	//SQL server опять перестал нормально работать,
    public class HomeController : Controller
    {
		private readonly ILogger<HomeController> _logger;
		private readonly string _conStr = "Data source=localhost; initial catalog=AlifBase; integrated security=true";

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		public async Task<IActionResult> Index()
		{
			var persons = new List<Person>();
			using (IDbConnection conn = new SqlConnection(_conStr))
			{
				persons = (await conn.QueryAsync<Person>("SELECT * FROM Persons")).ToList();
			}
			return View(persons);
		}

		public IActionResult Privacy()
		{
			return View();
		}
		public IActionResult Create()
		{
			return View();	
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
