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
