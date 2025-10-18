using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using learnnet_api.Models.DTOs;
using learnnet_api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using learnnet_api.Data;

namespace learnnet_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        public UsersController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            return Ok(new { message = "GetUsers endpoint hit" });
        }

        //Create User Account
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDTO createUserDto)
        {
            try
            {
                //if (!ModelState.IsValid)
                //    return BadRequest(ModelState);

                if (dbContext.Users.Any(u => u.Email == createUserDto.Email))
                    return Conflict(new { error = "A user with this email already exists." });

                var newUser = new User()
                {
                    FirstName = createUserDto.FirstName,
                    LastName = createUserDto.LastName,
                    Email = createUserDto.Email,
                    PasswordHash = "",
                };

                var passwordHasher = new PasswordHasher<User>();
                newUser.PasswordHash = passwordHasher.HashPassword(newUser, createUserDto.Password);

                dbContext.Users.Add(newUser);
                dbContext.SaveChanges();

                var response = new UserResponseDTO
                {
                    Id = newUser.Id,
                    Email = newUser.Email,
                    FullName = $"{newUser.FirstName} {newUser.LastName}"
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while creating the user.", details = ex.Message });
            }
        }
    }
}
