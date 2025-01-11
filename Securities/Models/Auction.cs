using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Securities.Models;
namespace Securities.Models
{
    public class Auction
    {
        [Key]
        public int AuctionID { get; set; }
        public int PropertyID { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public decimal CurrentPrice { get; set; }
        public decimal MinimumBidIncrement { get; set; }
        public int? WinnerID { get; set; }
        public string Status { get; set; }
        public bool IsAutoExtend { get; set; }
        public int AutoExtendMinutes { get; set; }
        public DateTime? ExtendedEndDate { get; set; }
        public bool RequireDeposit { get; set; }
        public decimal DepositAmount { get; set; }

        [ForeignKey("PropertyID")]
        public virtual Product Products { get; set; }

        [ForeignKey("WinnerID")]
        public virtual User Winner { get; set; }
        public virtual ICollection<Bid> Bids { get; set; }
        public virtual ICollection<AuctionParticipant> Participants { get; set; }
    }
}
