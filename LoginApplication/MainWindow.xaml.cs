using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;


namespace LoginApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // initiliza a user for tesing
            string initUsername = "admin";
            string initPassword = "admin";
            byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);
            string hashPassword = GenerateHashPassword(initPassword, salt);
            User initUser = new User(initUsername, hashPassword, salt);
            UserDatabase.users.Add(initUser);

        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            // check if the text fields are blank
            if (!CheckTextFieldBlank())
            {
                return;
            }

            string usernameInput = usernameText.Text;
            string passwordInput = passwordText.Password;

            // check if the username has registered before
            int IsRegisteredResult = IsResgistered(usernameInput);
            if (IsRegisteredResult != -1)
            {
                byte[] salt = UserDatabase.users[IsRegisteredResult].Salt;
                string hashPassword = GenerateHashPassword(passwordInput, salt);
                if (hashPassword == UserDatabase.users[IsRegisteredResult].HashPassword)
                {
                    MessageBox.Show("Login successfully");
                    ClearTextField();
                }
                else
                {
                    MessageBox.Show("Failed to login");
                    ClearTextField();
                }
            }
            else
            {
                MessageBox.Show("The username doesn't exist");
                ClearTextField();
            }
        }

        private void registerButton_Click(object sender, RoutedEventArgs e)
        {
            if(!CheckTextFieldBlank())
            {
                return;
            }

            string usernameInput = usernameText.Text;
            string passwordInput = passwordText.Password;

            int IsResgisteredResult = IsResgistered(usernameInput);
            if (IsResgisteredResult == -1)
            {
                byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);
                string hashPassword = GenerateHashPassword(passwordInput, salt);
                User newUser = new User(passwordInput, hashPassword, salt); 
                UserDatabase.users.Add(newUser);
                MessageBox.Show($"{newUser.Username} is registered successfully");
                ClearTextField();
            }
            else
            {
                MessageBox.Show("The username has been registered");
                ClearTextField();
            }
        }

        public int IsResgistered(string username)
        {
            for (int i = 0; i < UserDatabase.users.Count(); i++)
            {
                if (username == UserDatabase.users[i].Username) return i;
            }
            return -1;
        }

        // Method - clear the text fields
        public void ClearTextField()
        {
            usernameText.Text = String.Empty;
            passwordText.Password = String.Empty;
        }

        // Method - check the text fields are blank or not
        public bool CheckTextFieldBlank()
        {
            if (String.IsNullOrEmpty(usernameText.Text))
            {
                MessageBox.Show("The username cannot be blank");
                return false;
            }
            else if (String.IsNullOrEmpty(passwordText.Password))
            {
                MessageBox.Show("The password cannot be blank");
                return false;
            }
            return true;
        }

        // Method - Generate hash password
        public string GenerateHashPassword(string password, byte[] salt)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password,
                salt,
                KeyDerivationPrf.HMACSHA256,
                1000,
                256 / 8));
        }
    }
}
