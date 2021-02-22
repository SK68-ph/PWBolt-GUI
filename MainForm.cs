using Newtonsoft.Json;
using PWBolt_GUI.Network;
using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace PWBolt_GUI
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Check if string only contains allowed characters
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public bool isStrValid(string str)
        {
            return Regex.IsMatch(str, @"^[a-zA-Z0-9\@\.]+$");
        }

        /// <summary>
        /// lastIdValue will serve as auto increment for row id
        /// </summary>
        int lastIdValue = 0;

        
        /// <summary>
        /// If user selects a row, set Edit Textbox field value to current select row value in datagridview.
        /// </summary>
        private void DataGrid_Accounts_SelectionChanged(object sender, EventArgs e)
        {
            if (DataGrid_Accounts.SelectedRows.Count != 0 && btn_EditAccount.Enabled && DataGrid_Accounts.Rows.Count > 1)
            {
                int curIndex = DataGrid_Accounts.CurrentRow.Index;
                txtBox_website.Text = DataGrid_Accounts.Rows[curIndex].Cells[1].Value.ToString();
                txtBox_username.Text = DataGrid_Accounts.Rows[curIndex].Cells[2].Value.ToString();
                txtBox_pass.Text = DataGrid_Accounts.Rows[curIndex].Cells[3].Value.ToString();
            }
        }

        /// <summary>
        /// Update lastIdValue when the user delete a row.
        /// </summary>
        private void DataGrid_Accounts_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if (DataGrid_Accounts.Rows.Count >= 1) // get current last row Id value
            {
                int lastIndex = DataGrid_Accounts.Rows.Count - 1;
                lastIdValue = int.Parse((string)DataGrid_Accounts.Rows[lastIndex].Cells[0].Value);
            }
            else // if empty, start again to index 1
            {
                lastIdValue = 0;
            }
        }

        /// <summary>
        /// Populate DataGridView for first login
        /// </summary>
        private async void refreshDataList()
        {
            DataGrid_Accounts.Rows.Clear();
            var rawJson = await WebServer.bolt_DisplayAccountAsync();
            if (!(rawJson.Contains("Status:ERROR") || rawJson.Contains("Status:WARNING")))
            {
                dynamic accountJson = JsonConvert.DeserializeObject(rawJson);
                foreach (var s in accountJson)
                {
                    string[] account = { s.id, s.website, s.username, s.password };
                    lastIdValue = s.id;
                    DataGrid_Accounts.Rows.Add(account);
                }
            }
        }

        /// <summary>
        /// Show AddAccount textboxfields and hide other options.
        /// </summary>
        private void btn_AddAccount_Click(object sender, EventArgs e)
        {
            txtBox_pass.Clear();
            txtBox_username.Clear();
            txtBox_website.Clear();
            lbl_web.Visible = !lbl_web.Visible;
            lbl_user.Visible = !lbl_user.Visible;
            lbl_pass.Visible = !lbl_pass.Visible;
            txtBox_pass.Visible = !txtBox_pass.Visible;
            txtBox_username.Visible = !txtBox_username.Visible;
            txtBox_website.Visible = !txtBox_website.Visible;
            btn_Submit.Visible = !btn_Submit.Visible;
            btn_random.Visible = !btn_random.Visible;
            btn_EditAccount.Enabled = !btn_EditAccount.Enabled;
            btn_DeleteAccount.Enabled = !btn_DeleteAccount.Enabled;
        }

        /// <summary>
        /// Show Edit Account textboxfields, hide other options and send value of selected row to textboxfields.
        /// </summary>
        private void btn_EditAccount_Click(object sender, EventArgs e)
        {
            if (DataGrid_Accounts.SelectedRows.Count != 0)
            {
                txtBox_pass.Clear();
                txtBox_username.Clear();
                txtBox_website.Clear();
                lbl_web.Visible = !lbl_web.Visible;
                lbl_user.Visible = !lbl_user.Visible;
                lbl_pass.Visible = !lbl_pass.Visible;
                txtBox_pass.Visible = !txtBox_pass.Visible;
                txtBox_username.Visible = !txtBox_username.Visible;
                txtBox_website.Visible = !txtBox_website.Visible;
                btn_Done.Visible = !btn_Done.Visible;
                btn_random.Visible = !btn_random.Visible;
                btn_AddAccount.Enabled = !btn_AddAccount.Enabled;
                btn_DeleteAccount.Enabled = !btn_DeleteAccount.Enabled;

                int curIndex = DataGrid_Accounts.CurrentRow.Index;
                txtBox_website.Text = DataGrid_Accounts.Rows[curIndex].Cells[1].Value.ToString();
                txtBox_username.Text = DataGrid_Accounts.Rows[curIndex].Cells[2].Value.ToString();
                txtBox_pass.Text = DataGrid_Accounts.Rows[curIndex].Cells[3].Value.ToString();
            }
        }

        /// <summary>
        /// Delete account from usertable and datagridview(async)
        /// </summary>
        private async void btn_DeleteAccount_Click(object sender, EventArgs e)
        {
            if (DataGrid_Accounts.SelectedRows.Count != 0)
            {
                int curIndex = DataGrid_Accounts.CurrentRow.Index;
                string id = DataGrid_Accounts.Rows[curIndex].Cells[0].Value.ToString();
                await WebServer.bolt_DeleteAccountAsync(id);
                refreshDataList();
            }
        }

        /// <summary>
        /// Update user's table in database(async) and Update client datagridview
        /// </summary>
        private async void btn_Done_Click(object sender, EventArgs e)
        {
            string web = txtBox_website.Text;
            string user = txtBox_username.Text;
            string pass = txtBox_pass.Text;
            if (!string.IsNullOrWhiteSpace(web) && !string.IsNullOrWhiteSpace(user) && !string.IsNullOrWhiteSpace(pass) && DataGrid_Accounts.SelectedRows.Count >= 1)
            {
                lbl_web.Visible = false;
                lbl_user.Visible = false;
                lbl_pass.Visible = false;
                txtBox_pass.Visible = false;
                txtBox_username.Visible = false;
                txtBox_website.Visible = false;
                btn_Done.Visible = false;
                btn_random.Visible = false;
                btn_AddAccount.Enabled = true;
                btn_DeleteAccount.Enabled = true;
                int curIndex = DataGrid_Accounts.CurrentRow.Index;
                string id = DataGrid_Accounts.Rows[curIndex].Cells[0].Value.ToString();
                DataGrid_Accounts.Rows[curIndex].Cells[0].Value = id;
                DataGrid_Accounts.Rows[curIndex].Cells[1].Value = web;
                DataGrid_Accounts.Rows[curIndex].Cells[2].Value = user;
                DataGrid_Accounts.Rows[curIndex].Cells[3].Value = pass;
                await WebServer.bolt_EditAccountAsync(id, web, user, pass);
            }
        }

        /// <summary>
        /// Submit account to user's table in database(async) and add account to clients's datagridview
        /// </summary>
        private async void btn_Submit_Click(object sender, EventArgs e)
        {
            string web = txtBox_website.Text;
            string user = txtBox_username.Text;
            string pass = txtBox_pass.Text;
            if (isStrValid(web) && !string.IsNullOrWhiteSpace(web) && !string.IsNullOrWhiteSpace(user) && !string.IsNullOrWhiteSpace(pass))
            {
                lbl_web.Visible = false;
                lbl_user.Visible = false;
                lbl_pass.Visible = false;
                txtBox_pass.Visible = false;
                txtBox_username.Visible = false;
                txtBox_website.Visible = false;
                btn_Submit.Visible = false;
                btn_random.Visible = false;
                btn_EditAccount.Enabled = true;
                btn_DeleteAccount.Enabled = true;
                lastIdValue++;
                string[] account = { lastIdValue.ToString(), web, user, pass };
                DataGrid_Accounts.Rows.Add(account);
                await WebServer.bolt_AddAccountAsync( web, user, pass);
            }
        }

        /// <summary>
        /// Generate random password
        /// </summary>
        private void btn_random_Click(object sender, EventArgs e)
        {
            if (btn_AddAccount.Enabled || btn_EditAccount.Enabled)
            {
                string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789!@$?_-";
                char[] chars = new char[16];
                Random rd = new Random();
                for (int i = 0; i < 16; i++)
                {
                    chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
                }
                txtBox_pass.Text = new string(chars);
            }
        }

        /// <summary>
        /// Prepare Datagridview for population
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            DataGrid_Accounts.Rows.Insert(0, "");
            refreshDataList();
        }

        /// <summary>
        /// Close current form and return to loginform
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_logout_Click(object sender, EventArgs e)
        {
            WebServer.Logout();
            this.Close();
        }

        /// <summary>
        /// Destroy sessions/cookes then Close current form and exit.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_exit_Click(object sender, EventArgs e)
        {
            WebServer.Logout();
            this.Close();
            Application.Exit();
        }

    }
}
