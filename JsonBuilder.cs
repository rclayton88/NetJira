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
                return string.Format("\"customfield_{0}\": null", customFieldId);
            }

            // Example: "customfield_10008": [ {"value": "red" }, {"value": "blue" }, {"value": "green" }]
            customFieldId = customFieldId ?? "";
            var value = string.Join(",", values.Select(v => string.Format("{{\"value\":\"{0}\"}}", EscapeCharacters(v))));
            return string.Format("\"customfield_{0}\": [{1}]", customFieldId.Trim(), value);
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
                return string.Format("\"customfield_{0}\": null", customFieldId);
            }
            customFieldId = customFieldId ?? "";
            var formattedValue = string.Format("{{\"value\":\"{0}\"}}", EscapeCharacters(value));
            return string.Format("\"customfield_{0}\": {1}", customFieldId.Trim(), formattedValue);
        }

        public static string SetProjectKey(string projectKey)
        {
            return string.Format("\"project\":{{\"key\":\"{0}\"}}", projectKey);
        }

        public static string SetIssueType(string issueType)
        {
            return string.Format("\"issuetype\": {{\"name\":\"{0}\"}}", issueType);
        }

        public static string SetKeyValue(string key, string value)
        {
            return string.Format("\"{0}\":\"{1}\"", key, EscapeCharacters(value));
        }

        public static string SetPriorityById(int priorityId)
        {
            return string.Format("\"priority\":{{\"id\":\"{0}\"}}", priorityId);
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
