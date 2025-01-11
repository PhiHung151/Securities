using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Securities.Models
{
     public class ProductDocumentModel
    {
        public List<ProductDocument> productDocumentsDetails { get; set; }
    }
  public class ProductDocument
    {
        [Key]
        public int DocumentID { get; set; }
        public int ProductID { get; set; }
        public string DocumentType { get; set; }
        public string DocumentPath { get; set; }
        public DateTime UploadDate { get; set; }
        public bool IsVerified { get; set; }

        [ForeignKey("PropertyID")]
        public virtual Product Products { get; set; }
    }
}