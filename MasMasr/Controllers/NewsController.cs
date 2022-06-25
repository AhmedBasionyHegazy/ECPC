using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MasMasr.Models;
using System.IO;
using MasMasr.Dto.News;
using ImageProcessor;
using ImageProcessor.Plugins.WebP.Imaging.Formats;

namespace MasMasr.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly DataContext _context;

        public NewsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/News
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NewsGetListDto>>> GetNews()
        {
            var Data = await _context.News.ToListAsync();
            List<NewsGetListDto> result = new List<NewsGetListDto>();
            foreach (var item in Data)
            {
                List<string> files = new List<string>();
                if (item.Files != null)
                {
                    string[] fileNames = item.Files.Split(',');
                    foreach (var fileName in fileNames)
                    {
                        string filepath = Directory.GetCurrentDirectory() + "\\Upload\\" + fileName;
                        byte[] fileBytes = System.IO.File.ReadAllBytes(filepath);
                        files.Add("data:image/" + fileName.Split('.')[1] + ";base64," + Convert.ToBase64String(fileBytes, 0, fileBytes.Length));
                    }
                }
                result.Add(new NewsGetListDto
                {
                    Id = item.Id,
                    CreationDate = item.CreationDate,
                    Details = item.Details,
                    Files = files,
                    ModificationDate = item.ModificationDate,
                    Title = item.Title
                });

            }

            return result;
        }

        // GET: api/News/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NewsGetListDto>> GetNews(int id)
        {
            var news = await _context.News.FindAsync(id);

            if (news == null)
            {
                return NotFound();
            }
                List<string> files = new List<string>();
            if (news.Files != null)
            {
                string[] fileNames = news.Files.Split(',');
                foreach (var fileName in fileNames)
                {
                    string filepath = Directory.GetCurrentDirectory() + "\\Upload\\" + fileName;
                    byte[] fileBytes = System.IO.File.ReadAllBytes(filepath);
                    files.Add("data:image/" + fileName.Split('.')[1] + ";base64," + Convert.ToBase64String(fileBytes, 0, fileBytes.Length));
                }
            }
            NewsGetListDto result = new NewsGetListDto
            {
                Id = news.Id,
                CreationDate = news.CreationDate,
                Details = news.Details,
                Files = files,
                ModificationDate = news.ModificationDate,
                Title = news.Title
            };

            return result;
        }

        // PUT: api/News/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutNews([FromForm] NewsUpdateDto news)
        {
            
            
            News news1 = new News
            {
                CreationDate = news.CreationDate,
                Details = news.Details,
                Title = news.Title,
                Files = Helper.FileUpload.SaveFiles(news.File, news.File.FileName.Split('.')[0]),
                Id = news.Id,
                ModificationDate = DateTime.Now
            };
            _context.Entry(news1).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NewsExists(news.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        // POST: api/News
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<News>> PostNews([FromForm] NewsInsertDto news)
        {
            List<string> Names = new List<string>();
            foreach (var item in news.Files)
            {
                Names.Add(Helper.FileUpload.SaveFiles(item, item.FileName.Split('.')[0]));
            }
            News news1 = new News
            {
                CreationDate = DateTime.Now,
                Details = news.Details,
                Title = news.Title,
                Files = String.Join(",", Names)
            };
            _context.News.Add(news1);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNews", new { id = news1.Id }, news1);
        }

        // DELETE: api/News/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNews(int id)
        {
            var news = await _context.News.FindAsync(id);
            if (news == null)
            {
                return NotFound();
            }

            _context.News.Remove(news);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpPost(nameof(PostImages))]
        public ActionResult<string> PostImages([FromForm]IFormFile files)
        {
            string Names = "";
            
            Names=Helper.FileUpload.SaveFiles(files, files.FileName.Split('.')[0]);
            


            return Names;
        }

            private bool NewsExists(int id)
        {
            return _context.News.Any(e => e.Id == id);
        }
    }
}
