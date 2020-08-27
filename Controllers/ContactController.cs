using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PersonalSite.Models;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace PersonalSite.Controllers
{
    public class ContactController : Controller
    {
        private  readonly EmailAddress FromAndToEmailAddress;
        private readonly IEmailService EmailService;
        public ContactController(EmailAddress _fromAddress,
            IEmailService _emailService)
        {
            FromAndToEmailAddress = _fromAddress;
            EmailService = _emailService;
        }
        [HttpGet]
        [Route("ContactMe")]
        public IActionResult ContactMe()
        {
            if (HttpContext.Session.GetInt32("id") == null)
            {
                return RedirectToAction("Index", "User");
            }
            return View();
        }

        [HttpPost]
        [Route("ContactMe")]
        public IActionResult FormHandler(ContactFormModel model)
        {
            if (ModelState.IsValid)
            {
                EmailMessage msgToSend = new EmailMessage
                {
                    FromAddresses = new List<EmailAddress> { FromAndToEmailAddress },
                    ToAddresses = new List<EmailAddress> { FromAndToEmailAddress },
                    Content = $"Here is your message: Name: {model.Name}, " +
                        $"Email: {model.Email}, Message: {model.Message}",
                    Subject = "Contact Form - BasicContactForm App"
                };

                EmailService.Send(msgToSend);
                return RedirectToAction("ContactMe");
            }
            else
            {
                return RedirectToAction("ContactMe");
            }
        }
    }
}