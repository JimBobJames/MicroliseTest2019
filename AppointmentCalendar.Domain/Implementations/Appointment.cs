using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AppointmentCalendar.Domain.Contracts;

namespace AppointmentCalendar.Domain.Implementations
{
    [Table("Appointment")]
    public class Appointment : IEntity<int>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(255)]
        [Required(AllowEmptyStrings = false)]
        public string Summary { get; set; }
        [MaxLength(255)]
        [Required(AllowEmptyStrings = false)]
        public string Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
