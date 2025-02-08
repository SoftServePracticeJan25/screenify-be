using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
namespace Domain.Interfaces
{
    public interface IFilesGenerationService
    {
        byte[] GenerateTicketPdf(Ticket ticket);
        public byte[] GenerateInvoice(Transaction transaction);
        public string GenerateCalendarEvent(Ticket ticket);
    }
}