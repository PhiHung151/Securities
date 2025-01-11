using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Securities.Models;
namespace Securities.Models
{
    public class ProductModel
    {
        public List<Product> productsDetails { get; set; }
    }
    public class Product
        {
            [Key]
            public int ProductID { get; set; }

            [Required]
            public int SellerID { get; set; }

            [Required]
            [StringLength(200)]
            public string Title { get; set; }

            [Required]
            public string Description { get; set; }

            [Required]
            public string Address { get; set; }

            public double LandArea { get; set; }
            public int Bedrooms { get; set; }
            public int Bathrooms { get; set; }
            public int YearBuilt { get; set; }
            public string ProductType { get; set; }
            public string City { get; set; }
            public string District { get; set; }
            public string Ward { get; set; }
            public string LocationDescription { get; set; }
            public double? Latitude { get; set; }
            public double? Longitude { get; set; }
            public string Images { get; set; }
            public string Videos { get; set; }
            public string Documents { get; set; }

            [Required]
            [Range(0, double.MaxValue)]
            public decimal StartingPrice { get; set; }
            public decimal ReservePrice { get; set; }
            public string Status { get; set; }
            public DateTime CreatedDate { get; set; }
            public DateTime? ModifiedDate { get; set; }
            public bool IsVerified { get; set; }
            public bool IsFeature { get; set; }

            [ForeignKey("SellerID")]
            public virtual User Seller { get; set; }
            public virtual ICollection<Auction> Auctions { get; set; }
            public virtual ICollection<ProductDocument> LegalDocuments { get; set; }
        }
}