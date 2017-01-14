using Windows.UI.Xaml;
using System.Threading.Tasks;
using OwncloudUniversal.Services.SettingsServices;
using Windows.ApplicationModel.Activation;
using Template10.Controls;
using Template10.Common;
using System;
using System.Linq;
using Windows.ApplicationModel.Resources;
using Windows.UI.Popups;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Controls;
using OwncloudUniversal.Shared;
using OwncloudUniversal.Shared.SQLite;
using OwncloudUniversal.Views;
using OwncloudUniversal.WebDav;

namespace OwncloudUniversal
{
    /// Documentation on APIs used in this page:
    /// https://github.com/Windows-XAML/Template10/wiki

    [Bindable]
    sealed partial class App : BootStrapper
    {
        public App()
        {
            InitializeComponent();
            SplashFactory = (e) => new Views.Splash(e);

            #region app settings

            // some settings must be set in app.constructor
            var settings = SettingsService.Instance;
            RequestedTheme = settings.AppTheme;
            CacheMaxDuration = settings.CacheMaxDuration;
            ShowShellBackButton = settings.UseShellBackButton;
            AutoSuspendAllFrames = true;
            AutoRestoreAfterTerminated = true;
            AutoExtendExecutionSession = true;

            #endregion
        }

        public static ResourceLoader ResourceLoader = new ResourceLoader();

        public override UIElement CreateRootElement(IActivatedEventArgs e)
        {
            var service = NavigationServiceFactory(BackButton.Attach, ExistingContent.Exclude);
            return new ModalDialog
            {
                DisableBackButtonWhenModal = true,
                Content = new Views.Shell(service),
                //ModalContent = new Views.Busy(),
            };
        }

        public override async Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            // TODO: add your long-running task here
            SQLiteClient.Init();
            var status = await OcsClient.GetServerStatusAsync(Configuration.ServerUrl);
            if (status == null)
                await NavigationService.NavigateAsync(typeof(WelcomePage));
            else
            {
                await NavigationService.NavigateAsync(typeof(Views.FilesPage));
            }
        }

        public override void OnResuming(object s, object e, AppExecutionState previousExecutionState)
        {
            base.OnResuming(s, e, previousExecutionState);
        }
    }
}

