using LagerSystemApi.Models.DTO;

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
        public Task<LoggedInDTO> LogIn(UserLogInDTO user)
        {
            return null;
        }
        public Task<UserDTO> Get(int id)
        {
            return null;
        }
        public Task<UserDTO[]> GetAll()
        {
            return null;
        }
        public Task AddUser(UserDTO user)
        {
            return null;
        }
        public Task UpdateUser(UpdateUserDTO user)
        {
            return null;
        }
        public Task Disable(int id)
        {
            return null;
        }
    }
}
