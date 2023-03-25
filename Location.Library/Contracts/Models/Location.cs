using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace LocationLibrary.Contracts.Models
{
    public class Location
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Address { get; set; }
    }
}
