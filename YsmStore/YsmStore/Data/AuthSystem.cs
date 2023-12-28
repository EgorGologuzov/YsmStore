using System.Collections.Generic;
using YsmStore.Models;

namespace YsmStore.Data
{
    public static class AuthSystem
    {
        private const string LOGIN_KEY = "user_login";
        private const string PASSWORD_KEY = "user_password";
        private const string REMEMBER_ME_KEY = "user_remember_me";
        private const string REG_DATA_IS_NULL_MESSAGE = "RegData cannot be null for check";
        private const string LOGIN_UNFILLED_MESSAGE = "Логин не заполнен";
        private const string FULLNAME_UNFILLED_MESSAGE = "Полное имя не заполнено";
        private const string USERNAME_UNFILLED_MESSAGE = "Имя пользователя не заполнено";
        private const string PASSWORD_UNFILLED_MESSAGE = "Пароль не заполнен";
        private const string PASSWORDS_NOT_EQUAL_MESSAGE = "Пароли не совпадают";
        private const string AUTHDATA_IS_NULL_MESSAGE = "AuthData is null";
        private const string RECOVERY_PASSWORD_UNFILLED_MESSAGE = "Пароль восстановления не заполнен";

        public static User LoginedUser { get; private set; }

        public static AuthData SavedData
        {
            get
            {
                try
                {
                    AuthData data = new AuthData();
                    data.Login = (string)App.Current.Properties[LOGIN_KEY];
                    data.Password = (string)App.Current.Properties[PASSWORD_KEY];
                    data.RememberMe = (bool)App.Current.Properties[REMEMBER_ME_KEY];
                    return data;
                }
                catch (KeyNotFoundException)
                {
                    return null;
                }
            }
            set
            {
                if (value == null)
                {
                    if (App.Current.Properties.ContainsKey(LOGIN_KEY))
                        App.Current.Properties.Remove(LOGIN_KEY);
                    if (App.Current.Properties.ContainsKey(PASSWORD_KEY))
                        App.Current.Properties.Remove(PASSWORD_KEY);
                    if (App.Current.Properties.ContainsKey(REMEMBER_ME_KEY))
                        App.Current.Properties.Remove(REMEMBER_ME_KEY);
                    App.Current.SavePropertiesAsync();
                    return;
                }

                App.Current.Properties[LOGIN_KEY] = value.Login;
                App.Current.Properties[PASSWORD_KEY] = value.Password;
                App.Current.Properties[REMEMBER_ME_KEY] = value.RememberMe;
                App.Current.SavePropertiesAsync();
            }
        }

        public static void Login(AuthData data)
        {
            string errorMessage = CheckAuthData(data);
            if (errorMessage != null)
                throw new YsmStoreException(errorMessage);

            //execute query, get token
            string token = data.Login == TestingData.ADMIN_TEST_LOGIN ? TestingData.ADMIN_TEST_TOKEN : TestingData.CUSTOMER_TEST_TOKEN;
            LoginedUser = UserAdapter.Get(token);
            SavedData = data.RememberMe ? data : null;
            ((App)App.Current).SetStartPageForLoginedUser();
        }

        public static void Login(RegData data)
        {
            string errorMessage = CheckRegData(data);
            if (errorMessage != null)
                throw new YsmStoreException(errorMessage);

            //execute query, get token
            string token = TestingData.CUSTOMER_TEST_TOKEN;
            LoginedUser = UserAdapter.Get(token);
            AuthData authData = new AuthData() { Login = data.Login, Password = data.Password, RememberMe = true };
            SavedData = authData;
            ((App)App.Current).SetStartPageForLoginedUser();
        }

        public static void Login(RecoveryData data)
        {
            string errorMessage = CheckRecoveryData(data);
            if (errorMessage != null)
                throw new YsmStoreException(errorMessage);

            //execute query, get token
            string token = TestingData.CUSTOMER_TEST_TOKEN;
            LoginedUser = UserAdapter.Get(token);
            AuthData authData = new AuthData() { Login = data.Login, Password = data.NewPassword, RememberMe = true };
            SavedData = authData;
            ((App)App.Current).SetStartPageForLoginedUser();
        }

        public static void Logout()
        {
            LoginedUser = null;
            ((App)App.Current).SetStartPageForLoginedUser();
        }

        public static void SendRecoveryRequest(string email)
        {

        }

        private static string CheckAuthData(AuthData data)
        {
            if (data == null)
                throw new YsmStoreException(AUTHDATA_IS_NULL_MESSAGE);
            if (data.Login == null || data.Login == string.Empty)
                return LOGIN_UNFILLED_MESSAGE;
            if (data.Password == null || data.Password == string.Empty)
                return PASSWORD_UNFILLED_MESSAGE;
            return null;
        }

        private static string CheckRegData(RegData data)
        {
            if (data == null)
                throw new YsmStoreException(REG_DATA_IS_NULL_MESSAGE);
            if (data.Login == null || data.Login == string.Empty)
                return LOGIN_UNFILLED_MESSAGE;
            if (data.FullName == null || data.FullName == string.Empty)
                return FULLNAME_UNFILLED_MESSAGE;
            if (data.UserName == null || data.UserName == string.Empty)
                return USERNAME_UNFILLED_MESSAGE;
            if (data.Password == null || data.Password == string.Empty)
                return PASSWORD_UNFILLED_MESSAGE;
            if (data.Password != data.PasswordRepeat)
                return PASSWORDS_NOT_EQUAL_MESSAGE;
            return null;
        }

        private static string CheckRecoveryData(RecoveryData data)
        {
            if (data == null)
                throw new YsmStoreException(REG_DATA_IS_NULL_MESSAGE);
            if (string.IsNullOrEmpty(data.Login))
                return LOGIN_UNFILLED_MESSAGE;
            if (string.IsNullOrEmpty(data.RecoveryPassword))
                return RECOVERY_PASSWORD_UNFILLED_MESSAGE;
            if (string.IsNullOrEmpty(data.NewPassword))
                return PASSWORD_UNFILLED_MESSAGE;
            if (data.NewPassword != data.NewPasswordRepeat)
                return PASSWORDS_NOT_EQUAL_MESSAGE;
            return null;
        }
    }
}
