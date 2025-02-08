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
        public byte[] GenerateTicketPdf(Ticket ticket)
        {
            var ticketFileDto = _mapper.Map<TicketFileDto>(ticket);

            // Создаем новый документ
            Document document = new Document();
            Section section = document.AddSection();

            // Создаем стиль для шрифта Arial
            Style style = document.Styles.AddStyle("CourierStyle", "Normal");
            style.Font.Name = "Courier New";
            style.Font.Size = 12;

            // Устанавливаем шрифт Arial для титульной строки
            Paragraph title = section.AddParagraph("Screenify\n");
            title.Style = "CourierStyle";  // Применяем стиль Arial ко всем параграфам

            title.Format.Font.Size = 14;
            title.Format.Font.Bold = true;
            title.Format.SpaceAfter = 10;

            // Добавляем остальные параграфы с шрифтом Arial
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
    }
}