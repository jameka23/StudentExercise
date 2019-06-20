using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StudentExercises.Models;

namespace StudentExercises.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CohortController : ControllerBase
    {
        private readonly IConfiguration _config;

        public CohortController(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }


        public async Task<IActionResult> Get()
        {
            using(SqlConnection conn = Connection)
            {
                // open the connection 
                conn.Open();
                using(SqlCommand cmd = conn.CreateCommand())
                {
                    // create the query
                    cmd.CommandText = @"SELECT Id, CohortName FROM Cohort;";

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    // create a list that will hold the list of cohorts
                    List<Cohort> cohorts = new List<Cohort>();

                    // take the resutls and store in an object of cohort type
                    while (reader.Read())
                    {
                        Cohort cohort = new Cohort
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            CohortName = reader.GetString(reader.GetOrdinal("CohortName"))
                        };

                        // add a cohort to the list 
                        cohorts.Add(cohort);
                    }

                    // close the connection and return the list of cohorts
                    reader.Close();
                    return Ok(cohorts);
                }
            }
        }

        // GET: api/Cohort/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Cohort
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Cohort/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
