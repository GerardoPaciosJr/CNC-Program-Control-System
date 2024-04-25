using FluentValidation;
using Prism.Mvvm;
using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CNC_Program_Control_System
{
    public abstract class ValidatableBindableBase<T> :BindableBase, INotifyDataErrorInfo
    {
        public IValidator<T> Validator;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public bool HasErrors
        {
            get
            {
                if (Validator == null) return false;
                var context = new ValidationContext<object>(this);
                var hasErrors = Validator.Validate(context).Errors.Any();
                return hasErrors;
            }
        }

        public virtual void AttachValidator(IValidator<T> validator)
        {
            this.Validator = validator;
            RaisePropertyChanged(null);
        }

        public IEnumerable GetErrors([CallerMemberName] string propertyName = null)
        {
            if (Validator == null) return Enumerable.Empty<object>();
            var context = new ValidationContext<object>(this);
            var errors = Validator.Validate(context).Errors.Where(c => c.PropertyName == propertyName);

            return errors;
        }

        protected void OnErrorsChanged([CallerMemberName] string propertyName = null)
        {
            OnErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
        }

        protected void OnErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            var handler = ErrorsChanged;
            if (handler != null)
            {
                handler(sender, e);
            }
        }


    }
}
