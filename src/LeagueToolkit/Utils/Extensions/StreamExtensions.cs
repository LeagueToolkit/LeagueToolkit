namespace LeagueToolkit.Utils.Extensions
{
    public static class StreamExtensions
    {
        public static void ReadExact(this Stream stream, Span<byte> buffer)
        {
            int totalBytesRead = 0;
            int bytesRead;
            do
            {
                bytesRead = stream.Read(buffer[totalBytesRead..]);
                totalBytesRead += bytesRead;
            } while (bytesRead is not 0);

            if (totalBytesRead != buffer.Length)
                throw new IOException($"Failed to read {buffer.Length} bytes, bytesRead: {totalBytesRead}");
        }
    }
}
