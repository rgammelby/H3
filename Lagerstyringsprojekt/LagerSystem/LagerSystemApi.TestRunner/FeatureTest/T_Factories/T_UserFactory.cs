using System;
using System.Collections.Generic;
using LagerSystemApi.Models.DTO;

namespace LagerSystemApi.TestRunner.FeatureTest.T_Factories
{
    public static class T_UserFactory
    {
        public static List<UserDTO> CreateUsers()
        {
            return new List<UserDTO>
            {
                new UserDTO { id = 1, first_name = "John", last_name = "Doe", email = "John@zbc.dk", password = "1234!", telephone = "20202020", is_active = true, type = "user" },
                new UserDTO { id = 2, first_name = "Poul", last_name = "Joe", email = "JohnAdmin@zbc.dk", password = "1234!", telephone = "10101010", is_active = true, type = "admin" }
            };
        }
    }
}
