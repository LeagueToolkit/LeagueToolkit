namespace LeagueToolkit.Utils.Exceptions;

public class InvalidFileVersionException : Exception
{
    public InvalidFileVersionException() : base("Unsupported file Version") { }
}
