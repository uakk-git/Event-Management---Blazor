using System;
using System.ComponentModel.DataAnnotations;

namespace EventEase.Models
{
    public class Event
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Event name is required.")]
        [StringLength(100, ErrorMessage = "Event name must be 100 characters or fewer.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date is required.")]
        public DateTime Date { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Location is required.")]
        [StringLength(200, ErrorMessage = "Location must be 200 characters or fewer.")]
        public string Location { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        public int Capacity { get; set; } = 100;
        public int AttendeesCount { get; set; } = 0;

        public bool IsAvailable => AttendeesCount < Capacity;
    }
}