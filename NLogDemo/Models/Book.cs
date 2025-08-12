using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NLogDemo.Models
{
    public class Book
    {
        public int Id { get; set; }            // Mã định danh duy nhất của sách
        public string Title { get; set; }      // Tên sách
        public string Author { get; set; }     // Tác giả
        public int YearPublished { get; set; } // Năm xuất bản
    }
}