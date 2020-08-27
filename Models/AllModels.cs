using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace PersonalSite.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime Created_At { get; set; }
        public DateTime Updated_At { get; set; }
        public User()
        {
            Created_At = DateTime.Now;
            Updated_At = DateTime.Now;
        }
    }

    public class ContactFormModel
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
    }
}