namespace Star_Wars_API_Wrapper.Models
{
    public class film
    {
        public string title { get; set; }
        public string episode_id { get; set; }

        public string opening_crawl { get; set; }
       
        public string director {  get; set; }

        public string producer { get; set; }

        public DateOnly release_date { get; set; }

        public List<string> species { get; set; }

        public List<string> starships { get; set; }

        public List<string> vehicles { get; set; }

        public List<string> characters { get; set; }

        public List<string> planets { get; set; }

        public string url { get; set; }

        public string created {  get; set; }

        public string edited { get; set; }

    }
}
