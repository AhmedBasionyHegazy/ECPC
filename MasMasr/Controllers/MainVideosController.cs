using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MasMasr.Models;
using System.IO;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using MasMasr.Dto.MainVideos;

namespace MasMasr.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainVideosController : ControllerBase
    {
        private readonly DataContext _context;

        public MainVideosController(DataContext context)
        {
            _context = context;
        }

        // GET: api/MainVideos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MainVideosListDto>>> GetMainVideos()
        {
            var Data=new List<MainVideo>();
            try
            {
                List<MainVideosListDto> mainVideoDtos = new List<MainVideosListDto>();
                Data = await _context.MainVideos.ToListAsync();
                foreach (var item in Data)
                {
                    string fileName = GetMainVideo(item.Id).Result.Value.VideoLink;
                    string filepath = Directory.GetCurrentDirectory() + "\\Upload\\" + fileName;
                    byte[] fileBytes = System.IO.File.ReadAllBytes(filepath);


                    var stream = System.IO.File.OpenRead(filepath);
                    var file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
                    {
                        Headers = new HeaderDictionary(),
                        ContentType = System.Net.Mime.MediaTypeNames.Application.Octet //"application/pdf"
                    };
                    mainVideoDtos.Add(new MainVideosListDto
                    {
                        ExternalLink=item.ExternalLink,
                        Id=item.Id,
                        VideoLink=item.VideoLink,
                        VideoTitle=item.VideoTitle,
                        File = file
                    });

                    //var stream = new MemoryStream(fileBytes);

                    //mainVideoInsertDtos.Add(new MainVideoInsertDto
                    //{
                    //    File= new FormFile(stream, 0, fileBytes.Length, fileName, fileName)
                    //});
                }

                return mainVideoDtos;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // GET: api/MainVideos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MainVideo>> GetMainVideo(int id)
        {
            var mainVideo = await _context.MainVideos.FindAsync(id);

            if (mainVideo == null)
            {
                return NotFound();
            }

            return mainVideo;
        }

        // PUT: api/MainVideos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMainVideo(int id,[FromForm] MainVideoUpdateDto mainVideo)
        {
            if (id != mainVideo.Id)
            {
                return BadRequest();
            }

            MainVideo video = new MainVideo
            {
                Id = mainVideo.Id,
                VideoTitle = mainVideo.VideoTitle,
                VideoLink= Helper.FileUpload.SaveFiles(mainVideo.File, mainVideo.VideoTitle)
            };

            _context.Entry(video).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MainVideoExists(id))
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

        // POST: api/MainVideos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MainVideo>> PostMainVideo([FromForm] MainVideoInsertDto mainVideo)
        {
            try
            {
                MainVideo video = new MainVideo 
                {
                    VideoLink=Helper.FileUpload.SaveFiles(mainVideo.File, mainVideo.VideoTitle),
                    ExternalLink=mainVideo.ExternalLink,
                    VideoTitle= mainVideo.VideoTitle
                };
                _context.MainVideos.Add(video);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetMainVideo", new { id = video.Id }, mainVideo);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpGet("DownloadFile/{id}")]
        public async Task<FileResult> DownloadFile(int id)
        {
            string fileName = GetMainVideo(id).Result.Value.VideoLink;
            string filepath = Directory.GetCurrentDirectory() + "\\Upload\\" + fileName; 
            byte[] fileBytes = System.IO.File.ReadAllBytes(filepath);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        // DELETE: api/MainVideos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMainVideo(int id)
        {
            var mainVideo = await _context.MainVideos.FindAsync(id);
            if (mainVideo == null)
            {
                return NotFound();
            }

            _context.MainVideos.Remove(mainVideo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MainVideoExists(int id)
        {
            return _context.MainVideos.Any(e => e.Id == id);
        }
    }
}
