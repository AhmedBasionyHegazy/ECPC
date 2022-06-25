using Microsoft.AspNetCore.Http;

namespace MasMasr.Dto.MainVideos
{
    public class MainVideosListDto
    {
        public int Id { get; set; }
        public string VideoTitle { get; set; }
        public string VideoLink { get; set; }
        public string ExternalLink { get; set; }
        public IFormFile File { get; set; }
    }
}
