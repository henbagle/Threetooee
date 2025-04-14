using Terminal.Gui;
using ThreesTUI.Server;

namespace ThreesTUI.Views;
public class LoginDialog : Dialog
{ 
    public LoginDialog(Func<string, string, LoginResult> tryLoginFunc)
    {
        Title = $"Login to Threes";

        var emailLabel = new Label { Text = "Email" };

        var emailText = new TextField
        {
            X = Pos.Right(emailLabel) + 1,
            Width = Dim.Fill()
        };

        var passwordLabel = new Label
        {
            Text = "Password",
            X = Pos.Left(emailLabel),
            Y = Pos.Bottom(emailLabel) + 1
        };

        var passwordText = new TextField
        {
            Secret = true,
            X = Pos.Left(emailText),
            Y = Pos.Top(passwordLabel),
            Width = Dim.Fill()
        };

        var btnLogin = new Button
        {
            Text = "Login",
            X = Pos.Center(),
            Y = Pos.Bottom(passwordLabel) + 1,
            IsDefault = true
        };

        btnLogin.Accepting += (s, e) =>
        {
            var result = tryLoginFunc(emailText.Text, passwordText.Text);
            if (result.Success)
            {
                Application.RequestStop();
            }
            else
            {
                MessageBox.ErrorQuery("Login Failed", result.ErrorMessage, "OK");
            }

            e.Cancel = false;
        };
        
        Add(emailLabel, emailText, passwordLabel, passwordText, btnLogin);
    }
}