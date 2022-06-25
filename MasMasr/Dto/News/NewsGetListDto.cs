using System;
using System.Collections.Generic;

namespace MasMasr.Dto.News
{
    public class NewsGetListDto
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? ModificationDate { get; set; }
        public List<string> Files { get; set; }
    }
}
