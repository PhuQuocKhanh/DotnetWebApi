using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImplementSerilog.Models
{
    public class Book
    {
        public int Id { get; set; }               // Mã sách
        public string Title { get; set; }         // Tiêu đề sách
        public string Author { get; set; }        // Tác giả
        public int YearPublished { get; set; }    // Năm xuất bản
    }
}