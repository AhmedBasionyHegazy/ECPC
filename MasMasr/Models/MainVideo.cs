using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MasMasr.Models
{
    public class MainVideo
    {
        public int Id { get; set; }
        public string VideoTitle { get; set; }
        public string VideoLink { get; set; }
        public string ExternalLink { get; set; }
    }
}
