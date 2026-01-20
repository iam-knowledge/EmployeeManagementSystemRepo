
using System;
using System.ComponentModel.DataAnnotations;

namespace Employee_Management_System.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string FirstName { get; set; } = "";

        [Required, StringLength(50)]
        public string LastName { get; set; } = "";

        [Required, EmailAddress, StringLength(100)]
        public string Email { get; set; } = "";

        [Phone, StringLength(25)]
        public string? Phone { get; set; }

        [DataType(DataType.Date)]
        public DateTime HireDate { get; set; } = DateTime.UtcNow.Date;

        [StringLength(50)]
        public string? Department { get; set; }

        [Range(0, 1_000_000)]
        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }

        public bool IsActive { get; set; } = true;

        public string FullName => $"{FirstName} {LastName}";
    }
}

