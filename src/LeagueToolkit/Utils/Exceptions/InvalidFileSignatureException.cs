namespace LeagueToolkit.Utils.Exceptions;

public class InvalidFileSignatureException : Exception
{
    public InvalidFileSignatureException() : base("Invalid file signature") { }

    public InvalidFileSignatureException(string message) : base("Invalid file signature: " + message) { }
}
