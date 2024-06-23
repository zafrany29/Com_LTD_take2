using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Welp.Models
{
    [Table("CompanyInfo")]
    public class CompanyInfo
    {
        [Key]
        public string CompanyName { get; set; }
        public string Country { get; set; }
        public string Size { get; set; }
        public string Days { get; set; }
        public string Price { get; set; }
    }
}
