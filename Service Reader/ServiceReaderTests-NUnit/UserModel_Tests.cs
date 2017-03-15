using System;
using System.Collections.Generic;
using NUnit.Framework;
using System.Text;
using Service_Reader;
using System.Windows.Controls;

namespace ServiceReaderTests_NUnit
{
    [TestFixture]
    class UserModel_Tests
    {
         [Test]
         public void EnteredUsernameMatchesRetrieved()
        {
            //Arrange
            var expectedResult = "ExpectedUsername";
            UserModel um = new UserModel();
            um.Username = "ExpectedUsername";

            //Act
            var result = um.Username;

            //Assert
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void EnteredPasswordMatchesRetrieved()
        {
            var expectedResult = new PasswordBox();
            expectedResult.Password = "PasswordEntered";

            UserModel um = new UserModel();
            um.PasswordBoxEntry = expectedResult;

            var result = um.PasswordBoxEntry;

            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void DbModeSetWorks()
        {
            var expectedResult = UserModel.MODE_DATABASE;
            var um = new UserModel(UserModel.MODE_DATABASE);

            var result = um.Mode;
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void CanvasModeSetWorks()
        {
            var expectedResult = UserModel.MODE_CANVAS;
            var um = new UserModel(UserModel.MODE_CANVAS);

            var result = um.Mode;
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void DbMessageWorks()
        {
            var expectedResult = "Enter your service database username and password";
            var um = new UserModel(UserModel.MODE_DATABASE);

            var result = um.DisplayMessage;
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void CanvasMessageWorks()
        {
            var expectedResult = "Enter your Canvas username and password";
            var um = new UserModel(UserModel.MODE_CANVAS);

            var result = um.DisplayMessage;
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void RetrieveCanvasCredentialsFromCache()
        {
            var expectedResult = new UserModel();
            expectedResult.Username = "Test";
        }

        [Test]
        public void RetrieveDbCredentialsFromCache()
        {

        }


    }
}
