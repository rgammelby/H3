using LagerSystemApi.Models.DTO;
using LagerSystemApi.Repository;

namespace LagerSystemApi.Services
{
    public interface IUserService
    {
        Task<LoggedInDTO> LogIn(UserLogInDTO user);
        Task<UserDTO> Get(int id);
        Task<UserDTO[]> GetAll();
        Task AddUser(UserDTO user);
        Task UpdateUser(UpdateUserDTO user);
        Task Disable(int id);
    }
    public class UserService: IUserService
    {
        IUserRepository _user;
        public UserService(IUserRepository repo)
        {
            _user = repo;
        }
        public Task<LoggedInDTO> LogIn(UserLogInDTO user)
        {
            return null;
        }
        public async Task<UserDTO> Get(int id)
        {
            return await _user.Get(id);
        }
        public async Task<UserDTO[]> GetAll()
        {
            return await _user.GetAll();
        }
        public async Task AddUser(UserDTO user)
        {

            await _user.Add(user);
        }
        public async Task UpdateUser(UpdateUserDTO user)
        {
            await _user.Update(user);
        }
        public async Task Disable(int id)
        {
            await _user.Disable(id);
        }
    }
}
