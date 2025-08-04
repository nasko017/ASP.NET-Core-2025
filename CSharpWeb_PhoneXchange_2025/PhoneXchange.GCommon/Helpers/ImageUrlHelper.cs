
namespace PhoneXchange.GCommon.Helpers
{
    public static class ImageUrlHelper
    {
        public static List<string> Deserialize(string? serialized)
        {
            if (string.IsNullOrWhiteSpace(serialized))
                return new List<string>();

            return serialized
                .Split(';', StringSplitOptions.RemoveEmptyEntries)
                .ToList();
        }

        public static string Serialize(List<string>? urls)
        {
            return urls == null ? string.Empty : string.Join(";", urls);
        }
    }
}
