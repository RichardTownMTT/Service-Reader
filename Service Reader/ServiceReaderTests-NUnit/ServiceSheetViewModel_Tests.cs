using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Service_Reader;
using Moq;

namespace ServiceReaderTests_NUnit
{
    [TestFixture]
    public class ServiceSheetViewModel_Tests
    {
        [Test]
        public void EngineerFullnameEqualToFirstAndSecond()
        {
            var engFirstName = "Test";
            var engSurname = "McTestface";
            var expectedResult = "Test McTestface";

            var serviceSheetVM = new ServiceSheetViewModel();
            serviceSheetVM.UserFirstName = engFirstName;
            serviceSheetVM.UserSurname = engSurname;
            var result = serviceSheetVM.EngineerFullName;

            Assert.AreEqual(expectedResult, result);
            
        }
        
        [Test]
       
        public void CountNumberOfBarrierPayments()
        {
            var expectedResult = 2;
            var dayOneBP =true;
            var dayTwoBP = true;
            var dayThreeBp = false;

            var serviceSheetVm = new ServiceSheetViewModel();
            var dayOne = new ServiceDayViewModel(new DateTime(), new DateTime(), new DateTime(), new DateTime(),
                0, false, false, dayOneBP, 0, 0, 0, 0, "", "", new DateTime(), serviceSheetVm);

            var dayTwo = new ServiceDayViewModel(new DateTime(), new DateTime(), new DateTime(), new DateTime(),
                0, false, false, dayTwoBP, 0, 0, 0, 0, "", "", new DateTime(), serviceSheetVm);

            var dayThree = new ServiceDayViewModel(new DateTime(), new DateTime(), new DateTime(), new DateTime(),
               0, false, false, dayThreeBp, 0, 0, 0, 0, "", "", new DateTime(), serviceSheetVm);


            serviceSheetVm.AddServiceDayViewModel(dayOne);
            serviceSheetVm.AddServiceDayViewModel(dayTwo);
            serviceSheetVm.AddServiceDayViewModel(dayThree);

            serviceSheetVm.updateAllTimes();
            var result = serviceSheetVm.TotalBarrierPayments;

            Assert.AreEqual(expectedResult, result);
        }


    }
}
