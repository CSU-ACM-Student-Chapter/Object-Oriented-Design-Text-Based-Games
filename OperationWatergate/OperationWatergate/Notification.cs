﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperationWatergate
{
    public class Notification
    {
        public String Name { get; set; }
        public Object Object { get; set; }
        public Dictionary<String, Object> UserInfo { get; set; }

        public Notification() : this("NotificationName")
        { 
        }

        public Notification(String name) : this(name, null)
        {
        }

        public Notification(String name, Object obj) : this(name, obj, null)
        {
        }

        public Notification(String name, Object obj, Dictionary<String, Object> userInfo)
        {
            this.Name = name;
            this.Object = obj;
            this.UserInfo = userInfo;
        }
    }
    /* Can be used for the use of multiple clocks */
}
