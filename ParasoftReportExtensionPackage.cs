using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using ParasoftReportExtension.Commands;
using ParasoftReportExtension.Services;
using System;
using System.ComponentModel.Design;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace ParasoftReportExtension
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(ParasoftReportExtensionPackage.PackageGuidString)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    public sealed class ParasoftReportExtensionPackage : AsyncPackage
    {
        public const string PackageGuidString = "12345678-1234-1234-1234-123456789012";
        public const string CommandSetGuidString = "87654321-4321-4321-4321-210987654321";
        public const int LoadReportCommandId = 0x0100;

        private ErrorListService _errorListService;

        public static readonly Guid CommandSet = new Guid(CommandSetGuidString);

        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

            // Initialize services
            _errorListService = new ErrorListService(this);

            // Initialize commands
            if (await GetServiceAsync(typeof(IMenuCommandService)) is OleMenuCommandService commandService)
            {
                await LoadReportCommand.InitializeAsync(this, commandService, _errorListService);
            }
        }

        public async Task<T> GetServiceAsync<T>()
        {
            return (T)await GetServiceAsync(typeof(T));
        }
    }
}
