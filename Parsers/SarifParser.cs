using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ParasoftReportExtension.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ParasoftReportExtension.Parsers
{
    public class SarifParser
    {
        public static List<ErrorInfo> ParseSarifFile(string filePath)
        {
            var errors = new List<ErrorInfo>();

            try
            {
                var json = File.ReadAllText(filePath);
                var sarif = JObject.Parse(json);

                var runs = sarif["runs"] as JArray;
                if (runs == null) return errors;

                foreach (var run in runs)
                {
                    var results = run["results"] as JArray;
                    if (results == null) continue;

                    var rules = ExtractRules(run["tool"]?["driver"]?["rules"] as JArray);

                    foreach (var result in results)
                    {
                        var ruleId = result["ruleId"]?.ToString();
                        var message = result["message"]?["text"]?.ToString();
                        var level = result["level"]?.ToString();

                        var locations = result["locations"] as JArray;
                        if (locations != null)
                        {
                            foreach (var location in locations)
                            {
                                var physicalLocation = location["physicalLocation"];
                                var artifactLocation = physicalLocation?["artifactLocation"];
                                var region = physicalLocation?["region"];

                                var fileName = artifactLocation?["uri"]?.ToString();
                                var lineNumber = region?["startLine"]?.Value<int>() ?? 0;
                                var columnNumber = region?["startColumn"]?.Value<int>() ?? 0;

                                var errorInfo = new ErrorInfo
                                {
                                    RuleId = ruleId,
                                    Message = message,
                                    FileName = fileName,
                                    LineNumber = lineNumber,
                                    ColumnNumber = columnNumber,
                                    Severity = MapSeverity(level),
                                    Category = "Parasoft C++TEST",
                                    Description = GetRuleDescription(rules, ruleId)
                                };

                                errors.Add(errorInfo);
                            }
                        }
                        else
                        {
                            // Handle results without locations
                            var errorInfo = new ErrorInfo
                            {
                                RuleId = ruleId,
                                Message = message,
                                FileName = "",
                                LineNumber = 0,
                                ColumnNumber = 0,
                                Severity = MapSeverity(level),
                                Category = "Parasoft C++TEST",
                                Description = GetRuleDescription(rules, ruleId)
                            };

                            errors.Add(errorInfo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var errorInfo = new ErrorInfo
                {
                    RuleId = "PARSER_ERROR",
                    Message = $"Error parsing SARIF file: {ex.Message}",
                    FileName = filePath,
                    LineNumber = 0,
                    ColumnNumber = 0,
                    Severity = ErrorSeverity.Error,
                    Category = "Parasoft C++TEST",
                    Description = "Failed to parse SARIF file"
                };

                errors.Add(errorInfo);
            }

            return errors;
        }

        private static Dictionary<string, string> ExtractRules(JArray rules)
        {
            var ruleDict = new Dictionary<string, string>();
            
            if (rules == null) return ruleDict;

            foreach (var rule in rules)
            {
                var id = rule["id"]?.ToString();
                var description = rule["shortDescription"]?["text"]?.ToString() ?? 
                                rule["fullDescription"]?["text"]?.ToString();

                if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(description))
                {
                    ruleDict[id] = description;
                }
            }

            return ruleDict;
        }

        private static string GetRuleDescription(Dictionary<string, string> rules, string ruleId)
        {
            return rules.ContainsKey(ruleId) ? rules[ruleId] : "";
        }

        private static ErrorSeverity MapSeverity(string level)
        {
            switch (level?.ToLower())
            {
                case "error":
                    return ErrorSeverity.Error;
                case "warning":
                    return ErrorSeverity.Warning;
                case "info":
                case "note":
                    return ErrorSeverity.Information;
                default:
                    return ErrorSeverity.Warning;
            }
        }
    }
}
