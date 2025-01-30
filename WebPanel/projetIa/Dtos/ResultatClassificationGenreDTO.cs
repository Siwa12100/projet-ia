namespace projetIa.Dtos
{
    public class ResultatClassificationGenreDTO
    {
        public string? gender { get; set; } // female | male
        public float? time { get; set; }
        public string? message { get; set; }
        public string? description { get; set; }
        public int? code { get; set; }
    }
}