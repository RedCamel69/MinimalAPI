namespace MinimalAPI.Data.Models
{
    public class Pose
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? SanskritName { get; set; }
        public bool IsBeginnerFriendly { get; set; } = false;

        public string? TutorComments { get; set; } = "private notes...";

       
    }
}
