using LagerSystemApi.Controllers;
using LagerSystemApi.Models.DTO;
using Moq;

namespace LagerSystemApi.TestRunner.FeatureTest
{
    public class UserFeatureTest
    {
        
        Mock<IUserController> _user;
        public UserFeatureTest()
        {
            _user = new Mock<IUserController>();
        }

        [Fact]
        public async void t_canuserlogin()
        {
            // Arrange
            UserDTO mockUser = new UserDTO { id = 1, first_name = "john", last_name = "doe", email = "John@zbc.dk", password = "1234!", telephone = "20202020", is_active = true, type = "user" };
            UserLogInDTO mockLogIn = new UserLogInDTO { email = "John@zbc.dk", password = "1234!" };

            _user.Setup(service => service.Add(mockUser));
            _user.Setup(service => service.LogIn(mockLogIn));

            // Act
            _user.Object.Add(mockUser);
            LoggedInDTO logIn = await _user.Object.LogIn(mockLogIn);

            // Assert
            Assert.NotNull(logIn);
            Assert.True(logIn.status_code == 200);
        }

        [Fact]
        public async void t_canyoucreateuser()
        {
            // Arrange
            UserDTO mockUser = new UserDTO { id = 1, first_name = "john", last_name = "doe", email = "John@zbc.dk", password = "1234!", telephone = "20202020", is_active = true, type = "user" };

            _user.Setup(service => service.Add(mockUser));
            _user.Setup(service => service.Get(1));

            // Act
            _user.Object.Add(mockUser);
            UserDTO user = await _user.Object.Get(1);

            // Assert
            Assert.NotNull(user);
            Assert.True(mockUser == user);
        }

        [Fact]
        public async void t_canyuoupdateuser()
        {
            // Arrange
            UserDTO mockUser = new UserDTO { id = 1, first_name = "john", last_name = "doe", email = "John@zbc.dk", password = "1234!", telephone = "20202020", is_active = true, type = "user" };
            UpdateUserDTO mockUpdateUser = new UpdateUserDTO { firstname = "Jens", lastname = "Bondegård", telephone = "10010010", password = "4321!" };

            _user.Setup(service => service.Add(mockUser));
            _user.Setup(service => service.Update(mockUpdateUser));
            _user.Setup(service => service.Get(1));

            // Act
            _user.Object.Add(mockUser);
            _user.Object.Update(mockUpdateUser);
            UserDTO user = await _user.Object.Get(1);

            // Assert
            Assert.NotNull(user);
            Assert.True(mockUpdateUser.firstname == user.first_name && mockUpdateUser.password == user.password);
        }

        [Fact]
        public async void t_canyougetallusers()
        {
            // Arrange
            UserDTO[] mockUsers = new UserDTO[]
            {
                new UserDTO { id = 1, first_name = "john", last_name = "doe", email = "John@zbc.dk", password = "1234!", telephone = "20202020", is_active = true, type = "user" },
                new UserDTO { id = 2, first_name = "poul", last_name = "joe", email = "JohnAdmin@zbc.dk", password = "1234!", telephone = "10101010", is_active = true, type = "admin" }
            };

            _user.Setup(service => service.Add(mockUsers[0]));
            _user.Setup(service => service.GetAll());

            // Act
            for (int i = 0; i < mockUsers.Length; i++)
            {
                _user.Object.Add(mockUsers[i]);
            }
            UserDTO[] users = await _user.Object.GetAll();

            // Assert
            Assert.NotNull(users);
            Assert.True(users.Any(user => user.first_name == "poul"));
        }

        [Fact]
        public async void t_cangetoneuser()
        {
            // Arrange
            UserDTO mockUser = new UserDTO { id = 1, first_name = "john", last_name = "doe", email = "John@zbc.dk", password = "1234!", telephone = "20202020", is_active = true, type = "user" };

            _user.Setup(service => service.Add(mockUser));
            _user.Setup(service => service.Get(mockUser.id));

            // Act
            _user.Object.Add(mockUser);
            UserDTO user = await _user.Object.Get(mockUser.id);

            // Assert
            Assert.NotNull(user);
            Assert.True(mockUser == user);
        }

        [Fact]
        public async void t_candisableauser()
        {
            // Arrange
            UserDTO mockUser = new UserDTO { id = 1, first_name = "john", last_name = "doe", email = "John@zbc.dk", password = "1234!", telephone = "20202020", is_active = true, type = "user" };

            _user.Setup(service => service.Add(mockUser));
            _user.Setup(service => service.Disable(mockUser.id));
            _user.Setup(service => service.Get(mockUser.id));

            // Act
            _user.Object.Add(mockUser);
            _user.Object.Disable(mockUser.id);
            UserDTO user = await _user.Object.Get(mockUser.id);

            // Assert
            Assert.NotNull(user);
            Assert.True(mockUser.is_active == user.is_active);
        }
        
    }
}
