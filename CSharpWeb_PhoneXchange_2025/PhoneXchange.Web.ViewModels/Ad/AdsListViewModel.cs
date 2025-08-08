namespace PhoneXchange.Web.ViewModels.Ad
{
    public class AdsListViewModel
    {
        public IEnumerable<AdViewModel> Ads { get; set; } = new List<AdViewModel>();
        public string? SearchTerm { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
