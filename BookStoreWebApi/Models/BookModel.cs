namespace BookStoreWebApi.Models
{
    public class BookModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string desc { get; set; }
        public int price { get; set; } 

        public AuthorModel author { get; set; }
        public PublisherModel publisher { get; set; }

    }
}
