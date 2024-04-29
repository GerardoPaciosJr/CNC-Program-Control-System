using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace CNC_Program_Control_System
{
    public class MainPageViewModel : BaseViewModel
    {

        #region Public Properties 

        public DatabaseCredentialModel DatabaseCredential
        {
            get { return _DatabaseCredential; }
            set { SetProperty(ref _DatabaseCredential, value); }
        }

        public string DatabaseName
        {
            get { return _DatabaseName; }
            set { SetProperty(ref _DatabaseName, value); }
        }

        public string ServerHostName
        {
            get { return _ServerHostName; }
            set { SetProperty(ref _ServerHostName, value); }
        }

        public string DBUsername
        {
            get { return _DBUsername; }
            set { SetProperty(ref _DBUsername, value); }
        }
        public string DBPassword
        {
            get { return _DBPassword; }
            set { SetProperty(ref _DBPassword, value); }
        }

        public string NewDatabaseName
        {
            get { return _NewDatabaseName; }
            set { SetProperty(ref _NewDatabaseName, value); }
        }
        public string NewDBUsername
        {
            get { return _NewDBUsername; }
            set { SetProperty(ref _NewDBUsername, value); }
        }
        public string NewDBPassword
        {
            get { return _NewDBPassword; }
            set { SetProperty(ref _NewDBPassword, value); }
        }

        public bool isConfigView
        {
            get { return _isConfigView; }
            set { _isConfigView = value; RaisePropertyChanged(); }
        }

        public string connectionStatus
        {
            get { return _connectionStatus; }
            set { _connectionStatus = value; RaisePropertyChanged(); }
        }

        public bool ServersLoading
        {
            get { return _serversLoading; }
            set { _serversLoading = value; RaisePropertyChanged(); }
        }
        #endregion

        #region Public Properties - Delegate Commands
        public DelegateCommand PageLoadedCommand { get; set; }
        public DelegateCommand<object> TestConnectionCommand { get; set; }
        #endregion

        #region Private Properties - Common

        private string _DatabaseName;
        private string _ServerHostName;
        private string _DBUsername;
        private string _DBPassword;


        private string _NewDatabaseName;
        private string _NewDBUsername;
        private string _NewDBPassword;

        private DatabaseCredentialModel _DatabaseCredential;
        private bool _serversLoading;
        private bool _isConfigView;

        //private DatabaseCredentialModel _databaseCredential = new DatabaseCredentialModel();
        #endregion

        #region Public Properties - Common
        public bool _isConnected = false; 
        private string _connectionStatus = "Connecting";
        #endregion

        #region Private Properties - Interface
        private readonly IBaseDBContext _BaseDBContext;
        private FluentValidation.Results.ValidationResult Result = new FluentValidation.Results.ValidationResult();
        #endregion
        public IAuthenticationService AuthenticationService { get; set; }
        public IConfigService ConfigService { get; set; }

        #region Constructor
        public MainPageViewModel(IBaseDBContext baseDBContext, IAuthenticationService authenticationService, IConfigService configService)
        {
            _BaseDBContext = baseDBContext;
            AuthenticationService = authenticationService;
            ConfigService = configService;

            DatabaseCredential = new DatabaseCredentialModel();
            InitCommands();

            Application.Current.Dispatcher.BeginInvoke(
            DispatcherPriority.ApplicationIdle,
            new Action(() =>
            {
                PageLoadedCommand.Execute();
                return;
            }));
        }
        #endregion

        #region Public Methods - Async

        private async Task LoadedAsync()
        {
            isConfigView = false;
            ServersLoading = true;
            var serverLoader = new BackgroundWorker();
            serverLoader.DoWork += ((sender, e) => e.Result = TestConnection());
            serverLoader.RunWorkerCompleted += ((sender, e) =>
            {
                //if(e.Result(0).ToString() == "false")
                //{
                //    isConfigView = true;
                //}
                //ServersLoading = false;
            });
            serverLoader.RunWorkerAsync();
        }
        public async Task TestConnectionAsync(object param)
        {
            if(TestConnection(param, true))
            {
                isConfigView = false;
            }
            //await Task.Delay(0);
        }
        #endregion

        #region Private Methods - Common
        private void InitCommands()
        {
            TestConnectionCommand = new DelegateCommand<object>(async (param) => await RunCommandAsync(() => IsNotBusy, async () => { await TestConnectionAsync(param); }));
            PageLoadedCommand = new DelegateCommand(async () => await RunCommandAsync(() => IsNotBusy, async () => { await LoadedAsync(); }));
        }

        private void GetDatabaseCredential()
        {
            AuthenticationService.CreateSQLConnectionOnModel(DatabaseCredential);
        }

        public bool TestConnection(object param, bool isTest)
        {
            connectionStatus = "Connecting";
            GetDatabaseCredential();

            //AttachConfigValidators();
            //Result = DatabaseCredential.Validator.Validate(DatabaseCredential);
            //if (!Result.IsValid) { return false; }

            bool IsValidDBConnection =  AuthenticationService.IsValidDBConnection(isTest);
            DatabaseCredential = AuthenticationService.DatabaseConnectionModel;

            if (IsValidDBConnection)
            {
                if (isTest) MessageBox.Show("Test Connection Successful!", "Test Connection", MessageBoxButton.OK, MessageBoxImage.Information);
                connectionStatus = "Connected";
                return true;
            }

            var Result = MessageBox.Show("Create the database and save the following configuration?", "Test Connection", MessageBoxButton.YesNo, MessageBoxImage.Question);
            connectionStatus = "Unable to Connect";
            if (Result == MessageBoxResult.Yes)
            {
                CreateDatabase();
                CreateTables();
                return true;
            }
            return false;

        }

        public bool TestConnection()
        {
            connectionStatus = "Connecting"; //To be move
            GetDatabaseCredential();

            //AttachConfigValidators();
            //Result = DatabaseCredential.Validator.Validate(DatabaseCredential);
            //if (!Result.IsValid) { return false; }

            bool IsValidDBConnection = AuthenticationService.IsValidDBConnection(false);
            DatabaseCredential = AuthenticationService.DatabaseConnectionModel;

            if (IsValidDBConnection)
            {
                connectionStatus = "Connected";
                ServersLoading = false;
                isConfigView = false;
                return true;
            }

            isConfigView = true;
            ServersLoading = false;
            connectionStatus = "Unable to Connect";
            //SwitchCredentialAsync(param);
            return false;

        }

        private void CreateDatabase()
        {
            connectionStatus = "Creating Database";
            NewDatabaseModel databaseModel = new NewDatabaseModel
            {
                DatabaseName = DatabaseCredential.DatabaseName,
                DatabaseUser = DatabaseCredential.DatabaseUser, 
                DatabasePassword = DatabaseCredential.DatabasePassword
            };

            if (ConfigService.CheckNewDBExist(databaseModel)) {
                MessageBox.Show("Database Already Exist", "Create Database", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            ConfigService.CreateNewDBAsync(databaseModel);
        }

        private void CreateTables()
        {
            connectionStatus = "Creating Tables";
            ConfigService.CreateNewDBTablesAsync(DatabaseCredential);
            MessageBox.Show("Database and Tables has been Created!", "Create Database", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void AttachConfigValidators()
        {
            if (DatabaseCredential.Validator == null) DatabaseCredential.AttachValidator(new DatabaseCredentialValidator());
        }

        #endregion

    }
}
