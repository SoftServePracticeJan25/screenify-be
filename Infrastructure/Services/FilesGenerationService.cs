using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.DataAccess;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using System.IO;
using Domain.DTOs.Data.TicketDtos;
using AutoMapper;
using Domain.DTOs.Data.TransactionDtos;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class FilesGenerationService : IFilesGenerationService
    {
        private readonly MovieDbContext _context;
        private readonly IMapper _mapper;

        public FilesGenerationService(MovieDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<byte[]> GenerateTicketPdfAsync(int ticketId)
        {
            var ticket = await _context.Tickets
                .Include(t => t.Session)  
                .ThenInclude(s => s.Movie)  
                .Include(t => t.Session)   
                .ThenInclude(s => s.Room)  
                .FirstOrDefaultAsync(t => t.Id == ticketId);

            if (ticket == null)
            {
                throw new Exception("Ticket not found.");
            }

            return GenerateTicketPdf(ticket);
        }

        public byte[] GenerateTicketPdf(Ticket ticket)
        {
            var ticketFileDto = _mapper.Map<TicketFileDto>(ticket);

            Document document = new Document();
            Section section = document.AddSection();

            Style style = document.Styles.AddStyle("CourierStyle", "Normal");
            style.Font.Name = "Courier New";
            style.Font.Size = 12;

            Paragraph title = section.AddParagraph("Screenify\n");
            title.Style = "CourierStyle";  

            title.Format.Font.Size = 14;
            title.Format.Font.Bold = true;
            title.Format.SpaceAfter = 10;

            section.AddParagraph($"Movie: {ticketFileDto.Title}").Style = "CourierStyle";
            section.AddParagraph($"Room: {ticketFileDto.Name}").Style = "CourierStyle";
            section.AddParagraph($"Seat: {ticketFileDto.SeatNum}").Style = "CourierStyle";
            section.AddParagraph($"Price: {ticketFileDto.Price}").Style = "CourierStyle";
            section.AddParagraph($"Start time: {ticketFileDto.StartTime}").Style = "CourierStyle";
            
            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(true);
            pdfRenderer.Document = document;
            pdfRenderer.RenderDocument();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                pdfRenderer.PdfDocument.Save(memoryStream, false);
                return memoryStream.ToArray();
            }
        }

        public byte[] GenerateInvoice(Transaction transaction)
        {
            var transactionDto = _mapper.Map<TransactionReadDto>(transaction);

            Document document = new Document();
            Section section = document.AddSection();

            Style style = document.Styles.AddStyle("CourierStyle", "Normal");
            style.Font.Name = "Courier New";
            style.Font.Size = 12;

            Paragraph title = section.AddParagraph("Transaction\n");
            title.Style = "CourierStyle"; 

            title.Format.Font.Size = 14;
            title.Format.Font.Bold = true;
            title.Format.SpaceAfter = 10;

            section.AddParagraph($"Id: {transactionDto.Id}").Style = "CourierStyle";
            section.AddParagraph($"Sum: {transactionDto.Sum}").Style = "CourierStyle";
            section.AddParagraph($"Date time: {transactionDto.CreationTime}").Style = "CourierStyle";
            
            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(true);
            pdfRenderer.Document = document;
            pdfRenderer.RenderDocument();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                pdfRenderer.PdfDocument.Save(memoryStream, false);
                return memoryStream.ToArray();
            }
        }

        public byte[] GenerateCalendarEvent(Ticket ticket)
        {
            var ticketNotifDto = _mapper.Map<TicketNotifDto>(ticket);

            string description = $"Don't miss {ticketNotifDto.Title} in Screenify cinema!";

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("BEGIN:VCALENDAR");
            sb.AppendLine("VERSION:2.0");
            sb.AppendLine("PRODID:-//Screenify//Screenify//EN");
            sb.AppendLine("BEGIN:VEVENT");
            sb.AppendLine($"UID:{Guid.NewGuid()}");
            sb.AppendLine($"DTSTAMP:{DateTime.UtcNow:yyyyMMddTHHmmssZ}");
            sb.AppendLine($"DTSTART:{ticketNotifDto.StartTime:yyyyMMddTHHmmss}");
            sb.AppendLine($"DTEND:{ticketNotifDto.EndTime:yyyyMMddTHHmmss}");
            sb.AppendLine($"SUMMARY:{ticketNotifDto.Title}");
            sb.AppendLine($"LOCATION:{ticketNotifDto.Adress}");
            sb.AppendLine($"DESCRIPTION:{description}");
            sb.AppendLine("END:VEVENT");
            sb.AppendLine("END:VCALENDAR");

            return Encoding.UTF8.GetBytes(sb.ToString());
        }
    }
}
