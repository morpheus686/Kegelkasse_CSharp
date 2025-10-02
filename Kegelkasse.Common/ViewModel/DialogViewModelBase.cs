using System.Collections;
using System.ComponentModel;

namespace Kegelkasse.Common.ViewModel
{
    public abstract class DialogViewModelBase : LoadableViewModelBase, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, List<string>> _errors;

        public event EventHandler<object?>? RequestClose;

        protected DialogViewModelBase()
        {
            _errors = [];
        }

        public virtual bool HasErrors => _errors.Any();

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        protected void Close(object? result = null)
        {
            RequestClose?.Invoke(this, result);
        }

        public IEnumerable GetErrors(string? propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                return _errors.SelectMany(err => err.Value);
            }

            if (_errors.ContainsKey(propertyName))
            {
                return _errors[propertyName];
            }

            return Enumerable.Empty<string>();
        }

        protected void AddError(string propertyName, string errorMessage)
        {
            if (!_errors.ContainsKey(propertyName))
            {
                _errors[propertyName] = new List<string>();
            }

            if (!_errors[propertyName].Contains(errorMessage))
            {
                _errors[propertyName].Add(errorMessage);
                OnErrorsChanged(propertyName);
            }
        }

        protected void ClearErrors(string propertyName)
        {
            if (_errors.ContainsKey(propertyName))
            {
                _errors.Remove(propertyName);
                OnErrorsChanged(propertyName);
            }
        }

        private void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
    }
}
