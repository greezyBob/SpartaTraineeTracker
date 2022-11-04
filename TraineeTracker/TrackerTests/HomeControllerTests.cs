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
        [Category("Happy Path")]
        public void GivenUser_IsInRoleTrainee_Index_ReturnsRedirect_Weeks()
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

        [Test]
        [Category("Happy Path")]
        public void GivenUser_IsInRoleTrainer_Index_ReturnsRedirect_Trainees()
        {
            var serviceMock = new Mock<ITraineeService>();
            var loggerMock = new Mock<ILogger<HomeController>>();

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Role, "Trainer"),
            }, "Trainer"));

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
            Assert.That(resultType.ControllerName, Is.EqualTo("Trainees"));
        }

        [Test]
        [Category("Happy Path")]
        public void GivenUser_IsInRoleAdmin_Index_ReturnsRedirect_Spartans()
        {
            var serviceMock = new Mock<ITraineeService>();
            var loggerMock = new Mock<ILogger<HomeController>>();

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Role, "Admin"),
            }, "Admin"));

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
            Assert.That(resultType.ControllerName, Is.EqualTo("Spartans"));
        }

        [Test]
        [Category("Happy Path")]
        public void GivenUser_IsRoleless_Index_Returns_TypeOfActionResult()
        {
            var serviceMock = new Mock<ITraineeService>();
            var loggerMock = new Mock<ILogger<HomeController>>();

            var sut = new HomeController(loggerMock.Object, serviceMock.Object);

            sut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            // Act
            var result = sut.Index();

            // Assert
            Assert.That(result, Is.TypeOf(typeof(ViewResult)));
        }

        #endregion

        #region Privacy()

        [Test]
        [Category("Happy Path")]
        public void Privacy_ReturnsTypeOfView()
        {
            var serviceMock = new Mock<ITraineeService>();
            var loggerMock = new Mock<ILogger<HomeController>>();

            var sut = new HomeController(loggerMock.Object, serviceMock.Object);

            sut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            // Act
            var result = sut.Privacy();

            // Assert
            Assert.That(result, Is.TypeOf(typeof(ViewResult)));
        }

        #endregion

        #region Error()

        [Test]
        [Category("Happy Path")]
        public void Error_ReturnsTypeOf_ErrorViewModel()
        {
            var serviceMock = new Mock<ITraineeService>();
            var loggerMock = new Mock<ILogger<HomeController>>();

            var sut = new HomeController(loggerMock.Object, serviceMock.Object);

            sut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            // Act
            var result = sut.Error();
            var viewResult = (ViewResult)result;
            var viewData = viewResult.ViewData.Model;

            // Assert
            Assert.That(viewData, Is.TypeOf(typeof(ErrorViewModel)));
        }

        #endregion
    }
}