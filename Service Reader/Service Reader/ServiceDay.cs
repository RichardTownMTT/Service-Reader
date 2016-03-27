using System;
using System.Linq;
using System.Xml.Linq;

namespace Service_Reader
{
    public class ServiceDay
    {
        public DateTime dtServiceDay { get; set; }
        public DateTime travelStartTime { get; set; }
        public DateTime arrivalOnsiteTime { get; set; }
        public DateTime departSiteTime { get; set; }
        public DateTime travelEndTime { get; set; }
        public double mileage { get; set; } = 0;
        public double dailyAllowance { get; set; } = 0;
        public double overnightAllowance { get; set; } = 0;
        public double barrierPayment { get; set; } = 0;
        public double travelTimeToSite { get; set; } = 0;
        public double travelTimeFromSite { get; set; } = 0;
        public double totalTravelTime { get; set; } = 0;
        public double totalTimeOnsite { get; set; } = 0;
        public string dailyReport { get; set; } = "";
        public string partsSupplied { get; set; } = "";

        private static string RESPONSE_GROUP = "ResponseGroup";
        private static string RESPONSES = "Responses";
        private static string SECTION = "Section";
        private static string SCREENS = "Screens";
        private static string SCREEN = "Screen";

        private static string DATE = "Date";
        private static string TRAVEL_START = "Travel time start";
        private static string ARRIVE_ONSITE = "Arrival time on site";
        private static string DEPART_SITE = "Departure time from site";
        private static string TRAVEL_END = "Travel end time";
        private static string MILEAGE = "Mileage";
        private static string DAILY_ALLOWANCE = "Daily allowance";
        private static string OVERNIGHT_ALLOWANCE = "Overnight allowance";
        private static string BARRIER_PAYMENT = "Barrier payment";
        private static string TRAVEL_TO_SITE = "Travel time to site";
        private static string TRAVEL_FROM_SITE = "Travel time from site";
        private static string TOTAL_TRAVEL = "Total travel time";
        private static string TOTAL_TIME_ONSITE = "Total travel time";
        private static string DAILY_REPORT = "Daily report";
        private static string PARTS_SUPPLIED = "Parts supplied today";

        public static ServiceDay[] createDays(XElement allDays)
        {
            int dayCounter = 0;
            int totalDays;
            totalDays = allDays.Descendants(RESPONSE_GROUP).Count();
            ServiceDay[] retval = new ServiceDay[totalDays];

            foreach (XElement responseGroupXml in allDays.Elements())
            {
                ServiceDay dayOfService = new ServiceDay();
                string dtServiceStr = xmlResult(DATE, responseGroupXml);
                dayOfService.dtServiceDay = Convert.ToDateTime(dtServiceStr);
                XElement sectionXml = responseGroupXml.Element(SECTION);
                XElement screensXml = sectionXml.Element(SCREENS);
                XElement screenXml = screensXml.Element(SCREEN);
                XElement responsesXml = screenXml.Element(RESPONSES);

                string travelStartStr = xmlResult(TRAVEL_START, responsesXml);
                dayOfService.travelStartTime = Convert.ToDateTime(travelStartStr);
                string arrivalOnsiteStr = xmlResult(ARRIVE_ONSITE, responsesXml);
                dayOfService.arrivalOnsiteTime = Convert.ToDateTime(arrivalOnsiteStr);
                string departSiteStr = xmlResult(DEPART_SITE, responsesXml);
                dayOfService.departSiteTime = Convert.ToDateTime(departSiteStr);
                string travelEndStr = xmlResult(TRAVEL_END, responsesXml);
                dayOfService.travelEndTime = Convert.ToDateTime(travelEndStr);
                dayOfService.mileage = Convert.ToDouble(xmlResult(MILEAGE, responsesXml));
                dayOfService.dailyAllowance = Convert.ToDouble(xmlResult(DAILY_ALLOWANCE, responsesXml));
                dayOfService.overnightAllowance = Convert.ToDouble(xmlResult(OVERNIGHT_ALLOWANCE, responsesXml));
                dayOfService.barrierPayment = Convert.ToDouble(xmlResult(BARRIER_PAYMENT, responsesXml));
                dayOfService.travelTimeToSite = Convert.ToDouble(xmlResult(TRAVEL_TO_SITE, responsesXml));
                dayOfService.travelTimeFromSite = Convert.ToDouble(xmlResult(TRAVEL_FROM_SITE, responsesXml));
                dayOfService.totalTravelTime = Convert.ToDouble(xmlResult(TOTAL_TRAVEL, responsesXml));
                dayOfService.totalTimeOnsite = Convert.ToDouble(xmlResult(TOTAL_TIME_ONSITE, responsesXml));
                dayOfService.dailyReport = xmlResult(DAILY_REPORT, responsesXml);
                dayOfService.partsSupplied = xmlResult(PARTS_SUPPLIED, responsesXml);

                retval[dayCounter] = dayOfService;
                dayCounter++;
            }

            return retval;
        }

        private static string xmlResult(string label, XElement xmlInput)
        {
            var resultXML = xmlInput.Elements("Response").Where(x => x.Element("Label").Value == label);
            XElement result = resultXML.First();

            string retval = "";

            object resultStr = result.Element("Value").Value;
            if (resultStr != null)
            {
                retval = (string)resultStr;
            }

            return retval;
        }
    }
}
