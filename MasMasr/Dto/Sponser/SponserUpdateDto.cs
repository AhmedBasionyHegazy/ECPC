using Microsoft.AspNetCore.Http;

namespace MasMasr.Dto.Sponser
{
    public class SponserUpdateDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string DetailsAbout { get; set; }
        public string WebsiteLink { get; set; }
        public IFormFile File { get; set; }
    }
}
