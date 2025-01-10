public class PermissionNotFoundException : Exception{
    public PermissionNotFoundException(int permissionId)
        : base($"Permission with ID {permissionId} not found."){}
}
