using Microsoft.AspNetCore.Http;
using System;

namespace MasMasr.Dto.News
{
    public class NewsInsertDto
    {
        public string Title { get; set; }
        public string Details { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public IFormFileCollection Files { get; set; }
    }
}
