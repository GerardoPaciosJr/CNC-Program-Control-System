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
        #endregion
        public IAuthenticationService AuthenticationService { get; set; }

        #region Constructor
        public MainPageViewModel(IBaseDBContext baseDBContext, IAuthenticationService authenticationService)
        {
            InitCommands();

            //_AppDetachedServices = appDetachedServices;
            //_AppDetachedServices.Init();

            _BaseDBContext = baseDBContext;
            AuthenticationService = authenticationService;

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
            CreateDatabase(param);

            await Task.Delay(0);
        }
        public async Task CreateTablesAsync(object param)
        {
            CreateTables(param);

            await Task.Delay(0);
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
            bool IsValidDBConnection =  AuthenticationService.IsValidDBConnection();

            if (IsValidDBConnection)
            {
                if (isTest) MessageBox.Show("Test Connection", "Test Connection Successful!", MessageBoxButton.OK, MessageBoxImage.Information);
                return true;
            }

            MessageBox.Show("Test Connection", "Unable to connect to server. Please contact your network administrator", MessageBoxButton.OK, MessageBoxImage.Warning);
            return false;

        }

        private void CreateDatabase(object param)
        {
            try
            {

                //string connectionString = _BaseDBContext.ConnectionString;
                using (SqlConnection connection = new SqlConnection(AuthenticationService.GetValidDBConnection()))
                {
                    connection.Open();
                    string createDatabaseQuery = $"CREATE DATABASE {NewDatabaseName}";
                    SqlCommand command = new SqlCommand(createDatabaseQuery, connection);
                    command.CommandTimeout = 0;

                    command.ExecuteNonQuery();

                // Create User with Admin Permissions
                string createUserQuery = $"CREATE LOGIN {NewDBUsername} WITH PASSWORD = '{NewDBPassword}';" +
                                            $"USE {NewDatabaseName}; " +
                                            $"CREATE USER {NewDBUsername} FOR LOGIN {NewDBUsername}; " +
                                            $"ALTER ROLE db_owner ADD MEMBER {NewDBUsername};";

                SqlCommand createUserCommand = new SqlCommand(createUserQuery, connection);
                    createUserCommand.CommandTimeout = 60;
                    createUserCommand.ExecuteNonQuery();

                MessageBox.Show("Database Creation", "Database Creation Complete!", MessageBoxButton.OK, MessageBoxImage.Information);
                    UpdateDBConnectionSetting();
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CreateTables(object param)
        {
            try
            {

                string sqlFilePath = Directory.GetCurrentDirectory() + "\\tables.sql";
                if (!File.Exists(sqlFilePath))
                {
                    MessageBox.Show("SQL file not found.");
                    return;
                }

                GetDatabaseCredential(param);
                
                string sqlScript = File.ReadAllText(sqlFilePath);
                //string connectionString = _BaseDBContext.ConnectionString;
                using (SqlConnection connection = new SqlConnection(AuthenticationService.IsValidDBConnectionString()))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlScript, connection);
                    command.CommandTimeout = 60;
                    command.ExecuteNonQuery();

                    MessageBox.Show("Table Creation", "Table Creation Complete!", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                

                    

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UpdateDBConnectionSetting()
        {
            DatabaseName = NewDBPassword;
            DBUsername = NewDBUsername;
            DBPassword = NewDBPassword;
        }

            #endregion

        }
}
