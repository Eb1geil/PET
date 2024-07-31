namespace PET1.Domain.Entities
{
    public class Game
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public int Price { get; set; }
        public string? ImgPath { get; set; }
        public string ImgType { get; set;}

    }
}
