using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;

namespace MasMasr.Helper
{
    public class FileUpload
    {
        public static string SaveFiles(IFormFile File,string Name)
        {
            if (File==null)
            {
                return "";
            }
            string fileName = "";
            string current = Directory.GetCurrentDirectory();
            if (!Directory.Exists(current + "\\Upload\\"))
            {
                Directory.CreateDirectory(current + "\\Upload\\");
            }
            
            //fileName = Guid.NewGuid() + System.IO.Path.GetExtension(File.FileName);
            using (FileStream fileStream = System.IO.File.Create(current + "\\Upload\\" + Name+ System.IO.Path.GetExtension(File.FileName)))
            {
                File.CopyTo(fileStream);
                fileStream.Flush();
            }
            return Name + System.IO.Path.GetExtension(File.FileName);
        }

        public static string Save(string base64Image)
        {
            if (string.IsNullOrEmpty(base64Image))
            {
                return "";
            }
            String path = Directory.GetCurrentDirectory()+ "\\Upload\\";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            var data = base64Image.Substring(0, 5);
            var extension = "";
            switch (data.ToUpper())
            {
                case "IVBOR":
                     extension = ".png";
                    break;
                case "/9J/4":
                    extension = ".jpg";
                    break;
            }

            var imageName = string.Format(@"{0}", Guid.NewGuid()) + extension;
            string imgPath = Path.Combine(path, imageName);
            var imageBytes = Convert.FromBase64String(base64Image);
            var imagefile = new FileStream(imgPath, FileMode.Create);
            imagefile.Write(imageBytes, 0, imageBytes.Length);
            imagefile.Flush();
            return imageName;
        }
    }
}
