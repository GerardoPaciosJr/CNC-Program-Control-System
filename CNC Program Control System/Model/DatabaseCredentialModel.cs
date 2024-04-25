using System.Security;

namespace CNC_Program_Control_System
{
    public class DatabaseCredentialModel : ValidatableBindableBase<DatabaseCredentialModel>
    {
        public string ServerHostName { get; set; }

        public string DatabaseName { get; set; }

        public string DatabaseUser { get; set; }

        public string DatabasePassword { get; set; }
        //public SecureString DatabasePassword { get; set; }

    }
}
