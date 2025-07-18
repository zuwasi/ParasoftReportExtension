using HtmlAgilityPack;
using ParasoftReportExtension.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ParasoftReportExtension.Parsers
{
    public class HtmlParser
    {
        public static List<ErrorInfo> ParseHtmlFile(string filePath)
        {
            var errors = new List<ErrorInfo>();

            try
            {
                var html = File.ReadAllText(filePath);
                var doc = new HtmlDocument();
                doc.LoadHtml(html);

                // Try multiple strategies to parse Parasoft HTML reports
                errors.AddRange(ParseTableBasedReport(doc));
                errors.AddRange(ParseListBasedReport(doc));
                errors.AddRange(ParseDivBasedReport(doc));
            }
            catch (Exception ex)
            {
                var errorInfo = new ErrorInfo
                {
                    RuleId = "PARSER_ERROR",
                    Message = $"Error parsing HTML file: {ex.Message}",
                    FileName = filePath,
                    LineNumber = 0,
                    ColumnNumber = 0,
                    Severity = ErrorSeverity.Error,
                    Category = "Parasoft C++TEST",
                    Description = "Failed to parse HTML file"
                };

                errors.Add(errorInfo);
            }

            return errors;
        }

        private static List<ErrorInfo> ParseTableBasedReport(HtmlDocument doc)
        {
            var errors = new List<ErrorInfo>();

            // Look for table-based reports
            var tables = doc.DocumentNode.SelectNodes("//table");
            if (tables == null) return errors;

            foreach (var table in tables)
            {
                var rows = table.SelectNodes(".//tr");
                if (rows == null || rows.Count < 2) continue;

                // Try to identify header row
                var headerRow = rows[0];
                var headers = headerRow.SelectNodes(".//th|.//td")?.Select(th => th.InnerText.Trim().ToLower()).ToList();
                
                if (headers == null) continue;

                // Find column indices
                var fileIndex = FindColumnIndex(headers, new[] { "file", "filename", "source", "location" });
                var lineIndex = FindColumnIndex(headers, new[] { "line", "line number", "row" });
                var ruleIndex = FindColumnIndex(headers, new[] { "rule", "rule id", "violation", "check" });
                var messageIndex = FindColumnIndex(headers, new[] { "message", "description", "details" });
                var severityIndex = FindColumnIndex(headers, new[] { "severity", "level", "priority" });

                // Parse data rows
                for (int i = 1; i < rows.Count; i++)
                {
                    var row = rows[i];
                    var cells = row.SelectNodes(".//td");
                    if (cells == null || cells.Count < Math.Max(fileIndex, Math.Max(lineIndex, Math.Max(ruleIndex, messageIndex)))) continue;

                    var fileName = GetCellValue(cells, fileIndex);
                    var lineNumber = ParseInt(GetCellValue(cells, lineIndex));
                    var ruleId = GetCellValue(cells, ruleIndex);
                    var message = GetCellValue(cells, messageIndex);
                    var severity = GetCellValue(cells, severityIndex);

                    if (!string.IsNullOrEmpty(ruleId) || !string.IsNullOrEmpty(message))
                    {
                        errors.Add(new ErrorInfo
                        {
                            RuleId = ruleId,
                            Message = message,
                            FileName = fileName,
                            LineNumber = lineNumber,
                            ColumnNumber = 0,
                            Severity = MapSeverityFromText(severity),
                            Category = "Parasoft C++TEST",
                            Description = message
                        });
                    }
                }
            }

            return errors;
        }

        private static List<ErrorInfo> ParseListBasedReport(HtmlDocument doc)
        {
            var errors = new List<ErrorInfo>();

            // Look for list-based reports
            var violationNodes = doc.DocumentNode.SelectNodes("//div[contains(@class, 'violation')] | //li[contains(@class, 'violation')] | //div[contains(@class, 'error')] | //div[contains(@class, 'warning')]");
            if (violationNodes == null) return errors;

            foreach (var node in violationNodes)
            {
                var fileName = ExtractFromNode(node, new[] { "file", "filename", "source" });
                var lineNumber = ParseInt(ExtractFromNode(node, new[] { "line", "row" }));
                var ruleId = ExtractFromNode(node, new[] { "rule", "id", "violation" });
                var message = ExtractFromNode(node, new[] { "message", "description", "text" });
                var severity = ExtractFromNode(node, new[] { "severity", "level" });

                if (!string.IsNullOrEmpty(ruleId) || !string.IsNullOrEmpty(message))
                {
                    errors.Add(new ErrorInfo
                    {
                        RuleId = ruleId,
                        Message = message,
                        FileName = fileName,
                        LineNumber = lineNumber,
                        ColumnNumber = 0,
                        Severity = MapSeverityFromText(severity),
                        Category = "Parasoft C++TEST",
                        Description = message
                    });
                }
            }

            return errors;
        }

        private static List<ErrorInfo> ParseDivBasedReport(HtmlDocument doc)
        {
            var errors = new List<ErrorInfo>();

            // Look for div-based reports with specific patterns
            var errorNodes = doc.DocumentNode.SelectNodes("//div[contains(@id, 'error')] | //div[contains(@id, 'violation')] | //div[contains(@id, 'issue')]");
            if (errorNodes == null) return errors;

            foreach (var node in errorNodes)
            {
                var text = node.InnerText;
                
                // Use regex to extract information from text
                var fileMatch = Regex.Match(text, @"(?:File|Source):\s*(.+?)\s*(?:Line|$)", RegexOptions.IgnoreCase);
                var lineMatch = Regex.Match(text, @"(?:Line|Row):\s*(\d+)", RegexOptions.IgnoreCase);
                var ruleMatch = Regex.Match(text, @"(?:Rule|ID|Violation):\s*([^\s]+)", RegexOptions.IgnoreCase);
                var messageMatch = Regex.Match(text, @"(?:Message|Description):\s*(.+?)(?:\s*(?:File|Line|Rule|$))", RegexOptions.IgnoreCase);

                var fileName = fileMatch.Success ? fileMatch.Groups[1].Value.Trim() : "";
                var lineNumber = lineMatch.Success ? ParseInt(lineMatch.Groups[1].Value) : 0;
                var ruleId = ruleMatch.Success ? ruleMatch.Groups[1].Value.Trim() : "";
                var message = messageMatch.Success ? messageMatch.Groups[1].Value.Trim() : text.Trim();

                if (!string.IsNullOrEmpty(ruleId) || !string.IsNullOrEmpty(message))
                {
                    errors.Add(new ErrorInfo
                    {
                        RuleId = ruleId,
                        Message = message,
                        FileName = fileName,
                        LineNumber = lineNumber,
                        ColumnNumber = 0,
                        Severity = ErrorSeverity.Warning,
                        Category = "Parasoft C++TEST",
                        Description = message
                    });
                }
            }

            return errors;
        }

        private static int FindColumnIndex(List<string> headers, string[] possibleNames)
        {
            for (int i = 0; i < headers.Count; i++)
            {
                if (possibleNames.Any(name => headers[i].Contains(name)))
                {
                    return i;
                }
            }
            return -1;
        }

        private static string GetCellValue(HtmlNodeCollection cells, int index)
        {
            if (index < 0 || index >= cells.Count) return "";
            return cells[index].InnerText.Trim();
        }

        private static string ExtractFromNode(HtmlNode node, string[] patterns)
        {
            foreach (var pattern in patterns)
            {
                var childNode = node.SelectSingleNode($".//*[contains(@class, '{pattern}') or contains(@id, '{pattern}')]");
                if (childNode != null)
                {
                    return childNode.InnerText.Trim();
                }

                // Try regex on the text content
                var match = Regex.Match(node.InnerText, $@"{pattern}:\s*([^\s]+)", RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    return match.Groups[1].Value.Trim();
                }
            }
            return "";
        }

        private static int ParseInt(string value)
        {
            if (int.TryParse(value, out int result))
            {
                return result;
            }
            return 0;
        }

        private static ErrorSeverity MapSeverityFromText(string severity)
        {
            if (string.IsNullOrEmpty(severity)) return ErrorSeverity.Warning;

            switch (severity.ToLower())
            {
                case "error":
                case "high":
                case "critical":
                    return ErrorSeverity.Error;
                case "warning":
                case "medium":
                    return ErrorSeverity.Warning;
                case "info":
                case "information":
                case "low":
                case "note":
                    return ErrorSeverity.Information;
                default:
                    return ErrorSeverity.Warning;
            }
        }
    }
}
