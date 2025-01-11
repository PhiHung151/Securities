using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Securities.Models
{
     public class UserModel
    {
        public List<User> usersDetails { get; set; }
    }
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string FullName { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        public string UserType { get; set; }

        public string Address { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public string ProfileImage { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public Guid SecurityStamp { get; set; }

        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<Bid> Bids { get; set; }
        public virtual ICollection<Auction> WonAuctions { get; set; }
        public virtual ICollection<AuctionParticipant> Participations { get; set; }
         public virtual ICollection<Notification> Notifications { get; set; }
    }
}