using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PET1.Domain.Models
{
    public class ListModel<T>
    {
        public ListModel() { }
        public List<T> Items { get; set; } = new();
        // запрошенный список объектов
        // номер текущей страницы
        public int CurrentPage { get; set; } = 1;
        // общее количество страниц
        public int TotalPages { get; set; } = 1;
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
    }
}
