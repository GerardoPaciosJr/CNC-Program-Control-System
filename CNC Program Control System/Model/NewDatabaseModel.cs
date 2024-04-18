using System.Security;

namespace CNC_Program_Control_System
{
    public class NewDatabaseModel : ValidatableBindableBase<NewDatabaseModel>
    {

        public string DatabaseName { get; set; }

        public string DatabaseUser { get; set; }

        public string DatabasePassword { get; set; }

    }
}
