using Microsoft.AspNetCore.Mvc;
using ExportDemoApp.Models;
using ExportDemoApp.Repositories;
using ClosedXML.Excel;
using Rotativa.AspNetCore;
using System.IO;

namespace ExportDemoApp.Controllers
{
    public class ExportController : Controller
    {
        private readonly LaporanRepository _repository;

        public ExportController(IConfiguration configuration)
        {
            _repository = new LaporanRepository(configuration);
        }

        public IActionResult Index()
        {
            try
            {
                var data = _repository.GetLaporan();
                ViewBag.KoneksiStatus = "✅ Koneksi berhasil ke PostgreSQL.";
                return View(data);
            }
            catch (Exception ex)
            {
                ViewBag.KoneksiStatus = $"❌ Gagal koneksi: {ex.Message}";
                return View(new List<LaporanModel>());
            }
        }

        public IActionResult DownloadExcel()
        {
            var data = _repository.GetLaporan();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Laporan");

            worksheet.Cell(1, 1).Value = "Nama";
            worksheet.Cell(1, 2).Value = "Tanggal";

            for (int i = 0; i < data.Count; i++)
            {
                worksheet.Cell(i + 2, 1).Value = data[i].Nama;
                worksheet.Cell(i + 2, 2).Value = data[i].Tanggal.ToString("dd/MM/yyyy");
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);

            return File(stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Laporan.xlsx");
        }

        public IActionResult DownloadPDF()
        {
            var data = _repository.GetLaporan(); // Ambil data dari database

            var viewModel = new LaporanViewModel
            {
                Title = "Laporan Mingguan",
                Date = DateTime.Now,
                LaporanList = data
            };

            return new ViewAsPdf("PdfView", viewModel)
            {
                FileName = "Laporan.pdf",
                PageSize = Rotativa.AspNetCore.Options.Size.A4
            };
        }
    }
}
