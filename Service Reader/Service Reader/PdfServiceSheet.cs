using System;
using System.Windows;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using System.Diagnostics;

namespace Service_Reader
{
    public class PdfServiceSheet
    {
        private Document serviceSheetDoc;
        private ServiceSubmissionModel currentSheet;
        private string COLUMN_ONE_WIDTH = "6.43cm";
        private string COLUMN_TWO_WIDTH = "9.87cm";
        private Color headerGrey = new Color(153, 153, 153);
        private Color entryHeaderGrey = new Color(192, 192, 192);
        private Color timesheetDayGrey = new Color(128, 128, 128);

        private Table jobDetailsTable;
        private Table timesheetTable;

        public Boolean createPdfSheetForSubmission(ServiceSubmissionModel serviceSubmissionSheet)
        {
            Boolean successful = false;
            currentSheet = serviceSubmissionSheet;

            //Check if there is a current sheet
            if (currentSheet == null)
            {
                MessageBox.Show("Error - no sheet selected");
                return false;
            }

            serviceSheetDoc = new Document();

            defineDocumentStyles();
            createHeader();
            createJobDetailsSection();
            createTimesheetSection();

            PdfDocumentRenderer docRenderer = new PdfDocumentRenderer();
            docRenderer.Document = serviceSheetDoc;
            docRenderer.RenderDocument();
            docRenderer.Save("C:\\Users\\Admin\\Desktop\\Test.pdf");
            Process.Start("C:\\Users\\Admin\\Desktop\\Test.pdf");

            successful = true;
            return successful;
        }

        private void createTimesheetSection()
        {
            //Go through all the timesheets and create the table for each.  
            //Title only occurs on first day
            Section currentSection = (Section)serviceSheetDoc.Sections.LastObject;
            timesheetTable = currentSection.AddTable();

            //Setup the table columns and borders
            Column tableColumnn = timesheetTable.AddColumn(COLUMN_ONE_WIDTH);
            tableColumnn = timesheetTable.AddColumn(COLUMN_TWO_WIDTH);
            timesheetTable.Borders.Color = Colors.Black;
            timesheetTable.Borders.Width = 0.25;

            int noOfDays = currentSheet.ServiceTimesheets.Count;
            int counter = 0;

            while (counter < noOfDays)
            {
                if (counter ==0)
                {
                    Row row1Title = timesheetTable.AddRow();
                    row1Title.Cells[0].MergeRight = 1;
                    Paragraph jobDetailsPara = row1Title.Cells[0].AddParagraph();
                    jobDetailsPara.AddFormattedText("T", "allCapsFirstLetter");
                    jobDetailsPara.AddFormattedText("IMESHEET ", "allCapsNextLetter");
                    row1Title.Cells[0].Shading.Color = headerGrey;
                }

                ServiceDayModel currentDay = currentSheet.ServiceTimesheets[counter];
                addDayToTimesheet(currentDay, counter);

                counter = counter + 1;
            } 

        }

        private void addDayToTimesheet(ServiceDayModel currentDay, int dayCounter)
        {
            //Add the header of the day title

            //Day number is counter plus 1
            int dayNumber = dayCounter + 1;

            Row row1Title = timesheetTable.AddRow();
            row1Title.Cells[0].MergeRight = 1;
            Paragraph jobDetailsPara = row1Title.Cells[0].AddParagraph();
            jobDetailsPara.AddFormattedText("D", "allCapsFirstLetter");
            jobDetailsPara.AddFormattedText("AY " + dayNumber, "allCapsNextLetter");
            row1Title.Cells[0].Shading.Color = timesheetDayGrey;

            //Keep the rows together
            row1Title.KeepWith = 14;    

            addLineToTimesheet("Date", currentDay.DtServiceDay.ToShortDateString());
            addLineToTimesheet("Day", currentDay.DtServiceDay.ToString("dddd"));
            addLineToTimesheet("Travel start time", currentDay.TravelStartTime.ToShortTimeString());
            addLineToTimesheet("Arrival time onsite", currentDay.ArrivalOnsiteTime.ToShortTimeString());
            addLineToTimesheet("Departure time from site", currentDay.DepartSiteTime.ToShortTimeString());
            addLineToTimesheet("Travel end time", currentDay.TravelEndTime.ToShortTimeString());
            addLineToTimesheet("Mileage", currentDay.Mileage.ToString());
            addLineToTimesheet("Daily allowance", currentDay.DailyAllowance.ToString());
            addLineToTimesheet("Overnight allowance", currentDay.OvernightAllowance.ToString());
            addLineToTimesheet("Barrier payment", currentDay.BarrierPayment.ToString());
            MessageBox.Show("Remove barrier payment!");
            addLineToTimesheet("Total travel time", currentDay.TotalTravelTime.ToString());
            addLineToTimesheet("Total time onsite", currentDay.TotalTimeOnsite.ToString());
            addLineToTimesheet("Daily report", currentDay.DailyReport);
            addLineToTimesheet("Parts supplied today", currentDay.PartsSupplied);
        }

        private void createHeader()
        {
            //Create the report header inc service sheet number
            Section newSection = serviceSheetDoc.AddSection();
            Table timesheetHeaderTable = newSection.AddTable();

            Column tableColumnn = timesheetHeaderTable.AddColumn("12.11cm");
            tableColumnn = timesheetHeaderTable.AddColumn("4.19cm");

            Row row1 = timesheetHeaderTable.AddRow();
            row1.Cells[0].AddParagraph("SERVICE REPORT AND TIMESHEET");
            row1.Cells[1].AddParagraph("No. " + currentSheet.SubmissionNo);
            timesheetHeaderTable.Style = "timesheetHeader";

            //Add a line break  at the end of the header
            newSection.AddParagraph();
        }

        private void defineDocumentStyles()
        {
            //Change the normal style on the document
            MigraDoc.DocumentObjectModel.Style documentStyle = serviceSheetDoc.Styles["Normal"];
            documentStyle.Font.Name = "Verdana";
            documentStyle.Font.Size = 10;

            //Create a style for the section headers
            MigraDoc.DocumentObjectModel.Style sectionHeaderStyle = serviceSheetDoc.Styles.AddStyle("sectionHeader", "Normal");
            sectionHeaderStyle.Font.Size = 10;
            sectionHeaderStyle.Font.Bold = true;

            MigraDoc.DocumentObjectModel.Style timesheetHeaderStyle = serviceSheetDoc.Styles.AddStyle("timesheetHeader", "Normal");
            timesheetHeaderStyle.Font.Size = 18;

            MigraDoc.DocumentObjectModel.Style allCapsFirstLetterStyle = serviceSheetDoc.Styles.AddStyle("allCapsFirstLetter", "Normal");
            allCapsFirstLetterStyle.Font.Size = 11;
            allCapsFirstLetterStyle.Font.Bold = true;

            MigraDoc.DocumentObjectModel.Style allCapsNextLetterStyle = serviceSheetDoc.Styles.AddStyle("allCapsNextLetter", "Normal");
            allCapsNextLetterStyle.Font.Size = 9;
            allCapsNextLetterStyle.Font.Bold = true;
            
        }

        private void createJobDetailsSection()
        {
            Section currentSection = (Section)serviceSheetDoc.Sections.LastObject;
            jobDetailsTable = currentSection.AddTable();

            //Setup the table columns and borders
            Column tableColumnn = jobDetailsTable.AddColumn(COLUMN_ONE_WIDTH);
            tableColumnn = jobDetailsTable.AddColumn(COLUMN_TWO_WIDTH);
            jobDetailsTable.Borders.Color = Colors.Black;
            jobDetailsTable.Borders.Width = 0.25;

            Row row1Title = jobDetailsTable.AddRow();
            row1Title.Cells[0].MergeRight = 1;
            Paragraph jobDetailsPara = row1Title.Cells[0].AddParagraph();
            jobDetailsPara.AddFormattedText("J", "allCapsFirstLetter");
            jobDetailsPara.AddFormattedText("OB ", "allCapsNextLetter");
            jobDetailsPara.AddFormattedText("D", "allCapsFirstLetter");
            jobDetailsPara.AddFormattedText("ETAILS ", "allCapsNextLetter");
            row1Title.Cells[0].Shading.Color = headerGrey;

            addLineToJobDetails("Customer", currentSheet.Customer);
            addLineToJobDetails("Address Line 1", currentSheet.Address1);
            addLineToJobDetails("Address Line 2", currentSheet.Address2);
            addLineToJobDetails("Town/City", currentSheet.TownCity);
            addLineToJobDetails("Postcode", currentSheet.Postcode);
            addLineToJobDetails("Customer contact", currentSheet.CustomerContact);
            addLineToJobDetails("Customer phone number", currentSheet.CustomerPhone);
            addLineToJobDetails("Machine make and model", currentSheet.MachineMakeModel);
            addLineToJobDetails("Machine serial number", currentSheet.MachineSerial);
            addLineToJobDetails("CNC controller", currentSheet.MachineController);
            addLineToJobDetails("Job start date", currentSheet.JobStart.ToShortDateString());
            addLineToJobDetails("Customer order no.", currentSheet.CustomerOrderNo);
            addLineToJobDetails("MTT job no.", currentSheet.MttJobNumber);
            addLineToJobDetails("Job description", currentSheet.JobDescription);

            //Add a line break between the tables
            currentSection.AddParagraph();
           
        }

        private void addLineToJobDetails(string rowTitle, string rowData)
        {
            Paragraph currentParagraph;
            Row currentRow = jobDetailsTable.AddRow();
            currentParagraph = currentRow.Cells[0].AddParagraph();
            currentParagraph.AddText(rowTitle);
            currentParagraph.Style = "sectionHeader";

            currentParagraph = currentRow.Cells[1].AddParagraph();
            currentParagraph.AddText(rowData);
            currentParagraph.Style = "Normal";
            currentRow.Cells[0].Shading.Color = entryHeaderGrey;
        }

        private void addLineToTimesheet(string rowTitle, string rowData)
        {
            Paragraph currentParagraph;
            Row currentRow = timesheetTable.AddRow();
            currentParagraph = currentRow.Cells[0].AddParagraph();
            currentParagraph.AddText(rowTitle);
            currentParagraph.Style = "sectionHeader";

            currentParagraph = currentRow.Cells[1].AddParagraph();
            currentParagraph.AddText(rowData);
            currentParagraph.Style = "Normal";
            currentRow.Cells[0].Shading.Color = entryHeaderGrey;
        }
    }
}
