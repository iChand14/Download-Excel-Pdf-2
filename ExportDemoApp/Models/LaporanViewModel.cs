namespace ExportDemoApp.Models
{
    public class LaporanViewModel
    {
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public List<LaporanModel> LaporanList { get; set; }
    }
}
