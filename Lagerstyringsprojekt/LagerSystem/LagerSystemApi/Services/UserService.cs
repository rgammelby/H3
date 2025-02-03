using LagerSystemApi.Models.DTO;
using LagerSystemApi.Repository;

namespace LagerSystemApi.Services
{
    public interface IUserService
    {
        Task<LoggedInDTO> LogIn(UserLogInDTO user);
        Task<UserDTO> Get(int id);
        Task<UserDTO> GetUserByEmail(string email);   
        Task<UserDTO[]> GetAll();
        Task AddUser(UserDTO user);
        Task UpdateUser(UpdateUserDTO user);
        Task Disable(int id);
    }
    public class UserService: IUserService
    {
        IUserRepository _user;
        PasswordService _passwordService;

        public UserService(IUserRepository repo, PasswordService passwordService)
        {
            _user = repo;
            _passwordService = passwordService;
        }

        public async Task<UserDTO> GetUserByEmail(string email)
        {
            return await _user.GetUserByEmail(email); 
        }

        public async Task<LoggedInDTO> LogIn(UserLogInDTO user)
        {
            Console.WriteLine($"\nUser.email: {user.email}\nUser.password: {user.password}\n");

            UserDTO loginUser = await GetUserByEmail(user.email);
            Console.WriteLine($"\nloginUser.id: {loginUser.id}\n");
            Console.WriteLine($"\nloginUser.password: {loginUser.password}\n");
            Console.WriteLine($"\nloginUser.salt: {loginUser.salt}\n");
            Console.WriteLine($"\nbool: {_passwordService.VerifyPassword(user.password, loginUser.password, loginUser.salt)}");

            if (loginUser == null || !_passwordService.VerifyPassword(user.password, loginUser.password, loginUser.salt))
            {
                return new LoggedInDTO
                {
                    token = "hejrune",
                    message = "Login failed. ",
                    status_code = 403
                };
            }

            return new LoggedInDTO
            {
                token = "hejrune",
                message = "Login successful. ",
                status_code = 200
            };
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
            // TODO: implement passwordService
            user.salt = _passwordService.GenerateSalt();
            user.password = _passwordService.HashPassword(user.password, user.salt);

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
