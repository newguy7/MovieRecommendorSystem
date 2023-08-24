namespace MovieStoreApp.ModelsVM
{
    public class UserService
    {
        private UserInfo _currentUser;
        public UserInfo CurrentUser
        {
            get { return _currentUser; }
            set
            {
                if (_currentUser != value)
                {
                    _currentUser = value;
                    NotifyStateChanged();
                }
            }
        }

        public event Action OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();

        public void Logout()
        {
            CurrentUser = null;
            NotifyStateChanged();
        }
    }
}
