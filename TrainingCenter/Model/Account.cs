using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingCenter.Model
{
    public class Account
    {
        public int AccountId { get; set; }
        [StringLength(102)]
        public string Email { get; set; }
        [StringLength(250)]
        public string Password { get; set; }
        [StringLength(100)]
        public string FirstName { get; set; }
        [StringLength(100)]
        public string LastName { get; set; }
        public DateTime SignUpDate { get; set; }
        [StringLength(100)]
        public string AdressStreet { get; set; }
        [StringLength(100)]
        public string AdressNumber { get; set; }
        [StringLength(100)]
        public string City { get; set; }
        [StringLength(100)]
        public string Country { get; set; }
        [StringLength(100)]
        public string AccountType { get; set; }
        [StringLength(255)]
        public string Notes { get; set; }
    }
}
