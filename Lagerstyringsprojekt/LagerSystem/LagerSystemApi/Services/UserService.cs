using LagerSystemApi.Models.DTO;
using LagerSystemApi.Repository;
using System.Text.RegularExpressions;

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
            try
            {
                string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

                if (!Regex.IsMatch(email, emailPattern)) throw new Exception("Not a valid email");
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
                    throw new Exception($"Error on admin infomation");
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
                    throw new Exception($"Error on user infomation");
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
                if (id <= 0) throw new Exception("Not a valid id");
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
                string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

                if (user == null) throw new Exception("User can not be null");
                if (user.email == null) throw new Exception("Email can not be null");
                if (!Regex.IsMatch(user.email, emailPattern)) throw new Exception("Email is not valid");
                if (string.IsNullOrEmpty(user.first_name)) throw new Exception("First name can not be empty or null");
                if (string.IsNullOrEmpty(user.last_name)) throw new Exception("Last name can not be empty or null");
                if (string.IsNullOrEmpty(user.type)) throw new Exception("User type can not be empty or null");
                if (string.IsNullOrEmpty(user.password)) throw new Exception("User password can not be empty or null");

                // TODO: implement passwordService
                user.salt = _passwordService.GenerateSalt();
                user.password = _passwordService.HashPassword(user.password, user.salt);

                return await _user.Add(user);
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
                if (user == null) throw new Exception("User can not be null");

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
                if (id <= 0) throw new Exception("Id can not be 0 or less");
                await _user.Disable(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
