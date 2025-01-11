using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Securities.Models;
namespace Securities.Models
{
    public class AuctionParticipantModel{
        public List<AuctionParticipant> auctionParticipantsDetails{get; set;}
    }
    public class AuctionParticipant
    {
        public int AuctionID { get; set; }
        public int UserID { get; set; }
        public DateTime RegisterDate { get; set; }
        public bool HasPaidDeposit { get; set; }
        public decimal DepositAmount { get; set; }
        public string Status { get; set; }

        [ForeignKey("AuctionID")]
        public virtual Auction Auction { get; set; }

        [ForeignKey("UserID")]
        public virtual User User { get; set; }
    }
}