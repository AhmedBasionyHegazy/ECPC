using System;

namespace MasMasr.Models
{
    public class News
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public DateTime CreationDate { get; set; }= DateTime.Now;
        public DateTime? ModificationDate { get; set; }
        public string Files { get; set; }
    }
}
