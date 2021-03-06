namespace RightpointLabs.ConferenceRoom.Domain.Models.Entities
{
    public class MeetingEntity : Entity, IByOrganizationId
    {
        public string OrganizationId { get; set; }
        public string UniqueId { get; set; }
        public bool IsStarted { get; set; }
        public bool IsEndedEarly { get; set; }
        public bool IsCancelled { get; set; }
    }
}