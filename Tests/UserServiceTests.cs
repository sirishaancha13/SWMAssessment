using NSubstitute;
using NUnit.Framework;
using SWM.Assessment.Models;
using SWM.TechnicalAssessment.Services;
using System.Collections.Generic;

namespace SWM.TechnicalAssessment.Tests
{
    public class UserServiceTests
    {
        private IClientService clientService;

        [SetUp]
        public void GivenAListOfAdditionalLoanAccounts()
        {
            clientService = Substitute.For<IClientService>();
        }

        [Test]
        public void GetUserDetails_returns_data()
        {
            var users = new List<User>
            {
                new User {Id=1, First="Name 1", Last="Last 1", Age= 60, Gender= "F" },
                new User {Id=2, First="Name 2", Last="Last 2", Age= 20, Gender= "M"},
                new User {Id=3, First="Name 3", Last="Last 3", Age= 60, Gender= "T"}
            };

            clientService.GetAsync<User>(Arg.Any<string>()).Returns(users);
            var userService = new UserService(clientService);
            var actualResults = userService.GetUserDetails("path", 2, 30);

            Assert.IsNotNull(actualResults);
        }
    }
}
