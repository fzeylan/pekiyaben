using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Emoda.PekiYaBen.Entity.Enums.General;

namespace Emoda.PekiYaBen.Business.DTOs
{
    public class ProductListItem
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string ContentDescription { get; set; }
        public string ContentImage { get; set; }
        public string File { get; set; }
        public decimal Price { get; set; }
        public int SortOrder { get; set; }
        public bool Available { get; set; }
        public ProductType Type { get; set; }
        public Status Status { get; set; }

        public int RecordCount { get; set; }
    }
}
