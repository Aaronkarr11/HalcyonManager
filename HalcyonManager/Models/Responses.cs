namespace HalcyonManager.Models
{
    public class WorkItemResponse
    {
        public int id { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public string project { get; set; }
        public string deletedDate { get; set; }
        public string deletedBy { get; set; }
        public int code { get; set; }
        public string url { get; set; }
    }

    public class CreateOrUpdateWorkItemExceptionResponse
    {
        public string id { get; set; }
        public object innerException { get; set; }
        public string message { get; set; }
        public string typeName { get; set; }
        public string typeKey { get; set; }
        public int errorCode { get; set; }
        public int eventId { get; set; }
    }

    public class RequestItemResponse
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public string Title { get; set; }
        public string ReasonDescription { get; set; }
        public DateTime? DesiredDate { get; set; }
        public int IsFulfilled { get; set; }
    }

    public class WorkTaskCompletedStats
    {
        public double PercentCompleted { get; set; }
        public double PercentUnCompleted { get; set; }
        public int CompletedCount { get; set; }
        public int UnCompletedCount { get; set; }
    }
}
