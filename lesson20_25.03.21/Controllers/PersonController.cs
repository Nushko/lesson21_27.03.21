using Dapper;
using lesson20_25._03._21.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace lesson20_25._03._21.Controllers
{
    public class PersonController : Controller
    {
        private readonly ILogger<PersonController> _logger;
        private readonly string connectionString;
        private readonly IConfiguration _configuration;

        public PersonController(ILogger<PersonController> logger, IConfiguration configuration)
        {
            _configuration = configuration;
            _logger = logger;
            connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var people = new List<Person>();
            using (var conn = new SqlConnection(connectionString))
                people = (await conn.QueryAsync<Person>("SELECT * FROM Persons;")).ToList();

            return View(people);
        }

        //Create Person

        [HttpPost]
        public async Task<IActionResult> Create(Person person)
        {
            if (person != null)
                using (var conn = new SqlConnection(connectionString))
                {
                    await conn.ExecuteAsync(@"INSERT INTO Persons(LastName, FirstName, MiddleName)
                                          VALUES( @LastName, @FirstName, @MiddleName);", person);
                }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Create() => View();


        //FindById

        [HttpGet]
        public async Task<IActionResult> FindById(int id)
        {
            if (id > 0)
            {
                var people = new List<Person>();
                using (IDbConnection conn = new SqlConnection(connectionString))
                    people = (await conn.QueryAsync<Person>($"SELECT * FROM Persons WHERE Id = {id};")).ToList();
                return View("Index", people);
            }

            return RedirectToAction("Index");
        }


        //FindByName

        [HttpGet]
        public async Task<IActionResult> FindByName(string fullName)
        {
            if (fullName != "")
            {
                var people = new List<Person>();
                using (IDbConnection conn = new SqlConnection(connectionString))
                    people = (await conn.QueryAsync<Person>(@$"SELECT * FROM Persons
                                                           WHERE (LastName+MiddleName+FirstName)
                                                           LIKE '%{fullName}%';")).ToList();
                return View("Index", people);
            }
            return View("Index");
        }
    }
}
