using FixWithCustomSerialization.Controllers;
using System;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FixWithCustomSerialization.Services
{
    public class MediaFileJsonConverter : JsonConverter<MediaFile>
    {
        private readonly IWebHostEnvironment _environment;

        public MediaFileJsonConverter(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public override MediaFile Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = new StringBuilder();
            while (reader.Read())
            {
                value.Append(reader.GetString());
            }

            MediaFile mediaFile = JsonSerializer.Deserialize<MediaFile>(value.ToString(), options) ?? new MediaFile();

            var filePath = Path.Combine(_environment.WebRootPath, "images", mediaFile.Name);
            File.WriteAllBytes(filePath, Convert.FromBase64String(mediaFile.ImageDataB64));

            return mediaFile;
        }

        public override void Write(Utf8JsonWriter writer, MediaFile value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("PublicUrl");
            writer.WriteStringValue($"http://example.com/images/{value.Name}");

            writer.WriteEndObject();
        }
    }
}
