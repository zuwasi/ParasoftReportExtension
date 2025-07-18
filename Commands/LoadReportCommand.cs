using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using ParasoftReportExtension.Models;
using ParasoftReportExtension.Parsers;
using ParasoftReportExtension.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ParasoftReportExtension.Commands
{
    internal sealed class LoadReportCommand
    {
        public const int CommandId = 0x0100;
        public static readonly Guid CommandSet = new Guid("87654321-4321-4321-4321-210987654321");

        private readonly AsyncPackage _package;
        private readonly ErrorListService _errorListService;

        private LoadReportCommand(AsyncPackage package, OleMenuCommandService commandService, ErrorListService errorListService)
        {
            _package = package ?? throw new ArgumentNullException(nameof(package));
            _errorListService = errorListService ?? throw new ArgumentNullException(nameof(errorListService));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            var menuCommandID = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(Execute, menuCommandID);
            commandService.AddCommand(menuItem);
        }

        public static LoadReportCommand Instance { get; private set; }

        private Microsoft.VisualStudio.Shell.IAsyncServiceProvider ServiceProvider => _package;

        public static async Task InitializeAsync(AsyncPackage package, OleMenuCommandService commandService, ErrorListService errorListService)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);
            Instance = new LoadReportCommand(package, commandService, errorListService);
        }

        private void Execute(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            try
            {
                var openFileDialog = new OpenFileDialog
                {
                    Title = "Select Parasoft C++TEST Report File",
                    Filter = "All Supported Files (*.sarif;*.html;*.htm)|*.sarif;*.html;*.htm|SARIF Files (*.sarif)|*.sarif|HTML Files (*.html;*.htm)|*.html;*.htm|All Files (*.*)|*.*",
                    FilterIndex = 1,
                    Multiselect = false
                };

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var filePath = openFileDialog.FileName;
                    LoadReportFile(filePath);
                }
            }
            catch (Exception ex)
            {
                ShowError($"Error loading report file: {ex.Message}");
            }
        }

        private void LoadReportFile(string filePath)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            try
            {
                if (!File.Exists(filePath))
                {
                    ShowError($"File not found: {filePath}");
                    return;
                }

                var errors = new List<ErrorInfo>();
                var fileExtension = Path.GetExtension(filePath).ToLower();

                switch (fileExtension)
                {
                    case ".sarif":
                        errors = SarifParser.ParseSarifFile(filePath);
                        break;
                    case ".html":
                    case ".htm":
                        errors = HtmlParser.ParseHtmlFile(filePath);
                        break;
                    default:
                        // Try to detect file type by content
                        var content = File.ReadAllText(filePath);
                        if (content.TrimStart().StartsWith("{"))
                        {
                            // Likely JSON/SARIF
                            errors = SarifParser.ParseSarifFile(filePath);
                        }
                        else if (content.TrimStart().StartsWith("<"))
                        {
                            // Likely HTML
                            errors = HtmlParser.ParseHtmlFile(filePath);
                        }
                        else
                        {
                            ShowError($"Unsupported file format: {fileExtension}");
                            return;
                        }
                        break;
                }

                if (errors.Count == 0)
                {
                    ShowInfo($"No errors found in the report file: {Path.GetFileName(filePath)}");
                }
                else
                {
                    _errorListService.LoadErrorsIntoErrorList(errors);
                    ShowInfo($"Loaded {errors.Count} error(s) from {Path.GetFileName(filePath)} into Error List");
                }
            }
            catch (Exception ex)
            {
                ShowError($"Error processing file {Path.GetFileName(filePath)}: {ex.Message}");
            }
        }

        private void ShowError(string message)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            
            VsShellUtilities.ShowMessageBox(
                _package,
                message,
                "Parasoft Report Extension",
                OLEMSGICON.OLEMSGICON_CRITICAL,
                OLEMSGBUTTON.OLEMSGBUTTON_OK,
                OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
        }

        private void ShowInfo(string message)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            
            VsShellUtilities.ShowMessageBox(
                _package,
                message,
                "Parasoft Report Extension",
                OLEMSGICON.OLEMSGICON_INFO,
                OLEMSGBUTTON.OLEMSGBUTTON_OK,
                OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
        }
    }
}
