using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiDemo.Data;
using WebApiDemo.Models;

namespace WebApiDemo.Controllers
{
    [Route("api/v1/User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private DemoDbContext _dbContext;
        public UserController(DemoDbContext dbcontext)
        {
            _dbContext = dbcontext;
        }
        [HttpGet("GetAllUser")]
        public  IActionResult Get()
        {
            /*var user = GetUsers();*/
            try
            {
                var users = _dbContext.Users.ToList();
                if (users.Count == 0)
                {
                    return StatusCode(404, "User Not Found");
                }

                return Ok(users);
            }
            catch (Exception)
            {
                return StatusCode(500, "there is some issue with server");
            }
        }

        [HttpGet("GetUser/{id}")]
        public IActionResult GetUserById([FromRoute]int id)
        {
            try
            {
                var user = _dbContext.Users.FirstOrDefault(x => x.id == id);
                if (user == null)
                {
                    return StatusCode(404, "User Not Found");
                }

                _dbContext.Entry(user).State = EntityState.Unchanged;
                _dbContext.SaveChanges();

                return Ok(user);
            }
            catch (Exception)
            {
                return StatusCode(500, "there is some issue with server");
            }
        }

        [HttpPost("CreateUser")]
        public IActionResult Create([FromBody] UserRequest request)
        {
            Users user = new Users();
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.City = request.City;
            user.State = request.State;
            user.Country = request.Country;

            try
            {
                _dbContext.Users.Add(user);
                _dbContext.SaveChanges();
            }
            catch (Exception)
            {
                return StatusCode(500, "An Error Occurred");
            }

            return Ok("User Created Successfully");
        }

        [HttpPut("UpdateUser")]
        public IActionResult Update([FromBody] UserRequest request)
        {
            try
            {
                var users = _dbContext.Users.FirstOrDefault(x => x.id == request.id);
                if (users == null)
                {
                    return StatusCode(404 ,"User Not Found");
                }
                users.FirstName = request.FirstName;
                users.LastName = request.LastName;
                users.City = request.City;
                users.State = request.State;
                users.Country = request.Country;

                _dbContext.Entry(users).State = EntityState.Modified;
                _dbContext.SaveChanges();

            }
            catch (Exception)
            {

                return StatusCode(500, "There is Some Issue With Server");
            }

            return Ok("User Updated");
        }

        [HttpDelete("deleteUser/{id}")]
        public IActionResult delete([FromRoute]int id)
        {
            try
            {
                var users = _dbContext.Users.FirstOrDefault(x => x.id == id);
                if(users == null)
                {
                    return StatusCode(404, "User Not Found");
                }

                _dbContext.Entry(users).State = EntityState.Deleted;
                _dbContext.SaveChanges();
            }
            catch (Exception)
            {
                return StatusCode(500, "An Error Occurred");
            }

            return Ok("User Deleted Succfully");
        }

  /*      private List<UserRequest> GetUsers()
        {
            return new List<UserRequest>{
                new UserRequest { id=1, FirstName = "ahmed" , LastName = "faisal" , City= "Rawalpindi" , Country ="Pak" , State="Punjab"},
                new UserRequest { id=2,FirstName = "omer", LastName = "faisal" , City="Islamabad" , Country="Pakistan" ,State="Fedral"},
                new UserRequest { id=3, FirstName = "fatima", LastName = "Lol" , City="New York" , Country="USA" , State="New York"}
            };
        }*/
    }
}

