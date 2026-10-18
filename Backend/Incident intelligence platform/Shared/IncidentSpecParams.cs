using Domain.Enums.Incident;
using Shared.Enums;

namespace Shared
{
    public class IncidentSpecParams
    {
        private const int MaxPageSize = 50;
        public int PageIndex { get; set; } = 1;

        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        public int? ServiceId { get; set; }
        public IncidentStatus? Status { get; set; }
        public IncidentSort? Sort { get; set; }
        public string? Search { get; set; }
    }
}

