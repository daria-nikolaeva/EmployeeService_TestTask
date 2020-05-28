namespace EmployeeServiceTest
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Employee")]
    public partial class Employee
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Surname { get; set; }

        [StringLength(50)]
        public string Middlename { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date_of_birth { get; set; }

        [Required]
        [StringLength(100)]
        public string Address { get; set; }

        public int Department_id { get; set; }

        [Column(TypeName = "text")]
        public string About { get; set; }

        public virtual Department Department { get; set; }
    }
}
