using Microsoft.Data.SqlClient;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

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
        #endregion

        #region Public Properties - Delegate Commands
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
            InitCommands();

            //_AppDetachedServices = appDetachedServices;
            //_AppDetachedServices.Init();

            _BaseDBContext = baseDBContext;
            AuthenticationService = authenticationService;
            ConfigService = configService;

        }
        #endregion

        #region Public Methods - Async
        
        public async Task TestConnectionAsync(object param)
        {
            TestConnection(param, true);
            await Task.Delay(0);
        }
        public async Task CreateDBAsync(object param)
        {
            await CreateDatabase();
            MessageBox.Show("Create Database", "Database Created!", MessageBoxButton.OK, MessageBoxImage.Information);
            //await Task.Delay(0);
        }
        public async Task CreateTablesAsync(object param)
        {
            await CreateTables();
            MessageBox.Show("Create Tables", "Tables Created!", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        #endregion

        #region Private Methods - Common
        private void InitCommands()
        {
            TestConnectionCommand = new DelegateCommand<object>(async (param) => await RunCommandAsync(() => IsNotBusy, async () => { await TestConnectionAsync(param); }));
            CreateDBCommand = new DelegateCommand<object>(async (param) => await RunCommandAsync(() => IsNotBusy, async () => { await CreateDBAsync(param); }));
            CreateTablesCommand = new DelegateCommand<object>(async (param) => await RunCommandAsync(() => IsNotBusy, async () => { await CreateTablesAsync(param); }));
        }

        private void GetDatabaseCredential(object param)
        {
            AuthenticationService.CreateSQLConnectionModel(new DatabaseCredentialModel
            {
                DatabaseName = DatabaseName,
                DatabasePassword = DBPassword, // ((MainWindow)param)._TextDBPassword.Text.ToString(),
                ServerHostName = ServerHostName,
                UserID = DBUsername,
            });
        }

        public bool TestConnection(object param, bool isTest)
        {

            GetDatabaseCredential(param);

            //AttachConfigValidators();
            //Result = DatabaseCredential.Validator.Validate(DatabaseCredential);
            //if (!Result.IsValid) { return false; }

            bool IsValidDBConnection =  AuthenticationService.IsValidDBConnection();

            if (IsValidDBConnection)
            {
                if (isTest) MessageBox.Show("Test Connection", "Test Connection Successful!", MessageBoxButton.OK, MessageBoxImage.Information);
                return true;
            }

            MessageBox.Show("Test Connection", "Unable to connect to server.", MessageBoxButton.OK, MessageBoxImage.Warning);
            return false;

        }

        private async Task CreateDatabase()
        {
            NewDatabaseModel databaseModel = new NewDatabaseModel
            {
                DatabaseName = NewDatabaseName,
                DatabaseUser = NewDBUsername, 
                DatabasePassword = NewDBPassword
            };

            if (ConfigService.CheckNewDBExist(databaseModel)) {
                MessageBox.Show("Create Database", "Database Already Exist", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            await ConfigService.CreateNewDBAsync(databaseModel);
        }

        private async Task CreateTables()
        {
            DatabaseCredentialModel databaseModel = new DatabaseCredentialModel
            {
                ServerHostName = ServerHostName,
                DatabaseName = DatabaseName,
                UserID = DBUsername,
                DatabasePassword = DBPassword
            };
            await ConfigService.CreateNewDBTablesAsync(databaseModel);
        }

        private void AttachConfigValidators()
        {
            if (DatabaseCredential.Validator == null) DatabaseCredential.AttachValidator(new DatabaseCredentialValidator());
        }
        #endregion

    }
}
