using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using CSharpWin_JD.CaptureImage;
using DevExpress.XtraEditors;

namespace TextDetect
{
    public partial class MainForm : DevExpress.XtraEditors.XtraForm
    {
        private Image_Identify_Helper image_helper = new Image_Identify_Helper();

        public MainForm()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            wizardControl1.SelectedPage = wizardPage1;
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            wizardControl1.SelectedPage = wizardPage2;
            //simpleButton6.PerformClick();
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            if (System.DateTime.Now.ToLongDateString() != TextDetect.Properties.Settings.Default.date_record)
                TextDetect.Properties.Settings.Default.used_cnt = 0;
            if (!licence_helper.enable)
            {
                XtraMessageBox.Show("当日试用次数用完！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return ;
            }
            OpenFileDialog filename = new OpenFileDialog();filename.InitialDirectory = Application.StartupPath;
            filename.Filter = "All files(*.*)|*.*|image files|*.jpg;*.png;*.bmp";
            filename.FilterIndex = 2;
            filename.RestoreDirectory = true;
            if (filename.ShowDialog() == DialogResult.OK)
            {
                splashScreenManager1.ShowWaitForm();
                pictureEdit1.Image = Image.FromFile(filename.FileName);
                richTextBox1.Text = image_helper.GetText(filename.FileName);
                textBox1.Text = filename.FileName;
                splashScreenManager1.CloseWaitForm();
                if (!toggleSwitch1.IsOn&&(richTextBox1.Text!=""))
                {
                    Clipboard.SetText(richTextBox1.Text);
                    XtraMessageBox.Show("文本已经复制到剪切板", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
               

            }
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            if (System.DateTime.Now.ToLongDateString() != TextDetect.Properties.Settings.Default.date_record)
                TextDetect.Properties.Settings.Default.used_cnt = 0;
            if (!licence_helper.enable)
            {
                XtraMessageBox.Show("当日试用次数用完！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return ;
            }
            //capture.SelectCursor = new Cursor(Properties.Resources.Arrow_M.Handle); 
            this.Hide();
            Thread.Sleep(600);
            Application.DoEvents();
            CaptureImageTool capture = new CaptureImageTool();
            if (capture.ShowDialog() == DialogResult.OK)
            {
                if (!Visible)
                {
                    this.BringToFront();
                    this.Show();
                }

                splashScreenManager1.ShowWaitForm();
                Image image = capture.Image;
                pictureEdit2.Image = image;
                richTextBox2.Text = image_helper.GetText(image);
                splashScreenManager1.CloseWaitForm();
                if (!toggleSwitch2.IsOn)
                {
                    Clipboard.SetText(richTextBox2.Text);
                    XtraMessageBox.Show("文本已经复制到剪切板", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }


            if (!Visible)
            {
                this.BringToFront();
                this.Show();
            }
        }

        private void wizardControl1_KeyDown(object sender, KeyEventArgs e)
        {
         
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt && e.KeyCode == Keys.A)
            {
                simpleButton6.PerformClick();
            }
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            wizardControl1.SelectedPage = wizardPage3;
        }

        private void simpleButton9_Click(object sender, EventArgs e)
        {
            wizardControl1.SelectedPage = welcomeWizardPage1;
        }

        private void wizardControl1_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            XtraMessageBox.Show("功能尚完善中，请联系作者！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            wizardControl1.SelectedPage = wizardPage4;
        }

        private void wizardControl1_SelectedPageChanged(object sender, DevExpress.XtraWizard.WizardPageChangedEventArgs e)
        {
            if (e.Page == wizardPage4)
            {
                if (System.DateTime.Now.ToLongDateString() != TextDetect.Properties.Settings.Default.date_record)
                {    
                    TextDetect.Properties.Settings.Default.used_cnt = 0;
                    TextDetect.Properties.Settings.Default.Save();
                }
                labelControl4.Text = "当前软件版权：" + (licence_helper.licenced ? "正式版" : "试用版");
                labelControl6.Text = "(日最大识别次数10次，已使用" + TextDetect.Properties.Settings.Default.used_cnt + "次)";
              
               labelControl6.Visible = simpleButton12.Visible = !licence_helper.licenced;
            }
        }

        private void simpleButton12_Click(object sender, EventArgs e)
        {
            if (licence_helper.set_code(textEdit1.Text))
            {
                licence_helper.generate_key();
                TextDetect.Properties.Settings.Default.Save();
                labelControl6.Visible = simpleButton12.Visible = !licence_helper.licenced;
                labelControl4.Text = "当前软件版权：" + (licence_helper.licenced ? "正式版" : "试用版");
                XtraMessageBox.Show("注册成功，谢谢使用！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                XtraMessageBox.Show("注册失败，请确认序列码后重试！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }
    }
}
