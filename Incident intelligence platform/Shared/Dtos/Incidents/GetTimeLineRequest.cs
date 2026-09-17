namespace Shared.Dtos.Incidents
{
    public class GetTimeLineRequest
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}