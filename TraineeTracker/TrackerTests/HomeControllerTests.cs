using Castle.Core.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TraineeTrackerApp.Controllers;
using TraineeTrackerApp.Models;
using TraineeTrackerApp.Services;

namespace TrackerTests
{
    internal class HomeControllerTests
    {

        #region Index()

        [Test]
        public void GivenUser_IsInRoleTrainee_Index_ReturnsRedirect()
        {
            var serviceMock = new Mock<ITraineeService>();
            var loggerMock = new Mock<ILogger<HomeController>>();

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Role, "Trainee"),
            }, "Trainee"));

            var sut = new HomeController(loggerMock.Object, serviceMock.Object);
            sut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            var result = sut.Index();
            var resultType = (RedirectToActionResult)result;

            // Assert
            Assert.That(resultType.ActionName, Is.EqualTo("Index"));
            Assert.That(resultType.ControllerName, Is.EqualTo("Weeks"));
        }
    }
}
        #endregion