using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    internal class TraineeControllerTests
    {

        #region Index()

        [Test]
        public async Task Index_ReturnsTypeOfListSpartans()
        {

            List<Spartan> spartans = new List<Spartan>();
            spartans.Add(new Spartan
            {
                FirstName = "Gary",
                LastName = "Pints"
            });
            spartans.Add(new Spartan
            {
                FirstName = "Peter",
                LastName = "Bollards"
            });

            // Arrange
            var serviceMockWeek = new Mock<IWeekService>();

            var serviceMockTrainee = new Mock<ITraineeService>();
            serviceMockTrainee.Setup(mock => mock.GetSpartansAsync()).ReturnsAsync(spartans);

            var store = new Mock<IUserStore<Spartan>>();
            var userMgrMock = new Mock<UserManager<Spartan>>(store.Object, null, null, null, null, null, null, null, null);
            userMgrMock.Setup(mock => mock.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(It.IsAny<Spartan>());

            var sut = new TraineesController(serviceMockTrainee.Object, serviceMockWeek.Object, userMgrMock.Object);
            sut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            // Act
            var result = sut.Index().Result;
            var viewResult = (ViewResult)result;
            var viewDataList = (List<Spartan>)viewResult.ViewData.Model;

            // Assert
            Assert.That(viewDataList, Is.TypeOf<List<Spartan>>());
        }

       
        #endregion


    }
}
