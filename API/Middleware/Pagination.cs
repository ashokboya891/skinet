namespace API.Middleware
{
    public class Pagination<T> where T: class
    {
        public Pagination(int pageIndex, int pageSize, int count, IReadOnlyList<T> data)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            Count = count;
            Data = data;
        }

        public int PageIndex{set;get;}
        public int PageSize{set;get;}
        public int Count{set;get;}
        public IReadOnlyList<T> Data{get;set;}
    }
}