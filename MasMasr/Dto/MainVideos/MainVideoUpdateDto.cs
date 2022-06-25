using Microsoft.AspNetCore.Http;

namespace MasMasr.Dto.MainVideos
{
    public class MainVideoUpdateDto
    {
        public int Id { get; set; }
        public string VideoTitle { get; set; }
        public string ExternalLink { get; set; }
        public IFormFile File { get; set; }
    }
}
