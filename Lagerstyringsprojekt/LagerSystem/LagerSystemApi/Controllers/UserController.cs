using LagerSystemApi.Models.DTO;
using LagerSystemApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace LagerSystemApi.Controllers
{
    public interface IUserController
    {
        Task<IActionResult> LogIn(UserLogInDTO logIn);
        Task<IActionResult> Get(int id);
        Task<IActionResult> GetAll();
        Task<IActionResult> Add(UserDTO user);
        Task Update(UpdateUserDTO user);
        Task Disable(int id);
    }
    public class UserController: ControllerBase, IUserController
    {
        private readonly IUserService _user;
        private readonly ILogger<UserController> _logger;
        
        public UserController(IUserService user, ILogger<UserController> logger)
        {
            _user = user;
            _logger = logger;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LogIn([FromBody] UserLogInDTO logIn)
        {
            try
            {
                return Ok(await _user.LogIn(logIn));
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest("Error trying to log in");
            }
        }

        [HttpPost("AdminLogin")]
        public async Task<IActionResult> AdminLogin([FromBody] UserLogInDTO admin)
        {
            try
            {
                return Ok(await _user.AdminLogin(admin));
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest("Error trying to log in");
            }
        }

        [HttpGet("GetUser")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                return Ok(await _user.Get(id));
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest($"Error trying to get user with id: {id}");
            }
        }

        [HttpGet("GetUserIdByEmail")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            try
            {
                return Ok(await _user.GetUserByEmail(email));
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest($"Error trying to get user with email: {email}");
            }
        }

        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(await _user.GetAll());
            }
            catch(Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest("Error retrieving all users");
            }
        }

        [HttpPost("AddUser")]
        public async Task<IActionResult> Add(UserDTO user)
        {
            try
            {
                return Ok(await _user.AddUser(user));
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest("Error adding user");
            }
        }

        [HttpPut("UpdateUser")]
        public async Task Update(UpdateUserDTO user)
        {
            try
            {
                await _user.UpdateUser(user);
                Ok();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                BadRequest("Error updating user");
            }
        }

        [HttpPut("DisableUser")]
        public async Task Disable(int id)
        {
            try
            {
                await _user.Disable(id);
                Ok();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                BadRequest($"Error while disabling user with id: {id}");
            }
        }
    }
}
