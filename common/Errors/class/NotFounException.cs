public class NotFoundException : Exception{
    public NotFoundException(int permissionId, string value)
        : base($"{value} with ID {permissionId} not found."){}
}
