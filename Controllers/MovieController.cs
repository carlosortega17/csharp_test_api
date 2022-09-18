using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Movpinion_api.Models;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movpinion_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovieController : ControllerBase
    {
        private readonly string TB_NAME = "Movies";
        private readonly ILogger<MovieController> _logger;

        public MovieController(ILogger<MovieController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<MovieModel> Get()
        {
            string query = string.Format("SELECT * FROM {0} ORDER BY id DESC", TB_NAME);
            DataTable moviesData = Database.ExecuteQuery(query, TB_NAME);
            List<MovieModel> movies = new List<MovieModel>();
            foreach (DataRow movie in moviesData.Rows)
            {
                movies.Add(new MovieModel
                {
                    id = int.Parse(movie["id"].ToString()),
                    name = movie["name"].ToString(),
                    description = movie["description"].ToString(),
                });
            }
            return movies;
        }

        [HttpGet("{id}")]
        public dynamic Find(int id)
        {
            string findQuery = string.Format("SELECT * FROM {0} WHERE id={1}", TB_NAME, id);
            DataTable find = Database.ExecuteQuery(findQuery, TB_NAME);
            if (find.Rows.Count <= 0)
                return NotFound(new
                {
                    name = "notFound",
                    message = "Movie not found",
                });            
            return Ok(new
            {
                id = int.Parse(find.Rows[0]["id"].ToString()),
                name = find.Rows[0]["name"].ToString(),
                message = find.Rows[0]["description"].ToString()
            }); ;
        }

        [HttpPost]
        public dynamic Create(MovieModel movie)
        {
            string query = string.Format("INSERT INTO Movies (name, description) VALUES ('{0}', '{1}')", movie.name, movie.description);
            Console.WriteLine(query);
            Database.ExecuteNonQuery(query);
            return NoContent();
        }

        [HttpPut("{id}")]
        public dynamic Update(int id, MovieModel movie)
        {
            string findQuery = string.Format("SELECT * FROM {0} WHERE id={1}", TB_NAME, id);
            DataTable find = Database.ExecuteQuery(findQuery, TB_NAME);
            if (find.Rows.Count <= 0)
                return NotFound(new {
                    name = "notFound",
                    message = "Movie not found",
                });
            string updateQuery = string.Format("UPDATE {0} SET name='{2}', description='{3}' WHERE id={1}", TB_NAME, id, movie.name, movie.description);
            Database.ExecuteNonQuery(updateQuery);
            return Ok(new
            {
                name = "update",
                message = "Movie update successfully"
            }); ;
        }

        [HttpDelete("{id}")]
        public dynamic Delete(int id)
        {
            string findQuery = string.Format("SELECT * FROM {0} WHERE id={1}", TB_NAME, id);
            DataTable find = Database.ExecuteQuery(findQuery, TB_NAME);
            if (find.Rows.Count <= 0)
                return NotFound(new
                {
                    name = "notFound",
                    message = "Movie not found",
                });
            string updateQuery = string.Format("DELETE FROM {0} WHERE id={1}", TB_NAME, id);
            Database.ExecuteNonQuery(updateQuery);
            return Ok(new
            {
                name = "delete",
                message = "Movie delete successfully"
            }); ;
        }
    }
}
