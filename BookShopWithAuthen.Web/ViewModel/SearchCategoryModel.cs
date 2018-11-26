namespace BookShopWithAuthen.Web.ViewModel
{
    public class SearchCategoryModel
    {
        public string SearchValue { get; set; }
        public int ID { get; set; }
        public int PriceFrom { get; set; }
        public int PriceTo { get; set; }
        public int AuthorID { get; set; }
        public int sortBy { get; set; }
        public int Page { get; set; }
        public SearchCategoryModel()
        {
            Page = 1;
            sortBy = (int)sortType.orderByNew;
            SearchValue = "";
            ID = -1;
            PriceFrom = 0;
            PriceTo = 700;
            AuthorID = -1;
        }
    }
    public enum sortType
    {
        orderByPriceHigh = 0,
        orderByPriceLow = 1,
        orderBySell = 2,
        orderByNew = 3,
        orderByRate = 4
    }
}