namespace Shared.Dtos.IcidentEventDTOs
{
    public class GetIncidentEventResponse
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string TimeStamp { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public int IncidentId { get; set; }
    }
}
