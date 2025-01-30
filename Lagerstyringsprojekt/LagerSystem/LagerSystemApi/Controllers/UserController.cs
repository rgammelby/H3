using LagerSystemApi.Data;
using LagerSystemApi.Models.DTO;
using LagerSystemApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace LagerSystemApi.Controllers
{
    public interface IUserController
    {
        Task<LoggedInDTO> LogIn(UserLogInDTO logIn);
        Task<UserDTO> Get(int id);
        Task<UserDTO[]> GetAll();
        void Add(UserDTO user);
        void Update(UpdateUserDTO user);
        void Disable(int id);
    }
    public class UserController: IUserController
    {
        private LagerSystemDbContext _context;
        private IUserService _user;
        
        public UserController(LagerSystemDbContext context, IUserService user)
        {
            _context = context;
            _user = user;
        }

        [HttpGet("Login")]
        public Task<LoggedInDTO> LogIn(UserLogInDTO logIn)
        {
            return _user.LogIn(logIn);
        }

        [HttpGet("GetUser")]
        public async Task<UserDTO> Get(int id)
        {
            return await _user.Get(id);
        }

        [HttpGet("GetAllUsers")]
        public async Task<UserDTO[]> GetAll()
        {
            return await _user.GetAll();
        }

        [HttpPost("AddUser")]
        public void Add(UserDTO user)
        {
            _user.AddUser(user);
        }

        [HttpPut("UpdateUser")]
        public void Update(UpdateUserDTO user)
        {
            _user.UpdateUser(user);
        }

        [HttpPut("DisableUser")]
        public void Disable(int id)
        {
            _user.Disable(id);
        }
    }
}
