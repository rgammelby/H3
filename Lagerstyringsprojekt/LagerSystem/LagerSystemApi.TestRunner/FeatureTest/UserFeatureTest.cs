using LagerSystemApi.Controllers;
using LagerSystemApi.Models.DTO;
using LagerSystemApi.TestRunner.FeatureTest.T_Factories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace LagerSystemApi.TestRunner.FeatureTest
{
    public class UserFeatureTest
    {
        private readonly Mock<IUserController> _user;
        private readonly List<UserDTO> _mockUsers;

        public UserFeatureTest()
        {
            _user = new Mock<IUserController>();
            _mockUsers = T_UserFactory.CreateUsers();
        }

        [Fact]
        public async void T_CanUserLogIn()
        {
            // Arrange
            var mockUser = _mockUsers[0];
            var mockLogIn = new UserLogInDTO { email = mockUser.email, password = mockUser.password };
            var mockReturn = new LoggedInDTO { token = "fortyfive", message = "all goodie", status_code = 200 };


            _user.Setup(service => service.Add(mockUser));
            _user.Setup(service => service.LogIn(mockLogIn)).ReturnsAsync(new OkObjectResult(mockReturn));

            // Act
            await _user.Object.Add(mockUser);
            var logIn = await _user.Object.LogIn(mockLogIn);

            // Assert
            var okObject = Assert.IsType<OkObjectResult>(logIn);
            var returnValue = Assert.IsType<LoggedInDTO>(okObject.Value);

            Assert.NotNull(returnValue);
            Assert.Equal(200, returnValue.status_code);
        }

        [Fact]
        public async void T_CanCreateUser()
        {
            // Arrange
            var mockUser = _mockUsers[0];

            _user.Setup(service => service.Add(mockUser));
            _user.Setup(service => service.Get(mockUser.id)).ReturnsAsync(new OkObjectResult(mockUser));

            // Act
            await _user.Object.Add(mockUser);
            var user = await _user.Object.Get(mockUser.id);

            // Assert
            var okObject = Assert.IsType<OkObjectResult>(user);
            var returnValue = Assert.IsType<UserDTO>(okObject.Value);

            Assert.NotNull(returnValue);
            Assert.Equal(mockUser.email, returnValue.email);
        }

        [Fact]
        public async void T_CanUpdateUser()
        {
            // Arrange
            var mockUser = _mockUsers[0];
            var mockUpdateUser = new UpdateUserDTO { first_name = "Jens", last_name = "Bondegård", telephone = "10010010", password = "4321!" };

            _user.Setup(service => service.Add(mockUser));
            _user.Setup(service => service.Update(mockUpdateUser));
            _user.Setup(service => service.Get(mockUser.id)).ReturnsAsync(new OkObjectResult(mockUpdateUser));

            // Act
            await _user.Object.Add(mockUser);
            await _user.Object.Update(mockUpdateUser);
            var user = await _user.Object.Get(mockUser.id);

            // Assert
            var okObject = Assert.IsType<OkObjectResult>(user);
            var returnValue = Assert.IsType<UpdateUserDTO>(okObject.Value);

            Assert.NotNull(user);
            Assert.Equal(mockUpdateUser.first_name, returnValue.first_name);
            Assert.Equal(mockUpdateUser.password, returnValue.password);
        }

        [Fact]
        public async void T_CanReadAllUsers()
        {
            // Arrange
            _user.Setup(service => service.GetAll()).ReturnsAsync(new OkObjectResult(_mockUsers));

            // Act
            var users = await _user.Object.GetAll();

            // Assert
            var okObject = Assert.IsType<OkObjectResult>(users);
            var returnValue = Assert.IsType<List<UserDTO>>(okObject.Value);  // Changed to List<UserDTO>

            Assert.NotNull(returnValue);
            Assert.Contains(returnValue, user => user.first_name == "Poul");
        }

        [Fact]
        public async void T_CanReadUser()
        {
            // Arrange
            var mockUser = _mockUsers[0];

            _user.Setup(service => service.Get(mockUser.id)).ReturnsAsync(new OkObjectResult(mockUser));

            // Act
            var user = await _user.Object.Get(mockUser.id);

            // Assert
            var okObject = Assert.IsType<OkObjectResult>(user);
            var returnValue = Assert.IsType<UserDTO>(okObject.Value);

            Assert.NotNull(returnValue);
            Assert.Equal(mockUser.email, returnValue.email);
        }

        [Fact]
        public async void T_CanDisableUser()
        {
            // Arrange
            var mockUser = _mockUsers[0];
            mockUser.is_active = false;

            _user.Setup(service => service.Disable(mockUser.id));
            _user.Setup(service => service.Get(mockUser.id)).ReturnsAsync(new OkObjectResult(mockUser));

            // Act
            await _user.Object.Disable(mockUser.id);
            var user = await _user.Object.Get(mockUser.id);

            // Assert
            var okObject = Assert.IsType<OkObjectResult>(user);
            var returnValue = Assert.IsType<UserDTO>(okObject.Value);

            Assert.NotNull(returnValue);
            Assert.False(returnValue.is_active);
        }
    }
}
