using System.Collections.Generic;
using System.Linq;


namespace NetJira
{
    /// <summary>
    /// Utility class to help with Jira operations.
    /// </summary>
    public static class JsonBuilder
    {
        /// <summary>
        /// Builds the JSON needed set the values of a multiselect custom field.
        /// </summary>
        /// <param name="customFieldId">ID of the custom field (just the number).</param>
        /// <param name="values">Values to set the field to.</param>
        /// <returns>JSON needed to update the field.</returns>
        public static string SetMultiSelectValues(string customFieldId, IEnumerable<string> values)
        {
            if (values == null)
            {
                return $"\"customfield_{customFieldId}\": null";
            }

            // Example: "customfield_10008": [ {"value": "red" }, {"value": "blue" }, {"value": "green" }]
            customFieldId = customFieldId ?? "";
            var value = string.Join(",", values.Select(v => string.Format("{{\"value\":\"{0}\"}}", EscapeCharacters(v))));
            return $"\"customfield_{customFieldId.Trim()}\": [{value}]";
        }

        /// <summary>
        /// Builds the JSON needed to update a single select custom field.
        /// </summary>
        /// <param name="customFieldId">ID of the custom field (just the number).</param>
        /// <param name="value">Value to set the field to.</param>
        /// <returns>JSON needed to update the field.</returns>
        public static string SetSingleSelectListValue(string customFieldId, string value)
        {
            // Example: "customfield_10008": [ {"value": "red" }, {"value": "blue" }, {"value": "green" }]
            if (value == null)
            {
                return $"\"customfield_{customFieldId}\": null";
            }
            customFieldId = customFieldId ?? "";
            var formattedValue = string.Format("{{\"value\":\"{0}\"}}", EscapeCharacters(value));
            return $"\"customfield_{customFieldId.Trim()}\": {formattedValue}";
        }

        public static string SetProjectKey(string projectKey)
        {
            return $"\"project\":{{\"key\":\"{projectKey}\"}}";
        }

        public static string SetIssueType(string issueType)
        {
            return $"\"issuetype\": {{\"name\":\"{issueType}\"}}";
        }

        public static string SetKeyValue(string key, string value)
        {
            return $"\"{key}\":\"{EscapeCharacters(value)}\"";
        }

        public static string SetPriorityById(int priorityId)
        {
            return $"\"priority\":{{\"id\":\"{priorityId}\"}}";
        }

        private static string EscapeCharacters(string str)
        {
            str = str.Replace("\t", "\\t");
            str = str.Replace("\r", "\\r");
            str = str.Replace("\n", "\\n");
            str = str.Replace("\"", "\\\"");
            return str;
        }
    }
}
