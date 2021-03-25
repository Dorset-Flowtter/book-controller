using System.ComponentModel.DataAnnotations;

namespace TestApplication.Models
{
    public class Book_description
    {
        [Key]
        public int id { get; set; }
        public int book_id { get; set; }
        public string book_name { get; set; }
        public string book_description { get; set; }
    }
}