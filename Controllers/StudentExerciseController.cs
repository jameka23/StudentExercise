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
    public class StudentExerciseController : ControllerBase
    {
        private readonly IConfiguration _config;

        public StudentExerciseController(IConfiguration config)
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
            using (SqlConnection conn = Connection)
            {
                // open the connection
                using(SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id, StudentId, ExerciseId FROM StudentExercise;";

                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    // create a list of studentexercises 
                    List<StudentExercise> studentExercises = new List<StudentExercise>();

                    while (reader.Read())
                    {
                        // take the resutls from the query and store it
                        StudentExercise studentExercise = new StudentExercise
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            StudentId = reader.GetInt32(reader.GetOrdinal("StudendId")),
                            ExerciseId = reader.GetInt32(reader.GetOrdinal("ExerciseId"))
                        };

                        // add to the list of student exs
                        studentExercises.Add(studentExercise);
                    }

                    // end connection and return list of student exercises
                    reader.Close();
                    return Ok(studentExercises);
                }
            }
        } 

        // GET: api/StudentExercise/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/StudentExercise
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/StudentExercise/5
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
