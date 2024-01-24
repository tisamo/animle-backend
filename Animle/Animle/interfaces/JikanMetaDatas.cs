using Animle.Models;

namespace Animle.Models
{
    using System;
    using System.Collections.Generic;

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);


    public class Broadcast
    {
        public string day { get; set; }
        public string time { get; set; }
        public string timezone { get; set; }
        public string @string { get; set; }
    }

    public class Datum
    {
        public int mal_id { get; set; }
       
        public Trailer trailer { get; set; }
      
    }

    public class Demographic
    {
        public int mal_id { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public string url { get; set; }
    }



    public class Genre
    {
        public int mal_id { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Images
    {
        public Jpg jpg { get; set; }
        public Webp webp { get; set; }
        public string image_url { get; set; }
        public string small_image_url { get; set; }
        public string medium_image_url { get; set; }
        public string large_image_url { get; set; }
        public string maximum_image_url { get; set; }
    }

    public class JikanImages
    {
        public List<Images> data { get; set; } = new List<Images>();
    }

    public class Items
    {
        public int count { get; set; }
        public int total { get; set; }
        public int per_page { get; set; }
    }

    public class Jpg
    {
        public string image_url { get; set; }
        public string small_image_url { get; set; }
        public string large_image_url { get; set; }
    }


    public class Pagination
    {
        public int last_visible_page { get; set; }
        public bool has_next_page { get; set; }
        public int current_page { get; set; }
        public Items items { get; set; }
    }




    public class JikanAnimeData
    {
        public Pagination pagination { get; set; }
        public List<Datum> data { get; set; }
    }




    public class Title
    {
        public string type { get; set; }
        public string title { get; set; }
    }


    public class Trailer
    {
        public string youtube_id { get; set; }
        public string url { get; set; }
        public string embed_url { get; set; }
        public Images images { get; set; }
    }

    public class Webp
    {
        public string image_url { get; set; }
        public string small_image_url { get; set; }
        public string large_image_url { get; set; }
    }


}



