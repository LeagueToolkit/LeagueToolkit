namespace Fantome.Libraries.League.IO.BIN
{
    public interface IBINValue
    {
        IBINValue Parent { get; }
        BINValue this[string path] { get; }
        BINValue this[uint hash] { get; }
        uint GetSize();
        string GetPath(bool excludeEntry = true);
    }
}
