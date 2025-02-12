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

namespace Domain.Interfaces
{
    public interface ISendGridEmailService
    {
        public Task<Response> SendEmailAsync(string toEmail, string subject, string body, List<(byte[] FileData, string FileName)> files);
        public Task<Response> SendTransactionEventTicketEmail(Ticket ticket, Transaction transaction, string toEmail);
        public Task<Response> SendEmailConfirmationAsync(string toEmail, string confirmationLink);
    }
}