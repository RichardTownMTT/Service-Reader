using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_Reader
{
    public class EngineerActivity
    {
        private int m_activityCode;
        private string m_activityDescription;

        public static List<EngineerActivity> getAllActivities()
        {
            List<EngineerActivity> retval = new List<EngineerActivity>();
            retval.Add(new EngineerActivity(ACTIVITY_HOLIDAY));
            retval.Add(new EngineerActivity(ACTIVITY_ILLNESS));
            retval.Add(new EngineerActivity(ACTIVITY_TRAINING));
            retval.Add(new EngineerActivity(ACTIVITY_STAND_BY));
            return retval;
        }

        private EngineerActivity(int actCode)
        {
            this.ActivityCode = actCode;
            switch (actCode)
            {
                case ACTIVITY_HOLIDAY:
                    this.ActivityDescription = "Holiday";
                    break;
                case ACTIVITY_ILLNESS:
                    this.ActivityDescription = "Illness";
                    break;
                case ACTIVITY_TRAINING:
                    this.ActivityDescription = "Training";
                    break;
                case ACTIVITY_STAND_BY:
                    this.ActivityDescription = "Stand By";
                    break;
                default:
                    new Exception("Unknown activity type " + actCode);
                    break;
            }
        }

        //RT 31/1/17 - Activity type records
        private const int ACTIVITY_HOLIDAY = -1;
        private const int ACTIVITY_ILLNESS = -2;
        private const int ACTIVITY_TRAINING = -3;
        private const int ACTIVITY_STAND_BY = -4;

        public int ActivityCode
        {
            get
            {
                return m_activityCode;
            }

            set
            {
                m_activityCode = value;
            }
        }

        public string ActivityDescription
        {
            get
            {
                return m_activityDescription;
            }

            set
            {
                m_activityDescription = value;
            }
        }
    }
}
