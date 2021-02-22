using PWBolt_GUI.Network;
using System;
using System.Windows.Forms;

namespace PWBolt_GUI
{

    public partial class LoginForm : Form
    {
        public static string user = "";
        public string pass = "";
        public LoginForm()
        {
            InitializeComponent();
        }

        //Login Event
        private async void btn_LogIn_Click(object sender, EventArgs e)
        {
            user = txtBox_user.Text;
            pass = txtBox_pass.Text;
            if (!user.Equals("") && !pass.Equals(""))
            {
                if (user.Length > 4 && pass.Length > 5)
                {
                    await WebServer.LoginPWBolt(user, pass);
                    if (WebServer.IsLoggedIn)
                    {
                        Hide();
                        using (MainForm form2 = new MainForm())
                            form2.ShowDialog();
                        Show();
                    }
                }
                else
                    MessageBox.Show("Username or Password too short.");
            }
            else
                MessageBox.Show("Empty Fields");
        }
        //Register Event
        private async void btn_register_Click(object sender, EventArgs e)
        {
            user = txtBox_user.Text;
            pass = txtBox_pass.Text;
            if (!user.Equals("") && !pass.Equals(""))
            {
                if (user.Length > 4 && pass.Length > 5)
                {
                    await WebServer.RegisterPWBoltAsync(user, pass);
                    txtBox_pass.Clear();
                }
                else
                    MessageBox.Show("Username or Password too short.");
            }
            else
                MessageBox.Show("Empty Fields");
        }

        //Exit Event
        private void btn_exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
