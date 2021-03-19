using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BikesIsland.Models.Dto
{
    public class BikeReservationDto
    {
        [Required]
        public string BikeId { get; set; }
        [Required]
        public string CustomerId { get; set; }
        [Required]
        public string CustomerName { get; set; }
        [Required]
        public DateTime RentFrom { get; set; }
        [Required]
        public DateTime RentTo { get; set; }
    }
}
