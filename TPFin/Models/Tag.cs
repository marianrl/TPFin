using System.Collections.Generic;

namespace TPFin.Models
{
    public class Tag
    {
        public int id { get; set; }
        public string palabra { get; set; }
        public ICollection<Post> Post { get; } = new List<Post>();
        public List<PostsTags> PostsTags { get; set; }

        public Tag() { }
        public Tag(string palabra)
        {
            this.id = id;
            this.palabra = palabra;
        }
        public Tag(int id,string palabra, List<Post> posts)
        {
            this.id = id;
            this.palabra = palabra;
        }
    }
}
