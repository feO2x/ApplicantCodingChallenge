using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;
using JsonException = System.Text.Json.JsonException;

namespace Hahn.ApplicationProcess.December2020.Web.Infrastructure
{
    public static class Json
    {
        public static IMvcBuilder AddJsonOptions(this IMvcBuilder mvcBuilder) =>
            mvcBuilder.AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new DateTimeConverter()));
    }

    public sealed class DateTimeConverter : JsonConverter<DateTime>
    {
        private const string DateFormat = "yyyy-MM-dd";
        private IFormatProvider InvariantCulture { get; } = CultureInfo.InvariantCulture;

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var dateTimeValue = reader.GetString();

            if (DateTime.TryParseExact(dateTimeValue, DateFormat, InvariantCulture, DateTimeStyles.None, out var result))
                return result;
                
            throw new JsonException($"The value \"{dateTimeValue}\" cannot be parsed to a date time value in the format \"{DateFormat}\".");
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(DateFormat, InvariantCulture));
        }
    }
}