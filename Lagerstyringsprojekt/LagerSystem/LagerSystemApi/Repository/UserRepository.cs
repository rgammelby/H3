using LagerstyringClassLibrary.Models;
using LagerSystemApi.Models.DTO;
using Microsoft.EntityFrameworkCore;
using LagerSystemApi.Services;

namespace LagerSystemApi.Repository
{
    public interface IUserRepository
    {
        //Task<LoggedInDTO> Login(UserLogInDTO user);
        Task<UserDTO> Add(UserDTO user);
        Task<UserDTO> GetUserByEmail(string email);
        Task Update(UpdateUserDTO user);
        Task Disable(int id);
        Task<UserDTO> Get(int id);
        Task<UserDTO[]> GetAll();
    }
    public class UserRepository : IUserRepository
    {
        Context _context;

        public UserRepository(Context db)
        {
            _context = db;
        }
        
        //public async Task<LoggedInDTO> Login(UserLogInDTO user)
        //{
        //    try
        //    {
        //        User newUser = await _context.Users.Where(db => db.email == user.email).FirstOrDefaultAsync();


        //        // Decrypt pass then compare user.password with newUser.hashedpwd, if the same, return 
        //        // Else throw error that says password not correct.
        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception($"Error while logging into user, Email: {user.email}.\nError: {ex.Message}");
        //    }
        //}

        public async Task<UserDTO> Add(UserDTO user)
        {
            try
            {
                User newUser = new User
                {
                    first_name = user.first_name,
                    last_name = user.last_name,
                    email = user.email,
                    salt = user.salt,
                    password = user.password,
                    is_active = user.is_active,
                    telephone = user.telephone,
                    type = user.type
                };

                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error while adding user.\nError: {ex.Message}");
            }
        }

        public async Task Update(UpdateUserDTO user)
        {
            try
            {
                User newUser = await _context.Users.Where(db => db.id == user.id).FirstOrDefaultAsync();

                if (newUser == null) throw new Exception("Could not find user to be updated");

                newUser.first_name = string.IsNullOrEmpty(user.first_name) != true ? user.first_name : newUser.first_name;
                newUser.last_name = string.IsNullOrEmpty(user.last_name) != true ? user.last_name : newUser.last_name;
                newUser.telephone = string.IsNullOrEmpty(user.telephone) != true ? user.telephone: newUser.telephone;

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error while trying to update user: {user.id}.\nError: {ex.Message}");
            }
        }

        public async Task Disable(int id)
        {
            try
            {
                User newUser = await _context.Users.Where(db => db.id == id).FirstOrDefaultAsync();

                newUser.is_active = false;

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error while disabling user_id: {id}.\nError: {ex.Message}");
            }
        }

        public async Task<UserDTO> GetUserByEmail(string email)
        {
            try
            {
                User user = await _context.Users.Where(db => db.email == email).FirstOrDefaultAsync();

                return new UserDTO
                {
                    id = user.id,
                    first_name = user.first_name,
                    last_name = user.last_name,
                    email = user.email,
                    password = user.password,
                    is_active = user.is_active,
                    telephone = user.telephone,
                    type = user.type,
                    salt = user.salt
                };
            } catch (Exception ex)
            {
                throw new Exception($"Unexpected error while getting user by e-mail address: {email}.\nError: {ex.Message}");
            }
        }

        public async Task<UserDTO> Get(int id)
        {
            try
            {
                User user = await _context.Users.Where(db => db.id == id).FirstOrDefaultAsync();

                return new UserDTO
                {
                    id = user.id,
                    first_name = user.first_name,
                    last_name = user.last_name,
                    email = user.email,
                    password = user.password,
                    is_active = user.is_active,
                    telephone = user.telephone,
                    type = user.type,
                    salt = user.salt
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error while getting user from user_id: {id}.\nError: {ex.Message}");
            }
        }

        public async Task<UserDTO[]> GetAll()
        {
            try
            {
                UserDTO[] users = await _context.Users.Select(db => new UserDTO
                {
                    id = db.id,
                    first_name = db.first_name,
                    last_name = db.last_name,
                    email = db.email,
                    is_active = db.is_active,
                    telephone = db.telephone,
                    type = db.type
                }).ToArrayAsync();
                return users;
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error while getting all users.\nError: {ex.Message}");
            }
        }
    }
}
