using System;

namespace ParasoftReportExtension.Models
{
    public class ErrorInfo
    {
        public string RuleId { get; set; }
        public string Message { get; set; }
        public string FileName { get; set; }
        public int LineNumber { get; set; }
        public int ColumnNumber { get; set; }
        public ErrorSeverity Severity { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
    }

    public enum ErrorSeverity
    {
        Error,
        Warning,
        Information
    }
}
