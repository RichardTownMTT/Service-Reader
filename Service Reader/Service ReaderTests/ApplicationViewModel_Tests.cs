using System;
using Service_Reader;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Service_ReaderTests
{
    [TestClass]
    public class ApplicationViewModel_Tests
    {
        [TestMethod]
        public void CountNumberOfScreens()
        {
            ApplicationViewModel appVM = new ApplicationViewModel();
            Assert.AreEqual(appVM.AllPageViews.Count, 54);

        }
    }
}
