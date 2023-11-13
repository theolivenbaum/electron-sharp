using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using static ElectronSharp.API.BridgeConnector;

namespace ElectronSharp.API.Entities.Serialization
{
    /// <summary>
    /// Converts releaseNotes attribute from (JSON older format coming from markdown) or from string to string.
    /// </summary>
    internal class ReleaseNotesConverter : JsonConverter
    {
        private static readonly CamelCaseNewtonsoftJsonSerializer Serializer = new();

        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException("Not intended to write, just read.");
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;

            try
            {
                if (reader.TokenType == JsonToken.String)
                {
                    // New format: string is used for release notes (array of VersionReleaseNotes format must be used).
                    // Return as is.
                    return reader.Value?.ToString();
                }

                if (reader.TokenType == JsonToken.StartArray)
                {
                    reader.Read();

                    // Old format: array of objects is used for release notes. Convert to string.
                    List<ReleaseNoteInfo> versionNotes = new();

                    while (reader.TokenType != JsonToken.EndArray)
                    {
                        versionNotes.Add(ReadVersionReleaseNote(reader));
                        reader.Read();
                    }

                    var jsonSerializeResult = Serializer.Serialize(versionNotes.ToArray());
                    return jsonSerializeResult.Json;
                }
            }
            catch (Exception ex)
            {
                throw new JsonSerializationException("Error converting release notes.", ex);
            }

            throw new FormatException($"Error converting release notes, unhandled toke type: {reader.TokenType}");
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException("Not intended to write, just read.");
        }

        private static ReleaseNoteInfo ReadVersionReleaseNote(JsonReader reader)
        {
            reader.Read();

            var dictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            while (reader.TokenType != JsonToken.EndObject)
            {
                if (reader.TokenType != JsonToken.PropertyName)
                    throw new FormatException($"Expected a property name, got {reader.TokenType}");
                string propertyName = reader.Value.ToString();
                reader.Read();

                if (reader.TokenType != JsonToken.String)
                    throw new FormatException($"Expected string, got {reader.TokenType}");
                string value = reader.Value.ToString();
                dictionary.Add(propertyName, value);
                reader.Read();
            }

            if (dictionary.Count != 2)
            {
                throw new FormatException($"Expected release version note object to have 2 keys, got {dictionary.Count}.");
            }

            if (!dictionary.ContainsKey("version") || !dictionary.ContainsKey("note"))
            {
                throw new FormatException($"Expected version and note keys, got: {string.Join(", ", dictionary.Keys)}");
            }

            var result = new ReleaseNoteInfo
            {
                Version = dictionary["version"],
                Note    = dictionary["note"]
            };

            return result;
        }

        /// <summary>
        /// Older format of release notes.
        /// </summary>
        internal class ReleaseNoteInfo
        {
            /// <summary>
            /// The version.
            /// </summary>
            public string Version { get; set; }

            /// <summary>
            /// The note.
            /// </summary>
            public string Note { get; set; }
        }
    }
}