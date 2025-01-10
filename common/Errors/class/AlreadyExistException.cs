public class AlreadyExistException: Exception{

    public AlreadyExistException(string value)
        :base($"this {value} already exits"){}
}