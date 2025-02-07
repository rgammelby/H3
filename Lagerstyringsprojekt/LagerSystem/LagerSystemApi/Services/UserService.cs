using LagerSystemApi.Models.DTO;
using LagerSystemApi.Repository;

namespace LagerSystemApi.Services
{
    public interface IUserService
    {
        Task<LoggedInDTO> AdminLogin(UserLogInDTO admin);
        Task<LoggedInDTO> LogIn(UserLogInDTO user);
        Task<UserDTO> Get(int id);
        Task<UserDTO> GetUserByEmail(string email);
        Task<UserDTO[]> GetAll();
        Task<UserDTO> AddUser(UserDTO user);
        Task UpdateUser(UpdateUserDTO user);
        Task Disable(int id);
    }
    public class UserService : IUserService
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
            try
            {
                return await _user.GetUserByEmail(email);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<LoggedInDTO> AdminLogin(UserLogInDTO admin)
        {
            try
            {
                UserDTO loginAdmin = await GetUserByEmail(admin.email);

                if (loginAdmin == null || !_passwordService.VerifyPassword(admin.password, loginAdmin.password, loginAdmin.salt) || loginAdmin.type != "admin")
                {
                    throw new Exception("E-mail address/password is incorrect, or user does not have admin status. ");
                    //return new LoggedInDTO
                    //{
                    //    token = "hejrune",
                    //    message = "E-mail address/password is incorrect, or user does not have admin status. ",
                    //    status_code = 403
                    //};
                }

                return new LoggedInDTO
                {
                    token = "hejrune",
                    message = "Login successful. ",
                    status_code = 200
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<LoggedInDTO> LogIn(UserLogInDTO user)
        {
            try
            {
                UserDTO loginUser = await GetUserByEmail(user.email);

                if (loginUser == null || !_passwordService.VerifyPassword(user.password, loginUser.password, loginUser.salt))
                {
                    throw new Exception("Login failed. ");
                    //return new LoggedInDTO
                    //{
                    //    token = "hejrune",
                    //    message = "Login failed. ",
                    //    status_code = 403
                    //};
                }

                return new LoggedInDTO
                {
                    token = "hejrune",
                    message = "Login successful. ",
                    status_code = 200
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<UserDTO> Get(int id)
        {
            try
            {
                return await _user.Get(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<UserDTO[]> GetAll()
        {
            try
            {
                return await _user.GetAll();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<UserDTO> AddUser(UserDTO user)
        {
            try
            {
                // TODO: implement passwordService
                user.salt = _passwordService.GenerateSalt();
                user.password = _passwordService.HashPassword(user.password, user.salt);

                await _user.Add(user);
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task UpdateUser(UpdateUserDTO user)
        {
            try
            {
                await _user.Update(user);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task Disable(int id)
        {
            try
            {
                await _user.Disable(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
