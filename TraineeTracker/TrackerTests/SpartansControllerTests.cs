using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
using static System.Formats.Asn1.AsnWriter;

namespace TrackerTests
{
    internal class SpartansControllerTests
    {

        #region Index()

        [Test]
        [Category("Happy Path")]
        public void GivenUser_Authorised_Index_ReturnsListOfSpartans()
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

            var serviceMockTrainee = new Mock<ITraineeService>();
            serviceMockTrainee.Setup(mock => mock.GetSpartansAsync()).ReturnsAsync(spartans);
            var serviceMockWeek = new Mock<IWeekService>();

            var store = new Mock<IUserStore<Spartan>>();
            var userMgrMock = new Mock<UserManager<Spartan>>(store.Object, null, null, null, null, null, null, null, null);
            userMgrMock.Setup(mock => mock.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(It.IsAny<Spartan>());

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Role, "Trainer"),
            }, "Trainer"));

            var sut = new SpartansController(serviceMockTrainee.Object, serviceMockWeek.Object, userMgrMock.Object);

            sut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            var result = sut.Index().Result;
            var viewResult = (ViewResult)result;
            var viewDataList = (List<Spartan>)viewResult.ViewData.Model;

            // Assert
            Assert.That(viewDataList[0].FirstName, Is.EqualTo("Gary"));
            Assert.That(viewDataList[1].LastName, Is.EqualTo("Bollards"));
        }

        [Test]
        [Ignore("Authorize Attributes not testable past routing engine level")]
        [Category("Sad Path")]
        public void Verify_Index_IsDecoratedWith_AuthorizeAttribute()
        {
        }

        #endregion

        #region Details()

        [Test]
        [Category("Sad Path")]
        public void Given_SpartansUnpopulated_DetailsReturns_NotFound()
        {
            // Arrange
            var serviceMockTrainee = new Mock<ITraineeService>();
            serviceMockTrainee.Setup(mock => mock.GetSpartansAsync()).ReturnsAsync(new List<Spartan>());
            var serviceMockWeek = new Mock<IWeekService>();

            var store = new Mock<IUserStore<Spartan>>();
            var userMgrMock = new Mock<UserManager<Spartan>>(store.Object, null, null, null, null, null, null, null, null);
            userMgrMock.Setup(mock => mock.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(It.IsAny<Spartan>());

            var sut = new SpartansController(serviceMockTrainee.Object, serviceMockWeek.Object, userMgrMock.Object);

            sut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            // Act
            // Act
            var result = sut.Details(It.IsAny<string>()).Result;
            var viewResult = (NotFoundResult)result;

            // Assert
            Assert.That(viewResult.StatusCode, Is.EqualTo(404));

        }

        [Test]
        [Category("Sad Path")]
        public void Given_SpartansId_WhenSpartanIsNull_DetailsReturns_NotFound()
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
            var serviceMockTrainee = new Mock<ITraineeService>();
            serviceMockTrainee.Setup(mock => mock.GetSpartanByIdAsync(spartans[0].Id)).ReturnsAsync(value: null);
            serviceMockTrainee.Setup(mock => mock.GetSpartansAsync()).ReturnsAsync(spartans);
            var serviceMockWeek = new Mock<IWeekService>();

            var store = new Mock<IUserStore<Spartan>>();
            var userMgrMock = new Mock<UserManager<Spartan>>(store.Object, null, null, null, null, null, null, null, null);
            userMgrMock.Setup(mock => mock.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(It.IsAny<Spartan>());

            var sut = new SpartansController(serviceMockTrainee.Object, serviceMockWeek.Object, userMgrMock.Object);

            sut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            // Act
            var result = sut.Details(spartans[0].Id).Result;
            var viewResult = (NotFoundResult)result;

            // Assert
            Assert.That(viewResult.StatusCode, Is.EqualTo(404));

        }

        [Test]
        [Category("Happy Path")]
        public void Given_SpartansId_DirectsToValidSpartan_DetailsReturns_Spartan()
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
            var serviceMockTrainee = new Mock<ITraineeService>();
            serviceMockTrainee.Setup(mock => mock.GetSpartanByIdAsync(spartans[0].Id)).ReturnsAsync(spartans[0]);
            serviceMockTrainee.Setup(mock => mock.GetSpartansAsync()).ReturnsAsync(spartans);
            var serviceMockWeek = new Mock<IWeekService>();

            var store = new Mock<IUserStore<Spartan>>();
            var userMgrMock = new Mock<UserManager<Spartan>>(store.Object, null, null, null, null, null, null, null, null);
            userMgrMock.Setup(mock => mock.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(It.IsAny<Spartan>());

            var sut = new SpartansController(serviceMockTrainee.Object, serviceMockWeek.Object, userMgrMock.Object);

            sut.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            // Act
            var result = sut.Details(spartans[0].Id).Result;
            var viewResult = (ViewResult)result;
            var viewData = (Spartan)viewResult.ViewData.Model;

            // Assert
            Assert.That(viewData.FirstName, Is.EqualTo("Gary"));
            Assert.That(viewData.LastName, Is.EqualTo("Pints"));

        }

        #endregion
    }
}
