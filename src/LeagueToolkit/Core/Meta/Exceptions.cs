namespace LeagueToolkit.Core.Meta;

public class InvalidPropertyTypeException : Exception
{
    public BinPropertyType PropertyType { get; }

    public InvalidPropertyTypeException(BinPropertyType propertyType) : base($"Invalid property type: {propertyType}")
    {
        this.PropertyType = propertyType;
    }
}
