using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
namespace Domain.Interfaces
{
    public interface IFilesGenerationService
    {
        public Task<byte[]> GenerateTicketPdfAsync(int ticketId);
        byte[] GenerateTicketPdf(Ticket ticket);
        public byte[] GenerateInvoice(Transaction transaction);
        public byte[] GenerateCalendarEvent(Ticket ticket);
    }
}