using System;
using System.ComponentModel.DataAnnotations;

namespace BlazorDapper.Data
{
    public class Video
    {
        public int VideoId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public DateTime Datepublished { get; set; }
        public bool IsActive { get; set; }

    }
}
