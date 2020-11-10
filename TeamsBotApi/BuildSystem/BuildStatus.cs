namespace BuildSystem
{
    public enum BuildStatus
    {
        None,
        Queued,
        Running,
        Cancelled,
        Failed,
        Succeeded,
    }
}