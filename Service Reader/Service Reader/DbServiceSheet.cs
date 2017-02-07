using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;
using System.Data.SqlClient;
using System.Data.Entity.Core;

namespace Service_Reader
{
    public class DbServiceSheet
    {
        //Class for handling data from the service sheet database table
        public static bool saveSheetsAndDays(ObservableCollection<ServiceSheetViewModel> allServiceSheets)
        {
            UserViewModel dbUserVM = getDbUser();
            if (dbUserVM == null)
            {
                return false;
            }

            try
            {
                using (var dbContext = new ServiceSheetsEntities())
                {
                    updateContextConnection(dbUserVM, dbContext);
                    //System.Data.Common.DbConnection connection = dbContext.Database.Connection;
                    //System.Data.Common.DbConnectionStringBuilder str = new System.Data.Common.DbConnectionStringBuilder();
                    //str.ConnectionString = dbContext.Database.Connection.ConnectionString;
                    //str.Add("Password", dbUserVM.PasswordBoxEntry.Password);
                    //dbContext.Database.Connection.ConnectionString = str.ConnectionString;
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

            UserViewModel dbUser = getDbUser();
            if (dbUser == null)
            {
                return null;
            }
            //Downloads the service sheets from the database and creates the vms
            try
            {
                using (var dbContext = new ServiceSheetsEntities())
                {
                    updateContextConnection(dbUser, dbContext);
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
            UserViewModel dbUser = getDbUser();
            if (dbUser == null)
            {
                return null;
            }

            try
            {
                using (var dbContext = new ServiceSheetsEntities())
                {
                    updateContextConnection(dbUser, dbContext);

                    var userQuery = dbContext.ServiceSheets
                                        .Select(x => new { x.Username, x.UserFirstName, x.UserSurname }).Distinct();
                    foreach (var user in userQuery)
                    {
                        retval.Add(new DbEmployee(user.Username, user.UserFirstName, user.UserSurname));
                    }
                }
            }
            catch (EntityException entityEx)
            {
                //Something went wrong with the load.  Clear the cache for the username.
                clearCacheDbUsername();
                Console.WriteLine(entityEx.ToString());
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
            return retval;
        }

        private static void clearCacheDbUsername()
        {
            ObjectCache objCache = MemoryCache.Default;
            if (objCache.Contains(UserViewModel.CACHE_DATABASE_USER))
            {
                objCache.Remove(UserViewModel.CACHE_DATABASE_USER);
            }
        }

        //RT 23/1/17 - Gets the database username and password.  Should be stored in the application
        public static UserViewModel getDbUser()
        {
            //Get the username from the Cache
            ObjectCache objCache = MemoryCache.Default;
            if (objCache.Contains(UserViewModel.CACHE_DATABASE_USER))
            {
                return (UserViewModel)objCache.Get(UserViewModel.CACHE_DATABASE_USER);
            }

            UserViewModel dbUserVM = new UserViewModel(UserViewModel.MODE_DATABASE);
            UserView userView = new UserView();
            userView.DataContext = dbUserVM;
            bool? userResult = userView.ShowDialog();

            //RT - The box may have been cancelled
            if (userResult != true)
            {
                return null;
            }

            //Add the item to the cache.  We don't know if the username and password are correct.
            CacheItemPolicy policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(5);

            objCache.Add(UserViewModel.CACHE_DATABASE_USER, dbUserVM, policy);

            return dbUserVM;
        }

        public static List<int> getAllSubmissionNumbers()
        {
            List<int> retval = new List<int>();
            //Selects all submission numbers from the database
            UserViewModel dbUser = getDbUser();
            if (dbUser == null)
            {
                return null;
            }

            try
            {
                using (var dbContext = new ServiceSheetsEntities())
                {
                    updateContextConnection(dbUser, dbContext);

                    var submissionNumberQuery = from submissions in dbContext.ServiceSheets
                                                select submissions.SubmissionNumber;
                    retval = submissionNumberQuery.ToList<int>();
                }
            }
            catch (EntityException entityEx)
            {
                //Something went wrong with the load.  Clear the cache for the username.
                clearCacheDbUsername();
                Console.WriteLine(entityEx.ToString());
                return null;
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

            UserViewModel dbUser = getDbUser();
            if (dbUser == null)
            {
                return null;
            }
            //Downloads the service sheets from the database and creates the vms
            try
            {
                using (var dbContext = new ServiceSheetsEntities())
                {
                    updateContextConnection(dbUser, dbContext);
                    var serviceSheets = from ServiceSheet in dbContext.ServiceSheets
                                        where ServiceSheet.SubmissionNumber > 15679
                                        select ServiceSheet;
                    retval = ServiceSheetViewModel.loadFromModel(serviceSheets);
                }
            }
            catch (EntityException entityEx)
            {
                //Something went wrong with the load.  Clear the cache for the username.
                clearCacheDbUsername();
                Console.WriteLine(entityEx.ToString());
                return null;
            }
            catch (Exception ex)
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

            UserViewModel dbUser = getDbUser();
            if (dbUser == null)
            {
                return null;
            }
            

            try
            {
                using (var dbContext = new ServiceSheetsEntities())
                {
                    updateContextConnection(dbUser, dbContext);
                    var serviceSheets =
                    from ss in dbContext.ServiceSheets
                    join sd in dbContext.ServiceDays on ss.Id equals sd.ServiceSheetId
                    where ss.Username == username && sd.DtReport >= startDate && sd.DtReport <= endDate
                    select ss;

                    retval = ServiceSheetViewModel.loadFromModel(serviceSheets);
                }
            }
            catch (EntityException entityEx)
            {
                //Something went wrong with the load.  Clear the cache for the username.
                clearCacheDbUsername();
                Console.WriteLine(entityEx.ToString());
                return null;
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

        private static void updateContextConnection(UserViewModel dbUser, ServiceSheetsEntities dbContext)
        {
            System.Data.Common.DbConnection connection = dbContext.Database.Connection;
            System.Data.Common.DbConnectionStringBuilder str = new System.Data.Common.DbConnectionStringBuilder();
            str.ConnectionString = dbContext.Database.Connection.ConnectionString;
            str.Add("Password", dbUser.PasswordBoxObj.Password);
            dbContext.Database.Connection.ConnectionString = str.ConnectionString;
        }
    }
}
