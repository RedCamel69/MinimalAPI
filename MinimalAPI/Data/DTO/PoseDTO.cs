using MinimalAPI.Data.Models;

namespace MinimalAPI.Data.DTO
{
    public class PoseDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? SanskritName { get; set; }
        public bool IsBeginnerFriendly { get; set; } = false;

        public PoseDTO() { }
        public PoseDTO(Pose pose) =>
        (Id, Name, SanskritName, IsBeginnerFriendly) = (pose.Id, pose.Name, pose.SanskritName, pose.IsBeginnerFriendly);
    }
}
