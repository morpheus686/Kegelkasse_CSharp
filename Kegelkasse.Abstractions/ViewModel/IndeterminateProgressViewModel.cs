namespace Kegelkasse.Base.ViewModel
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
				_message = value; 
				RaisePropertyChanged(nameof(Message));
			}
		}
    }
}
