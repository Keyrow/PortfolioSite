using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using MimeKit;
using MimeKit.Text;
using MailKit;

namespace PersonalSite.Models
{
    public class CustomDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime date = (DateTime)value;
            return date > DateTime.Now;
        }
    }

    // Login with validations
    public class LoginUser
    {
        public string LogEmail { get; set; }

        [DataType(DataType.Password)]
        public string LogPassword { get; set; }
    }

    // Registration with validations
    public class RegisterUser
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "First name field must not be empty.")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "First name must be non-numerical.")]
        [MinLength(2)]
        [MaxLength(50)]
        public string First_Name { get; set; }

        [Required(ErrorMessage = "Last name field must not be empty.")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Last name must be non-numerical.")]
        [MinLength(2)]
        [MaxLength(50)]
        public string Last_Name { get; set; }

        [Required(ErrorMessage = "Email field must not be empty.")]
        [EmailAddress]
        [RegularExpression(@"^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password field must not be empty.")]
        [MinLength(8, ErrorMessage = "Password must be 8 or more characters.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Password confirm field must not be empty.")]
        [NotMapped]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string Confirm { get; set; }
        public DateTime Created_At { get; set; }
        public DateTime Updated_At { get; set; }
    }

    // Dashboard aka Home Page
    public class DashboardModel
    {
        public User Users { get; set; }
    }

    // Email Address
    public class EmailAddress
    {
        public string Name { get; set; }
        public string Address { get; set; }
    }

    // Email Message
    public class EmailMessage
    {
        public List<EmailAddress> ToAddresses { get; set; } = new List<EmailAddress>();
        public List<EmailAddress> FromAddresses { get; set; } = new List<EmailAddress>();
        public string Subject { get; set; }
        public string Content { get; set; }
    }

    // SmtpServer Configuration
    public class EmailServerConfiguration
    {
        public EmailServerConfiguration(int _smtpPort = 587)
        {
            SmtpPort = _smtpPort;
        }

        public string SmtpServer { get; set; }
        public int SmtpPort { get; }
        public string SmtpUsername { get; set; }
        public string SmtpPassword { get; set; }
    }

    // Dependency Injection Feature
    public interface IEmailService
    {
        void Send(EmailMessage message);
    }

    public class MailKitEmailService : IEmailService
    {
        private readonly EmailServerConfiguration _eConfig;

        public MailKitEmailService(EmailServerConfiguration config)
        {
            _eConfig = config;
        }

        public void Send(EmailMessage msg)
        {
            var message = new MimeMessage();
            message.To.AddRange(msg.ToAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));
            message.From.AddRange(msg.FromAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));

            message.Subject = msg.Subject;

            message.Body = new TextPart("plain")
            {
                Text = msg.Content
            };

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                client.Connect(_eConfig.SmtpServer, _eConfig.SmtpPort);

                client.AuthenticationMechanisms.Remove("XOAUTH2");

                client.Authenticate(_eConfig.SmtpUsername, _eConfig.SmtpPassword);

                client.Send(message);
                client.Disconnect(true);
            }
        }
    }

}