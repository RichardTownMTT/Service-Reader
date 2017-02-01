using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_Reader
{
    public class DbServiceSheet
    {
        //Class for handling data from the service sheet database table
        public static bool saveSheetsAndDays(ObservableCollection<ServiceSheetViewModel> allServiceSheets)
        {
            UserViewModel dbUserVM = getDbUserVM();
            if (dbUserVM == null)
            {
                return false;
            }

            try
            {
                using (var dbContext = new ServiceSheetsEntities())
                {
                    System.Data.Common.DbConnection connection = dbContext.Database.Connection;
                    System.Data.Common.DbConnectionStringBuilder str = new System.Data.Common.DbConnectionStringBuilder();
                    str.ConnectionString = dbContext.Database.Connection.ConnectionString;
                    str.Add("Password", dbUserVM.PasswordBoxObj.Password);
                    dbContext.Database.Connection.ConnectionString = str.ConnectionString;
                    //dbContext.Database.Log = Console.Write;
                    foreach (ServiceSheetViewModel serviceVM in allServiceSheets)
                    {
                        dbContext.ServiceSheets.Add(serviceVM.ServiceSubmission);
                        foreach (ServiceDayViewModel day in serviceVM.AllServiceDays.AllServiceDayVMs)
                        {
                            dbContext.ServiceDays.Add(day.ServiceDayModel);
                        }
                    }
                    dbContext.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
            return true;
        }

        public static List<ServiceSheetViewModel> loadHolidayAbsenceSheets()
        {
            List<ServiceSheetViewModel> retval = new List<ServiceSheetViewModel>();

            UserViewModel dbUserVM = getDbUserVM();
            if (dbUserVM == null)
            {
                return null;
            }
            //Downloads the service sheets from the database and creates the vms
            try
            {
                using (var dbContext = new ServiceSheetsEntities())
                {
                    updateContextConnection(dbUserVM, dbContext);
                    var serviceSheets = from ServiceSheet in dbContext.ServiceSheets
                                        where ServiceSheet.SubmissionNumber < 0
                                        select ServiceSheet;
                    retval = ServiceSheetViewModel.loadFromModel(serviceSheets);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
            return retval;
        }

        public static List<DbEmployee> getAllUsers()
        {
            List<DbEmployee> retval = new List<DbEmployee>();
            //Selects all submission numbers from the database
            UserViewModel dbUserVM = getDbUserVM();
            if (dbUserVM == null)
            {
                return null;
            }

            try
            {
                using (var dbContext = new ServiceSheetsEntities())
                {
                    updateContextConnection(dbUserVM, dbContext);

                    var userQuery = dbContext.ServiceSheets
                                        .Select(x => new { x.Username, x.UserFirstName, x.UserSurname }).Distinct();
                    foreach (var user in userQuery)
                    {
                        retval.Add(new DbEmployee(user.Username, user.UserFirstName, user.UserSurname));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
            return retval;
        }

        //RT 23/1/17 - Gets the database username and password.  Should be stored in the application
        public static UserViewModel getDbUserVM()
        {
            UserViewModel DbUserVM = new UserViewModel(UserViewModel.DISPLAY_MODE_DATABASE);
            UserView userView = new UserView();
            userView.DataContext = DbUserVM;
            bool? userResult = userView.ShowDialog();

            //RT - The box may have been cancelled
            if (userResult != true)
            {
                return null;
            }

            return DbUserVM;
        }

        public static List<int> getAllSubmissionNumbers()
        {
            List<int> retval = new List<int>();
            //Selects all submission numbers from the database
            UserViewModel dbUserVM = getDbUserVM();
            if (dbUserVM == null)
            {
                return null;
            }

            try
            {
                using (var dbContext = new ServiceSheetsEntities())
                {
                    updateContextConnection(dbUserVM, dbContext);

                    var submissionNumberQuery = from submissions in dbContext.ServiceSheets
                                                select submissions.SubmissionNumber;
                    retval = submissionNumberQuery.ToList<int>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
            return retval;
        }

        public static List<ServiceSheetViewModel> downloadAllServiceSheets()
        {
            List<ServiceSheetViewModel> retval = new List<ServiceSheetViewModel>();

            UserViewModel dbUserVM = getDbUserVM();
            if (dbUserVM == null)
            {
                return null;
            }
            //Downloads the service sheets from the database and creates the vms
            try
            {
                using (var dbContext = new ServiceSheetsEntities())
                {
                    updateContextConnection(dbUserVM, dbContext);
                    var serviceSheets = from ServiceSheet in dbContext.ServiceSheets
                                        where ServiceSheet.SubmissionNumber > 15679
                                        select ServiceSheet;
                    retval = ServiceSheetViewModel.loadFromModel(serviceSheets);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
            return retval;
        }

        public static bool? checkForClashingDates(DateTime startDate, DateTime endDate, string username)
        {
            //Returns true if a service day exists for between the given dates.
            List<ServiceSheetViewModel> retval = new List<ServiceSheetViewModel>();

            UserViewModel dbUserVM = getDbUserVM();
            if (dbUserVM == null)
            {
                return null;
            }
            

            try
            {
                using (var dbContext = new ServiceSheetsEntities())
                {
                    updateContextConnection(dbUserVM, dbContext);
                    var serviceSheets =
                    from ss in dbContext.ServiceSheets
                    join sd in dbContext.ServiceDays on ss.Id equals sd.ServiceSheetId
                    where ss.Username == username && sd.DtReport >= startDate && sd.DtReport <= endDate
                    select ss;

                    retval = ServiceSheetViewModel.loadFromModel(serviceSheets);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }

            if (retval == null)
            {
                return false;
            }
            return true;
        }

        public static bool saveSheetsAndDays(ServiceSheetViewModel serviceSheetCreated)
        {
            ObservableCollection<ServiceSheetViewModel> sheetToSave = new ObservableCollection<ServiceSheetViewModel>();
            sheetToSave.Add(serviceSheetCreated);
            bool retval = saveSheetsAndDays(sheetToSave);
            return retval;
        }

        private static void updateContextConnection(UserViewModel dbUserVM, ServiceSheetsEntities dbContext)
        {
            System.Data.Common.DbConnection connection = dbContext.Database.Connection;
            System.Data.Common.DbConnectionStringBuilder str = new System.Data.Common.DbConnectionStringBuilder();
            str.ConnectionString = dbContext.Database.Connection.ConnectionString;
            str.Add("Password", dbUserVM.PasswordBoxObj.Password);
            dbContext.Database.Connection.ConnectionString = str.ConnectionString;
        }
    }
}
