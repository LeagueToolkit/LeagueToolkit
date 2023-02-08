namespace LeagueToolkit.Core.Meta;

public class InvalidPropertyTypeException : Exception
{
    public InvalidPropertyTypeException(BinPropertyType propertyType) : base($"Invalid property type: {propertyType}")
    { }
}
