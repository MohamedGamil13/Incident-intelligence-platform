using Domain.Entities.Services;
using Domain.Enums.Logs;

namespace Domain.Entities.Logs
{
    public class Log
    {
        public int Id { get; set; } //Primary Key only For DB 
        public LogLevel LogLevel { get; set; }
        public string Message { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public Guid TraceId { get; set; } // Will Be Genrated By TraceId Middleware
        public int ServiceId { get; set; } //Forign key For Service Table (1-M)
        public Service Service { get; set; }  // Navigation Proprity (In Case i wanna To get a Service By it's Logs ) 


    }
}
//Models => Create LogModel and it's Relations , Create it's Config Class and Register it in onModelCreateing and add migration and Update DB and Ensure it's Updated Correct ,Handle Indexing
//Genrate TraceId Middleware


//New entity: Log — Timestamp, Level, Message, ServiceId, TraceId.