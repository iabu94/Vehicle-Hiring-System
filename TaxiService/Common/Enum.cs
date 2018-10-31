using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaxiService.Common
{
    public class Enum
    {
        public enum EVehicleType
        {
            MINICAR = 1,
            NORMAL_CAR = 2,
            VAN = 3,
            BUS = 4
        }

        public enum EUserStatus
        {
            PENDING = 1,
            ACTIVE = 2,
            REJECTED = 3,
            RESIGNED = 4
        }

        public enum EIsDeleted
        {
            NO = 0,
            YES = 1
        }

        public enum EAvailability
        {
            OFFLINE = 0,
            ONLINE = 1
        }

        public enum ETripStatus
        {
            PENDING = 1,
            ALL = 2,
            APPROVED = 3,
            COMPLETED = 4,
            REJECTED = 5,
            CANCELLED = 6
        }

        public enum ETripVehicleType
        {
            MINI = 1,
            CAR = 2,
            VAN = 3,
            BUS = 4
        }

        public enum EUserType
        {
            ADMIN = 1,
            DRIVER = 2,
            RIDER = 3
        }
    }
}