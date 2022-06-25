using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace MasMasr.Dto.News
{
    public class NewsUpdateDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ModificationDate { get; set; } = DateTime.Now;
        //public List<string>? Files { get; set; }
        public IFormFile File { get; set; }
    }
}
