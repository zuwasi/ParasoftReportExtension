using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using ParasoftReportExtension.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace ParasoftReportExtension.Services
{
    public class ErrorListService
    {
        private readonly AsyncPackage _package;
        private IVsErrorList _errorList;
        private ErrorListProvider _errorListProvider;

        public ErrorListService(AsyncPackage package)
        {
            _package = package;
            ThreadHelper.ThrowIfNotOnUIThread();
            
            _errorListProvider = new ErrorListProvider(package)
            {
                ProviderName = "Parasoft C++TEST",
                ProviderGuid = new Guid("F7B12345-6789-4BCD-EF01-234567890ABC")
            };
        }

        public async System.Threading.Tasks.Task InitializeAsync()
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            
            _errorList = await _package.GetServiceAsync(typeof(SVsErrorList)) as IVsErrorList;
        }

        public void LoadErrorsIntoErrorList(List<ErrorInfo> errors)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            // Clear existing errors from our provider
            _errorListProvider.Tasks.Clear();

            foreach (var error in errors)
            {
                var errorTask = new ErrorTask
                {
                    ErrorCategory = MapToTaskErrorCategory(error.Severity),
                    Category = TaskCategory.BuildCompile,
                    Text = error.Message,
                    Document = error.FileName,
                    Line = Math.Max(0, error.LineNumber - 1), // VS uses 0-based line numbers
                    Column = Math.Max(0, error.ColumnNumber - 1), // VS uses 0-based column numbers
                    Priority = TaskPriority.Normal,
                    SubcategoryIndex = 0,
                    HelpKeyword = error.RuleId
                };

                // Set additional properties
                if (!string.IsNullOrEmpty(error.RuleId))
                {
                    errorTask.HelpKeyword = error.RuleId;
                }

                // Handle file navigation
                if (!string.IsNullOrEmpty(error.FileName) && File.Exists(error.FileName))
                {
                    errorTask.Navigate += (sender, e) => NavigateToError(error);
                }

                _errorListProvider.Tasks.Add(errorTask);
            }

            // Show the error list if it's not already visible
            ShowErrorList();
        }

        private void NavigateToError(ErrorInfo error)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            try
            {
                if (string.IsNullOrEmpty(error.FileName) || !File.Exists(error.FileName))
                    return;

                // Open the file and navigate to the line
                ThreadHelper.JoinableTaskFactory.Run(async () =>
                {
                    await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                    var dte = await _package.GetServiceAsync(typeof(EnvDTE.DTE)) as EnvDTE.DTE;
                    if (dte != null)
                    {
                        var document = dte.Documents.Open(error.FileName);
                        if (document != null)
                        {
                            var textSelection = document.Selection as EnvDTE.TextSelection;
                            if (textSelection != null)
                            {
                                textSelection.GotoLine(Math.Max(1, error.LineNumber), true);
                                if (error.ColumnNumber > 0)
                                {
                                    textSelection.CharRight(false, error.ColumnNumber - 1);
                                }
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                // Log error or show message to user
                System.Diagnostics.Debug.WriteLine($"Error navigating to file: {ex.Message}");
            }
        }

        private TaskErrorCategory MapToTaskErrorCategory(ErrorSeverity severity)
        {
            switch (severity)
            {
                case ErrorSeverity.Error:
                    return TaskErrorCategory.Error;
                case ErrorSeverity.Warning:
                    return TaskErrorCategory.Warning;
                case ErrorSeverity.Information:
                    return TaskErrorCategory.Message;
                default:
                    return TaskErrorCategory.Warning;
            }
        }

        private void ShowErrorList()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            try
            {
                if (_errorList != null)
                {
                    _errorList.BringToFront();
                }
                else
                {
                    // Alternative method to show error list
                    ThreadHelper.JoinableTaskFactory.Run(async () =>
                    {
                        await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                        var dte = await _package.GetServiceAsync(typeof(EnvDTE.DTE)) as EnvDTE.DTE;
                        if (dte != null)
                        {
                            dte.ExecuteCommand("View.ErrorList");
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error showing error list: {ex.Message}");
            }
        }

        public void ClearErrors()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            _errorListProvider.Tasks.Clear();
        }

        public void Dispose()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            _errorListProvider?.Dispose();
        }
    }
}
