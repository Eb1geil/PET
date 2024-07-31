using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PET1.Domain.Entities
{
    public class Movies
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public int Price { get; set; }
        public string? ImgPath { get; set; }
        public string ImgType { get; set; }
    }
}
