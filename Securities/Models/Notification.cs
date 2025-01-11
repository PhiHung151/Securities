 using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Securities.Models;
namespace Securities.Models
{
    public class NotificationModel{
        public List<Notification> notificationsDetails{get; set;}
    }
    public class Notification
    {
            [Key]
            public int NotificationID { get; set; }
            public int UserID { get; set; }
            public string Title { get; set; }
            public string Message { get; set; }
            public string Type { get; set; }
            public bool IsRead { get; set; }
            public DateTime CreatedDate { get; set; }

            [ForeignKey("UserID")]
            public virtual User User { get; set; }
    }
}