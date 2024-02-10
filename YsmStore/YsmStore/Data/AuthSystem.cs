using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
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

        private static HttpClient _client = new HttpClient();

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

        public static async Task Login(AuthData data)
        {
            string errorMessage = CheckAuthData(data);
            if (errorMessage != null)
                throw new YsmStoreException(errorMessage);

            HttpResponseMessage response = null;

            try
            {
                response = await _client.GetAsync($"{ApiOptions.RootUrl}/auth/{data.Login}/{data.Password}");
                response.EnsureSuccessStatusCode();
                string token = await response.Content.ReadAsStringAsync();

                LoginedUser = await UserAdapter.Get(token);
                LoginedUser.Login = data.Login;
                LoginedUser.Password = data.Password;

                SavedData = data.RememberMe ? data : null;
                ((App)App.Current).SetStartPageForLoginedUser();
            }
            catch (HttpRequestException)
            {
                if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    throw new YsmStoreException("Неверный логин или пароль");
                }
                else
                {
                    throw new YsmStoreException("Не удалось подключиться к серверу");
                }
            }
            finally
            {
                response?.Dispose();
            }
        }

        public static async Task Login(RegData data)
        {
            string errorMessage = CheckRegData(data);
            if (errorMessage != null)
                throw new YsmStoreException(errorMessage);

            HttpResponseMessage response = null;

            try
            {
                response = await _client.PostAsync($"{ApiOptions.RootUrl}/auth", data.ToStringContent());
                response.EnsureSuccessStatusCode();
                string token = await response.Content.ReadAsStringAsync();

                LoginedUser = await UserAdapter.Get(token);
                LoginedUser.Login = data.Login;
                LoginedUser.Password = data.Password;

                SavedData = new AuthData() { Login = data.Login, Password = data.Password, RememberMe = true };
                ((App)App.Current).SetStartPageForLoginedUser();
            }
            catch (HttpRequestException)
            {
                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    throw new YsmStoreException("Пользователь с таким логином уже существует");
                }
                else
                {
                    throw new YsmStoreException("Не удалось подключиться к серверу");
                }
            }
            finally
            {
                response?.Dispose();
            }
        }

        public static async Task Login(RecoveryData data)
        {
            string errorMessage = CheckRecoveryData(data);
            if (errorMessage != null)
                throw new YsmStoreException(errorMessage);

            HttpResponseMessage response = null;

            try
            {
                response = await _client.PutAsync($"{ApiOptions.RootUrl}/auth", data.ToStringContent());
                response.EnsureSuccessStatusCode();
                string token = await response.Content.ReadAsStringAsync();

                LoginedUser = await UserAdapter.Get(token);
                LoginedUser.Login = data.Login;
                LoginedUser.Password = data.NewPassword;

                SavedData = new AuthData() { Login = data.Login, Password = data.NewPassword, RememberMe = true };
                ((App)App.Current).SetStartPageForLoginedUser();
            }
            catch (HttpRequestException)
            {
                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    throw new YsmStoreException("Неверный логин или пароль воссатновления");
                }
                else
                {
                   throw new YsmStoreException("Не удалось подключиться к серверу");
                }
            }
            finally
            {
                response?.Dispose();
            }
        }

        public static void Logout()
        {
            LoginedUser = null;
            ((App)App.Current).SetStartPageForLoginedUser();
        }

        public static async Task SendRecoveryRequest(string email)
        {
            HttpResponseMessage response = null;

            try
            {
                response = await _client.PutAsync($"{ApiOptions.RootUrl}/auth/{email}", null);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException)
            {
                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    throw new YsmStoreException("Пользователя с таким логином не существует");
                }
                else
                {
                    throw new YsmStoreException("Не удалось подключиться к серверу");
                }
            }
            finally
            {
                response?.Dispose();
            }
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
