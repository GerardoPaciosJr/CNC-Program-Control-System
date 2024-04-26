using Microsoft.Data.SqlClient;
using Prism.Commands;
using System;
using System.Collections.Generic;
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

        public string isConfigView
        {
            get { return _isConfigView; }
            set { _isConfigView = value; RaisePropertyChanged(); }
        }

        public string connectionStatus
        {
            get { return _connectionStatus; }
            set { _connectionStatus = value; RaisePropertyChanged(); }
        }
        #endregion

        #region Public Properties - Delegate Commands
        public DelegateCommand<object> PageLoadedCommand { get; set; }
        public DelegateCommand<object> TestConnectionCommand { get; set; }
        public DelegateCommand<object> CreateDBCommand { get; set; }
        public DelegateCommand<object> CreateTablesCommand { get; set; }
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

        //private DatabaseCredentialModel _databaseCredential = new DatabaseCredentialModel();
        #endregion

        #region Public Properties - Common
        public bool _isConnected = false; 
        private string _isConfigView = "Collapsed";
        private string _connectionStatus = "Connecting.";
        #endregion

        #region Private Properties - Interface
        //private readonly IAppDetachedService _AppDetachedServices;
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
                PageLoadedCommand.Execute(this);
                return;
            }));
        }
        #endregion

        #region Public Methods - Async

        private async Task LoadedAsync(object param)
        {
            TestConnection(param, false);
            await Task.Delay(0);
        }
        public async Task TestConnectionAsync(object param)
        {
            if(!TestConnection(param, true))
            {
                CreateDatabase();
                CreateTables();
            }
            //await Task.Delay(0);
        }
        public async Task CreateDBAsync(object param)
        {
            CreateDatabase();
            //MessageBox.Show("Create Database", "Database Created!", MessageBoxButton.OK, MessageBoxImage.Information);
            //await Task.Delay(0);
        }
        public async Task CreateTablesAsync(object param)
        {
            CreateTables();
            MessageBox.Show("Create Tables", "Tables Created!", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        #endregion

        #region Private Methods - Common
        private void InitCommands()
        {
            TestConnectionCommand = new DelegateCommand<object>(async (param) => await RunCommandAsync(() => IsNotBusy, async () => { await TestConnectionAsync(param); }));
            CreateDBCommand = new DelegateCommand<object>(async (param) => await RunCommandAsync(() => IsNotBusy, async () => { await CreateDBAsync(param); }));
            CreateTablesCommand = new DelegateCommand<object>(async (param) => await RunCommandAsync(() => IsNotBusy, async () => { await CreateTablesAsync(param); }));
            PageLoadedCommand = new DelegateCommand<object>(async (param) => await RunCommandAsync(() => IsNotBusy, async () => { await LoadedAsync(param); }));
        }

        private void GetDatabaseCredential()
        {
            AuthenticationService.CreateSQLConnectionOnModel(DatabaseCredential);
        }

        public bool TestConnection(object param, bool isTest)
        {
            connectionStatus = "Connecting.";
            GetDatabaseCredential();

            //AttachConfigValidators();
            //Result = DatabaseCredential.Validator.Validate(DatabaseCredential);
            //if (!Result.IsValid) { return false; }

            bool IsValidDBConnection =  AuthenticationService.IsValidDBConnection(isTest);
            DatabaseCredential = AuthenticationService.DatabaseConnectionModel;

            if (IsValidDBConnection)
            {
                if (isTest) MessageBox.Show("Test Connection Successful!", "Test Connection", MessageBoxButton.OK, MessageBoxImage.Information);
                isConfigView = "Collapsed";
                connectionStatus = "Connected";
                return true;
            }

            //if (isTest) MessageBox.Show("Unable to connect to server.", "Test Connection", MessageBoxButton.OK, MessageBoxImage.Warning);
            isConfigView = "Visible";
            connectionStatus = "Unable to Connect.";
            //SwitchCredentialAsync(param);
            return false;

        }

        private void CreateDatabase()
        {
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
            //await ConfigService.CreateNewDBTablesAsync(DatabaseCredential);
            //MessageBox.Show("Database and Tables has been Created!", "Create Database", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void CreateTables()
        {
            ConfigService.CreateNewDBTablesAsync(DatabaseCredential);
            MessageBox.Show("Database and Tables has been Created!", "Create Database", MessageBoxButton.OK, MessageBoxImage.Information);
            isConfigView = "Collapsed";
            connectionStatus = "Connected.";
        }

        private void AttachConfigValidators()
        {
            if (DatabaseCredential.Validator == null) DatabaseCredential.AttachValidator(new DatabaseCredentialValidator());
        }

        #endregion

    }
}
