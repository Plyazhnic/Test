using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using EXP.Website;
using EXP.Entity;
using EXP.Website.Controllers;
using EXP.Core.Interface;
using NUnit.Framework;
using Moq;


namespace EXP.Website.Tests.Controllers
{
    [TestFixture]
    public class HomeControllerTest
    {
        [SetUp]
        public void Setup()
        {
            userRepository = new Mock<IUserProfileRepository>();
            makeRepository = new Mock<IVehicleMakeRepository>();
            controller = new HomeController();
        }

        private Mock<IUserProfileRepository> userRepository;
        private Mock<IVehicleMakeRepository> makeRepository;

        private HomeController controller;

        [Test]
        public void IndexTest()
        {
            ActionResult result = controller.Index() as ActionResult;
            Assert.IsNotNull(result,"Didn't render action");
        }

        [Test]
        public void ListMakesTest()
        {
            List<VehicleMake> makes = new List<VehicleMake>
                {new VehicleMake{ VehicleMakeID=1,VehicleMake1="make1"}, new VehicleMake{ VehicleMakeID=2,VehicleMake1="make2"}, new VehicleMake{ VehicleMakeID=3,VehicleMake1="make3"}};
            makeRepository.Setup(x => x.ListMakes()).Returns(makes);
            Assert.AreEqual(makes[0].VehicleMake1, "make1");
            Assert.AreEqual(makes[1].VehicleMake1, "make2");
            Assert.AreEqual(3, makes.Count());
        }

        [Test]
        public void RegisterTest()
        {
            bool rezult = true;
            userRepository.Setup(x => x.CreateUser("test", "password", "user@mail.com", "salt", false)).Returns(rezult);
            Assert.AreEqual(rezult, true);
        }

        [Test]
        public void SetProfileTypeTest()
        {
            bool rezult = true;
           // userRepository.Setup(x => x.UpdateProfile("test", 2, "user@mail.com", "firstName", "lastName", null, null, null, null, null, null, false, null, null)).Returns(rezult);
            Assert.AreEqual(rezult, true);
        }

        //[Test]
        //public void SetStatusTest()
        //{
        //    JsonResult res = controller1.SetStatus(2) as JsonResult;
        //    var data = res.Data;
        //    var type = data.GetType();
        //    var Info = type.GetProperty("success");
        //    var value = Info.GetValue(data, null);
        //    Assert.IsNotNull(res);
        //    Assert.AreEqual(false, value);     
        //}

        //[Test]
        //public void EditPersonalAddressTest()
        //{
        //    Models.PersonalAddressModel address = new Models.PersonalAddressModel();
        //    address.City = "London";
        //    address.Address1 = "Street";
        //    address.Zipcode = "";
        //    JsonResult res = controller.EditPersonalAddress(address) as JsonResult;
        //    var data = res.Data;     
        //    var type = data.GetType();     
        //    var Info = type.GetProperty("success");     
        //    var value = Info.GetValue(data, null);
        //    Assert.AreEqual(false, value);
        //}
    }
}
