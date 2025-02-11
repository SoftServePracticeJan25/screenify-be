using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.IO;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Services
{
    public class SendGridEmailService : ISendGridEmailService
    {
        private readonly string _apiKey;
        private readonly string _fromEmail;
        private readonly string _fromName;
        private readonly IFilesGenerationService _filesGenerationService;
        public SendGridEmailService(IConfiguration configuration, IFilesGenerationService filesGenerationService)
        {
            _apiKey = configuration["SendGrid:ApiKey"];
            _fromEmail = configuration["SendGrid:FromEmail"];
            _fromName = configuration["SendGrid:FromName"];

            _filesGenerationService = filesGenerationService;
        }

        public async Task<Response> SendEmailAsync(string toEmail, string subject, string body, List<(byte[] FileData, string FileName)>? files)
        {
            var client = new SendGridClient(_apiKey);
            var from = new EmailAddress(_fromEmail, _fromName);
            var to = new EmailAddress(toEmail);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, "", body);

            if ( files != null && files.Count != 0 )
            {
                foreach (var file in files)
                {
                    var fileBase64 = Convert.ToBase64String(file.FileData);
                    msg.AddAttachment(file.FileName, fileBase64);
                }
            }

            var response = await client.SendEmailAsync(msg);

            return response;
        }
        public async Task<Response> SendTransactionEventTicketEmail(Ticket ticket, Transaction transaction, string toEmail)
        {
            string subject = "Your ticket and transaction files from Screenify";
            string body = "Here is your files";

            byte[] invoicePdf = _filesGenerationService.GenerateInvoice(transaction);
            byte[] ticketPdf = await _filesGenerationService.GenerateTicketPdfAsync(ticket.Id);
            byte[] calendarFile = _filesGenerationService.GenerateCalendarEvent(ticket);

            var files = new List<(byte[], string)>
            {
                (invoicePdf, "Invoice.pdf"),
                (ticketPdf, "Ticket.pdf"),
                (calendarFile, "Event.ics") 
            };

            var response = await SendEmailAsync(toEmail, subject, body, files);
            return response;
        }

        public async Task<Response> SendEmailConfirmationAsync(string toEmail, string confirmationLink)
        {
            string subject = "Confirm your email";
            string body = $"Please confirm your email by clicking <a href='{confirmationLink}'>here</a>.";

            return await SendEmailAsync(toEmail, subject, body, null);
        }

    }
}
