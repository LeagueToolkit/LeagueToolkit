namespace Fantome.Libraries.League.IO.BIN
{
    /// <summary>
    /// Represents an Interface that every BIN Value inherits from
    /// </summary>
    public interface IBINFileValue
    {
        /// <summary>
        /// Gets the size of this <see cref="IBINFileValue"/>
        /// </summary>
        int GetSize();
    }
}