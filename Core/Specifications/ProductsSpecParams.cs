namespace Core.Specifications
{
    public class  ProductsSpecParams
    {
        private const int MaxPageSize=50;
        public int PageIndex{set;get;}=1;
        private int _pageSize=6;
        public int PageSize
        {
            get=>_pageSize;
            set=>_pageSize=(value>MaxPageSize)?MaxPageSize:value;
        }
        public int? BrandId{set;get;}
        public int? TypeId{set;get;}
        public string Sort{set;get;}
        public string _search;
        public string Search
        {
            get=>_search;
            set=>_search=value.ToLower();
        }



    }
}