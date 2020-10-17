using System.Text.Json;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;

namespace sample.microservice.reservation_grpc
{
    public static class Extensions
    {

        public static Any ConvertToAnyTypeAsync<T>(this T data)
        {
            var any = new Any();
            if (data == null)
                return any;

            var options = new JsonSerializerOptions();
            options.PropertyNameCaseInsensitive = true;
            options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;

            var bytes = JsonSerializer.SerializeToUtf8Bytes(data, options);
            any.Value = ByteString.CopyFrom(bytes);

            return any;
        }

        public static T ConvertFromAnyTypeAsync<T>(this Any any)
        {
            var options = new JsonSerializerOptions();
            options.PropertyNameCaseInsensitive = true;
            options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;

            var utf8String = any.Value.ToStringUtf8();
            return JsonSerializer.Deserialize<T>(utf8String, options);
        }
    }
}