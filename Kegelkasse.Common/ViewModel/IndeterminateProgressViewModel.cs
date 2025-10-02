namespace Kegelkasse.Common.ViewModel
{
    public class IndeterminateProgressViewModel : ViewModelBase
    {
        public IndeterminateProgressViewModel()
        {
            _message = string.Empty;
        }

        private string _message;

        public string Message
		{
			get { return _message; }
			set
			{
                SetProperty(ref _message, value);
			}
		}
    }
}
