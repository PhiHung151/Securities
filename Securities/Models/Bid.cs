using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Securities.Models;
namespace Securities.Models
{
    public class BidModel{
        public List<Bid> bidDetails{get; set;}
    }
    public class Bid
    {
        [Key]
        public int BidID { get; set; }
        public int AuctionID { get; set; }
        public int BidderID { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal BidAmount { get; set; }

        public DateTime BidTime { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
        public bool IsAutoBid { get; set; }
        public decimal? MaxAutoBidAmount { get; set; }

        [ForeignKey("AuctionID")]
        public virtual Auction Auction { get; set; }

        [ForeignKey("BidderID")]
        public virtual User Bidder { get; set; }
    }
}