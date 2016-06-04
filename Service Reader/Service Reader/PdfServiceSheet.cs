using System;
using System.Windows;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using System.Diagnostics;
using System.Reflection;
using System.IO;
using System.Drawing;

namespace Service_Reader
{
    public class PdfServiceSheet
    {
        private Document serviceSheetDoc;
        private ServiceSubmissionModel currentSheet;
        private string COLUMN_ONE_WIDTH = "6.43cm";
        private string COLUMN_TWO_WIDTH = "9.87cm";
        private MigraDoc.DocumentObjectModel.Color headerGrey = new MigraDoc.DocumentObjectModel.Color(191, 191, 191);
        private MigraDoc.DocumentObjectModel.Color entryHeaderGrey = new MigraDoc.DocumentObjectModel.Color(242, 242, 242);
        private MigraDoc.DocumentObjectModel.Color timesheetDayGrey = new MigraDoc.DocumentObjectModel.Color(217, 217, 217);
        private MigraDoc.DocumentObjectModel.Color tableBorderColour = new MigraDoc.DocumentObjectModel.Color(191, 191, 191);
        private double borderWidth = 0.25;

        private Table jobDetailsTable;
        private Table timesheetTable;
        private Table serviceReportTable;
        private Table followupTable;
        private Table signoffTable;

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
            createSheetTitle();
            createJobDetailsSection();
            createTimesheetSection();
            createServiceReportSection();
            createFollowupSection();
            createSignoffSection();
            createFooter();
            createHeader();

            PdfDocumentRenderer docRenderer = new PdfDocumentRenderer();
            docRenderer.Document = serviceSheetDoc;
            docRenderer.RenderDocument();
            docRenderer.Save("C:\\Users\\Admin\\Desktop\\Test.pdf");
            Process.Start("C:\\Users\\Admin\\Desktop\\Test.pdf");

            successful = true;
            return successful;
        }

        private void createHeader()
        {
            Section currentSection = (Section)serviceSheetDoc.Sections.LastObject;
            HeaderFooter header = currentSection.Headers.Primary;
            Paragraph headerPara = header.AddParagraph("Report No. " + currentSheet.SubmissionNo);
            headerPara.Format.Alignment = ParagraphAlignment.Right;
            header.Style = "Normal";
        }

        private void createFooter()
        {
            Section currentSection = (Section)serviceSheetDoc.Sections.LastObject;
            Table footerTable = currentSection.Footers.Primary.AddTable();
            footerTable.AddColumn("4cm");
            footerTable.AddColumn("10cm");
            footerTable.AddColumn("4cm");

            Row footerRow1 = footerTable.AddRow();
            footerTable.Style = "footerStyle";
            Paragraph addressParagraph = footerRow1.Cells[1].AddParagraph();
            addressParagraph.AddText("Machine Tool Technologies Ltd , 1H Ribble Court, 1 Meadway");

            Row footerRow2 = footerTable.AddRow();
            Paragraph addressPara2 = footerRow2.Cells[1].AddParagraph();
            addressPara2.AddText("Shuttleworth Mead Business Park, Padiham, Lancashire, BB12 7NG, UK");

            Row footerRow3 = footerTable.AddRow();
            Paragraph addressPara3 = footerRow3.Cells[1].AddParagraph();
            addressPara3.AddText("Tel. +44 (0) 845 077 9345  Fax. +44 (0) 1282 779 615  Web Address: www.mtt.uk.com");

            //Merge the first cell, to contain the logo
            footerRow1.Cells[0].MergeDown = 2;
            footerRow1.Cells[2].MergeDown = 2;

            Paragraph pageNumberParagraph = footerRow1.Cells[2].AddParagraph();
            pageNumberParagraph.AddPageField();
            pageNumberParagraph.AddText(" (");
            pageNumberParagraph.AddNumPagesField();
            pageNumberParagraph.AddText(")");

            footerRow1.Cells[2].Style = "Normal";

            Image img = Service_Reader.Properties.Resources.MTTFooterLogo;

            byte[] byteArray = new byte[0];
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                stream.Close();

                byteArray = stream.ToArray();
            }

            string imageFileName = migradocFilenameFromByteArray(byteArray);
            footerRow1.Cells[0].AddImage(imageFileName);

            currentSection.Footers.FirstPage = currentSection.Footers.Primary.Clone();

        }

        private static string migradocFilenameFromByteArray(byte[] selectedImage)
        {
            //Change the selected image to a string
            return "base64:" + Convert.ToBase64String(selectedImage);
        }

        private static byte[] loadImage(string imageName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream(imageName))
            {
                if (stream == null)
                {
                    throw new ArgumentException("No resource with name " + imageName);
                }

                int count = (int)stream.Length;
                
                byte[] data = new byte[count];
                stream.Read(data, 0, count);
                return data;
            }
        }

        private void createSignoffSection()
        {
            Section currentSection = (Section)serviceSheetDoc.Sections.LastObject;
            signoffTable = currentSection.AddTable();

            //Setup the table columns and borders
            Column tableColumnn = signoffTable.AddColumn(COLUMN_ONE_WIDTH);
            tableColumnn = signoffTable.AddColumn(COLUMN_TWO_WIDTH);
            signoffTable.Borders.Color = tableBorderColour;
            signoffTable.Borders.Width = borderWidth;

            Row row1Title = signoffTable.AddRow();
            row1Title.Cells[0].MergeRight = 1;
            Paragraph signoffPara = row1Title.Cells[0].AddParagraph();
            signoffPara.AddFormattedText("J", "allCapsFirstLetter");
            signoffPara.AddFormattedText("OB ", "allCapsNextLetter");
            signoffPara.AddFormattedText("S", "allCapsFirstLetter");
            signoffPara.AddFormattedText("IGNOFF", "allCapsNextLetter");
            row1Title.Cells[0].Shading.Color = headerGrey;

            row1Title.KeepWith = 6;
            Row row2Title = signoffTable.AddRow();
            row2Title.Cells[0].MergeRight = 1;
            Paragraph signoffParaCertify = row2Title.Cells[0].AddParagraph();
            signoffParaCertify.AddText("I hereby certify that the service work has been carried out to my satisfaction");
            row2Title.Cells[0].Shading.Color = entryHeaderGrey;

            addLineToSignoffTable("Customer signature", currentSheet.CustomerSignatureUrl);
            addLineToSignoffTable("Customer name", currentSheet.CustomerSignName);
            addLineToSignoffTable("Date", currentSheet.DtSigned.ToShortDateString());
            addLineToSignoffTable("MTT engineer signature", currentSheet.MttEngSignatureUrl);
            addLineToSignoffTable("MTT engineer", currentSheet.EngineerFullName);
        }

        private void addLineToSignoffTable(string rowTitle, string rowData)
        {
            Paragraph currentParagraph;
            Row currentRow = signoffTable.AddRow();
            currentParagraph = currentRow.Cells[0].AddParagraph();
            currentParagraph.AddText(rowTitle);
            currentParagraph.Style = "sectionHeader";

            currentParagraph = currentRow.Cells[1].AddParagraph();
            currentParagraph.AddText(rowData);
            currentParagraph.Style = "Normal";
            currentRow.Cells[0].Shading.Color = entryHeaderGrey;
        }

        private void createFollowupSection()
        {
            Section currentSection = (Section)serviceSheetDoc.Sections.LastObject;
            followupTable = currentSection.AddTable();

            //Setup the table columns and borders
            Column tableColumnn = followupTable.AddColumn(COLUMN_ONE_WIDTH);
            tableColumnn = followupTable.AddColumn(COLUMN_TWO_WIDTH);
            followupTable.Borders.Color = tableBorderColour;
            followupTable.Borders.Width = borderWidth;

            Row row1Title = followupTable.AddRow();
            row1Title.Cells[0].MergeRight = 1;
            Paragraph followupPara = row1Title.Cells[0].AddParagraph();
            followupPara.AddFormattedText("F", "allCapsFirstLetter");
            followupPara.AddFormattedText("OLLOW-UP ", "allCapsNextLetter");
            followupPara.AddFormattedText("W", "allCapsFirstLetter");
            followupPara.AddFormattedText("ORK", "allCapsNextLetter");
            row1Title.Cells[0].Shading.Color = headerGrey;

            row1Title.KeepWith = 9;

            addLineToFollowupTable("Additional faults found", currentSheet.AdditionalFaultsFound);
            addLineToFollowupTable("Parts required", currentSheet.PartsForFollowup);
            addLineToFollowupTable("Quote required", currentSheet.QuoteRequired.ToString());

            Row rowImages= followupTable.AddRow();
            rowImages.Cells[0].MergeRight = 1;
            Paragraph followupImagesPara = rowImages.Cells[0].AddParagraph();
            followupImagesPara.AddFormattedText("F", "allCapsFirstLetter");
            followupImagesPara.AddFormattedText("OLLOW-UP WORK IMAGES", "allCapsNextLetter");
            rowImages.Cells[0].Shading.Color = headerGrey;

            addLineToFollowupTable("Image 1", currentSheet.Image1Url);
            addLineToFollowupTable("Image 2", currentSheet.Image2Url);
            addLineToFollowupTable("Image 3", currentSheet.Image3Url);
            addLineToFollowupTable("Image 4", currentSheet.Image4Url);
            addLineToFollowupTable("Image 5", currentSheet.Image5Url);

            //Add a space before the next table
            currentSection.AddParagraph();
        }

        private void addLineToFollowupTable(string rowTitle, string rowData)
        {
            Paragraph currentParagraph;
            Row currentRow = followupTable.AddRow();
            currentParagraph = currentRow.Cells[0].AddParagraph();
            currentParagraph.AddText(rowTitle);
            currentParagraph.Style = "sectionHeader";

            currentParagraph = currentRow.Cells[1].AddParagraph();
            currentParagraph.AddText(rowData);
            currentParagraph.Style = "Normal";
            currentRow.Cells[0].Shading.Color = entryHeaderGrey;
        }

        private void createServiceReportSection()
        { 
            Section currentSection = (Section)serviceSheetDoc.Sections.LastObject;
            serviceReportTable = currentSection.AddTable();

            //Setup the table columns and borders
            Column tableColumnn = serviceReportTable.AddColumn(COLUMN_ONE_WIDTH);
            tableColumnn = serviceReportTable.AddColumn(COLUMN_TWO_WIDTH);
            serviceReportTable.Borders.Color = tableBorderColour;
            serviceReportTable.Borders.Width = borderWidth;

            Row row1Title = serviceReportTable.AddRow();
            row1Title.Cells[0].MergeRight = 1;
            Paragraph serviceReportPara = row1Title.Cells[0].AddParagraph();
            serviceReportPara.AddFormattedText("S", "allCapsFirstLetter");
            serviceReportPara.AddFormattedText("ERVICE ", "allCapsNextLetter");
            serviceReportPara.AddFormattedText("R", "allCapsFirstLetter");
            serviceReportPara.AddFormattedText("EPORT", "allCapsNextLetter");
            row1Title.Cells[0].Shading.Color = headerGrey;

            row1Title.KeepWith = 8;

            addLineToServiceReportTable("Total travel time", currentSheet.TotalTravelTime.ToString());
            addLineToServiceReportTable("Total time onsite", currentSheet.TotalTimeOnsite.ToString());
            addLineToServiceReportTable("Total mileage", currentSheet.TotalMileage.ToString());
            addLineToServiceReportTable("Total daily allowances", currentSheet.TotalDailyAllowances.ToString());
            addLineToServiceReportTable("Total overnight allowances", currentSheet.TotalOvernightAllowances.ToString());
            addLineToServiceReportTable("Total barrier payments", currentSheet.TotalBarrierPayments.ToString());
            addLineToServiceReportTable("Job status", currentSheet.JobStatus);
            addLineToServiceReportTable("Job report", currentSheet.FinalJobReport);

            //Add a space before the next table
            currentSection.AddParagraph();
        }

        private void addLineToServiceReportTable(string rowTitle, string rowData)
        {
            Paragraph currentParagraph;
            Row currentRow = serviceReportTable.AddRow();
            currentParagraph = currentRow.Cells[0].AddParagraph();
            currentParagraph.AddText(rowTitle);
            currentParagraph.Style = "sectionHeader";

            currentParagraph = currentRow.Cells[1].AddParagraph();
            currentParagraph.AddText(rowData);
            currentParagraph.Style = "Normal";
            currentRow.Cells[0].Shading.Color = entryHeaderGrey;
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
            timesheetTable.Borders.Color = tableBorderColour;
            timesheetTable.Borders.Width = borderWidth;

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

            //Add a gap before the next section
            currentSection.AddParagraph();
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

        private void createSheetTitle()
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

            MigraDoc.DocumentObjectModel.Style footerStyle = serviceSheetDoc.Styles.AddStyle("footerStyle", "Normal");
            footerStyle.Font.Size = 6;
            footerStyle.Font.Name = "Arial";
            footerStyle.Font.Color = headerGrey;

            //Document header is different for page 1
            serviceSheetDoc.DefaultPageSetup.DifferentFirstPageHeaderFooter = true;
        }

        private void createJobDetailsSection()
        {
            Section currentSection = (Section)serviceSheetDoc.Sections.LastObject;
            jobDetailsTable = currentSection.AddTable();

            //Setup the table columns and borders
            Column tableColumnn = jobDetailsTable.AddColumn(COLUMN_ONE_WIDTH);
            tableColumnn = jobDetailsTable.AddColumn(COLUMN_TWO_WIDTH);
            jobDetailsTable.Borders.Color = tableBorderColour;
            jobDetailsTable.Borders.Width = borderWidth;

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
