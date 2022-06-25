using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MasMasr.Models;
using MasMasr.Dto.Sponser;
using System.IO;
using ImageProcessor;
using ImageProcessor.Plugins.WebP.Imaging.Formats;

namespace MasMasr.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SponserController : ControllerBase
    {
        private readonly DataContext _context;

        public SponserController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Sponser
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SponserListDto>>> GetSponsers()
        {
            var Data = await _context.Sponsers.ToListAsync();
            List<SponserListDto> result = new List<SponserListDto>();
            string file = "";
            foreach (var item in Data)
            {
                file = "";
                if (!string.IsNullOrEmpty(item.logoImg))
                {
                    string filepath = Directory.GetCurrentDirectory() + "\\Upload\\" + item.logoImg;
                    //string filepath = Directory.GetCurrentDirectory() + "\\Upload\\ThumbNail\\" + item.logoImg;
                    byte[] fileBytes = System.IO.File.ReadAllBytes(filepath);
                    file = "data:image/" + item.logoImg.Split('.')[1] + ";base64," + Convert.ToBase64String(fileBytes, 0, fileBytes.Length);
                }

                result.Add(new SponserListDto
                {
                    Id = item.Id,
                    logoImg =file,
                    Title = item.Title,
                    WebsiteLink= item.WebsiteLink,
                    DetailsAbout=item.DetailsAbout
                });

            }

            return result;
        }
        // GET: api/Sponser/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SponserListDto>> GetSponser(int id)
        {
            var sponser = await _context.Sponsers.FindAsync(id);

            if (sponser == null)
            {
                return NotFound();
            }

            var Data = await _context.Sponsers.ToListAsync();
            SponserListDto result = new SponserListDto();
            string file = "";
            if (!string.IsNullOrEmpty(sponser.logoImg))
            {
                string filepath = Directory.GetCurrentDirectory() + "\\Upload\\" + sponser.logoImg;
                byte[] fileBytes = System.IO.File.ReadAllBytes(filepath);
                file = "data:image/" + sponser.logoImg.Split('.')[1] + ";base64," + Convert.ToBase64String(fileBytes, 0, fileBytes.Length);
            }

            return new SponserListDto
            {
                Id = sponser.Id,
                logoImg = file,
                Title = sponser.Title,
                WebsiteLink = sponser.WebsiteLink,
                DetailsAbout = sponser.DetailsAbout
            };


        }

        // PUT: api/Sponser/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutSponser([FromForm] SponserUpdateDto sponserDto)
        {
            Sponser sponser = new Sponser
            {
               
                Title = sponserDto.Title,
                logoImg = Helper.FileUpload.SaveFiles(sponserDto.File, sponserDto.File.FileName.Split('.')[0]),
                Id = sponserDto.Id,
                DetailsAbout= sponserDto.DetailsAbout,
                WebsiteLink= sponserDto.WebsiteLink
            };

            _context.Entry(sponser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SponserExists(sponserDto.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private string SaveAsThumbnail(IFormFile file)
        {
            string thumbFolder = Directory.GetCurrentDirectory() + "\\Upload\\ThumbNail\\" + file.FileName;
            Directory.CreateDirectory(thumbFolder);
            using (var webPFileStream = new FileStream(thumbFolder, FileMode.Create))
            {
                using (ImageFactory imageFactory = new ImageFactory(preserveExifData: false))
                {
                    imageFactory.Load(file.OpenReadStream());
                    var thumb = imageFactory.Image.GetThumbnailImage(150, 150, () => false, IntPtr.Zero);
                    imageFactory.Format(new WebPFormat())
                                .Quality(90)
                                .Save(webPFileStream);
                }
                webPFileStream.Close();
            }
            return file.FileName;
        }

        // POST: api/Sponser
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Sponser>> PostSponser([FromForm] SponserInsertDto sponserDto)
        {
            Sponser sponser = new Sponser
            {

                Title = sponserDto.Title,
                logoImg = Helper.FileUpload.SaveFiles(sponserDto.File, sponserDto.File.FileName.Split('.')[0]),
                DetailsAbout = sponserDto.DetailsAbout,
                WebsiteLink = sponserDto.WebsiteLink
            };
            _context.Sponsers.Add(sponser);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSponser", new { id = sponser.Id }, sponser);
        } 

        // DELETE: api/Sponser/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSponser(int id)
        {
            var sponser = await _context.Sponsers.FindAsync(id);
            if (sponser == null)
            {
                return NotFound();
            }

            _context.Sponsers.Remove(sponser);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SponserExists(int id)
        {
            return _context.Sponsers.Any(e => e.Id == id);
        }
    }
}
