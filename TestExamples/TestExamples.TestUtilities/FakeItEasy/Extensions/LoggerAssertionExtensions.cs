using FakeItEasy;
using FakeItEasy.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace TestExamples.TestUtilities.FakeItEasy.Extensions
{
    public static class LoggerAssertionExtensions
    {
        public static IVoidArgumentValidationConfiguration VerifyLogObject<T>(this ILogger<T> logger, LogLevel level, object value)
        {
            return A.CallTo(logger)
                .Where(call => call.Method.Name == "Log"
                    && call.GetArgument<LogLevel>(0) == level
                    && CheckLogArgument(call.GetArgument<object>(2), value));
        }

        private static bool CheckLogArgument(object argument, object value)
        {
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            var serializedArgument = JsonSerializer.Serialize(argument, options);
            var serializedValue = JsonSerializer.Serialize(value, options);

            return serializedArgument.Contains(serializedValue);
        }

        public static IVoidArgumentValidationConfiguration VerifyLogMessage<T>(this ILogger<T> logger, LogLevel level, string message)
        {
            return A.CallTo(logger)
                .Where(call => call.Method.Name == "Log"
                    && call.GetArgument<LogLevel>(0) == level
                    && CheckLogMessages(call.GetArgument<IReadOnlyList<KeyValuePair<string, object>>>(2), message));
        }

        private static bool CheckLogMessages(IReadOnlyList<KeyValuePair<string, object>> readOnlyLists, string message)
        {
            foreach (var kvp in readOnlyLists)
            {
                if (kvp.Value.ToString().Contains(message))
                    return true;
            }

            return false;
        }
    }
}
