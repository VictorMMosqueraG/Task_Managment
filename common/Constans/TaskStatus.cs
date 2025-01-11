namespace TaskManagement.Constants{
    public static class TaskStatus{
        public const string Pending = "Pending";
        public const string InProgress = "InProgress";
        public const string Done = "Done";

        public static readonly List<string> AllStatuses = new List<string>{
            Pending,
            InProgress,
            Done
        };
    }
}