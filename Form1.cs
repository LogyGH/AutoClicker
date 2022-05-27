using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Autoclicker
{
    public partial class Form1 : Form
    {
        private const int WH_KEYBOARD_LL = 13;
        private LowLevelKeyboardProcDelegate m_callback;
        private IntPtr m_hHook;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProcDelegate lpfn, IntPtr hMod, int dwThreadId);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("Kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetModuleHandle(IntPtr lpModuleName);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        private IntPtr LowLevelKeyboardHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                var khs = (KeyboardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyboardHookStruct));
                if (Convert.ToInt32("" + wParam) == 256)
                {
                    if ((int)khs.VirtualKeyCode == 113 && _isStart == true)//F2
                    {
                        button2.PerformClick();
                        WindowState = FormWindowState.Normal;
                    }
                    else if ((int)khs.VirtualKeyCode == 113 && _isStart == false)//F2
                    {
                        buttonStart.PerformClick();
                        WindowState = FormWindowState.Normal;
                    }
                    else if ((int)khs.VirtualKeyCode == 114)//F3
                    {
                        buttonGetPos.PerformClick();
                        WindowState = FormWindowState.Normal;
                    }
                    else if ((int)khs.VirtualKeyCode == 115)//F4
                    {
                        buttonSetPos.PerformClick();
                        WindowState = FormWindowState.Normal;
                    }
                }
            }
            return CallNextHookEx(m_hHook, nCode, wParam, lParam);
        }
        [StructLayout(LayoutKind.Sequential)]
        private struct KeyboardHookStruct
        {
            public readonly int VirtualKeyCode;
            public readonly int ScanCode;
            public readonly int Flags;
            public readonly int Time;
            public readonly IntPtr ExtraInfo;
        }
        private delegate IntPtr LowLevelKeyboardProcDelegate(int nCode, IntPtr wParam, IntPtr lParam);

        public void SetHook()
        {
            m_callback = LowLevelKeyboardHookProc;
            m_hHook = SetWindowsHookEx(WH_KEYBOARD_LL, m_callback, GetModuleHandle(IntPtr.Zero), 0);
        }
        public void Unhook()
        {
            UnhookWindowsHookEx(m_hHook);
        }

        [DllImport("user32.dll")]
        static extern bool GetCursorPos(ref Point lpPoint);

        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        //Нажатие на левую кнопку мыши
        private const int MOUSEEVENTF_LEFTDOWN = 0x0002;
        //Поднятие левой кнопки мыши
        private const int MOUSEEVENTF_LEFTUP = 0x0004;
        //Нажатие на правой кнопку мыши
        private const int MOUSEEVENTF_RIGHTDOWN = 0x0008;
        //Поднятие правой кнопки мыши
        private const int MOUSEEVENTF_RIGHTUP = 0x0010;

        Point lastPoint;
        Point defPnt = new Point();

        static public bool _isStart = false;
        static public bool _isSet = false;
        public Form1()
        {
            InitializeComponent();
            SetHook();
            this.Focus();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void panelTop_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }

        private void panelTop_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }

        private void panelMain_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }

        private void panelMain_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _isStart = false;
            buttonStart.Enabled = true;
            buttonGetPos.Enabled = true;
            buttonSetPos.Enabled = true;
            button2.Enabled = false;
        }

        async private void buttonStart_Click(object sender, EventArgs e)
        {
            bool check1 = false;
            if (textBoxMin.Text == "")
            {
                textBoxMin.Text = "0";
                check1 = true;
            }
            else
            {
                for (int i = 0; i < textBoxMin.Text.Length; i++)
                {
                    if (char.IsLetter(textBoxMin.Text[i]) || char.IsPunctuation(textBoxMin.Text[i]) || char.IsWhiteSpace(textBoxMin.Text[i]) ||
                        char.IsSeparator(textBoxMin.Text[i]) || char.IsSymbol(textBoxMin.Text[i]))
                    {
                        break;
                    }
                    if (i == textBoxMin.Text.Length - 1)
                    {
                        check1 = true;
                    }
                }
            }

            bool check2 = false;
            if (textBoxSec.Text == "" && check1)
            {
                textBoxSec.Text = "0";
                check2 = true;
            }
            else if (check1)
            {
                for (int i = 0; i < textBoxSec.Text.Length; i++)
                {
                    if (char.IsLetter(textBoxSec.Text[i]) || char.IsPunctuation(textBoxSec.Text[i]) || char.IsWhiteSpace(textBoxSec.Text[i]) ||
                        char.IsSeparator(textBoxSec.Text[i]) || char.IsSymbol(textBoxSec.Text[i]))
                    {
                        break;
                    }
                    if (i == textBoxSec.Text.Length - 1)
                    {
                        check2 = true;
                    }
                }
            }

            bool check3 = false;
            if (textBoxMsec.Text == "" && check1 && check2)
            {
                if (MessageBox.Show("Minimum interval 50 msec.\nDo you want to set 50 msec?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    textBoxMsec.Text = "50";
                    check3 = true;
                }
            }
            else if (check1 && check2)
            {
                check3 = false;
                for (int i = 0; i < textBoxMsec.Text.Length; i++)
                {
                    if (char.IsLetter(textBoxMsec.Text[i]) || char.IsPunctuation(textBoxMsec.Text[i]) || char.IsWhiteSpace(textBoxMsec.Text[i]) ||
                        char.IsSeparator(textBoxMsec.Text[i]) || char.IsSymbol(textBoxMsec.Text[i]))
                    {
                        break;
                    }
                    if (i == textBoxMsec.Text.Length - 1 && int.Parse(textBoxMsec.Text) >= 50)
                    {
                        check3 = true;
                    }
                }
            }
            if (check1 && check2 && check3)
            {
                _isStart = true;
                buttonStart.Enabled = false;
                buttonGetPos.Enabled = false;
                buttonSetPos.Enabled = false;
                button2.Enabled = true;
                if (radioButtonRepeatU.Checked == true)
                {
                    while (_isStart)
                    {
                        SetCursorPos(int.Parse(textBoxX.Text), int.Parse(textBoxY.Text));
                        if (radioButtonLeft.Checked == true && radioButtonSingle.Checked == true)
                        {
                            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
                        }
                        else if (radioButtonLeft.Checked == true && radioButtonDouble.Checked == true)
                        {
                            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
                            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
                        }
                        else if (radioButtonRight.Checked == true && radioButtonSingle.Checked == true)
                        {
                            mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
                        }
                        else if (radioButtonRight.Checked == true && radioButtonDouble.Checked == true)
                        {
                            mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
                            mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
                        }
                        await Task.Delay(60000 * Convert.ToInt32(textBoxMin.Text) + 1000 * Convert.ToInt32(textBoxSec.Text) + Convert.ToInt32(textBoxMsec.Text));
                    }
                }
                else if (radioButtonRepeat.Checked == true)
                {
                    int end = int.Parse(numericUpDown1.Value.ToString());
                    for (int i = 0; i < end; i++)
                    {
                        if (_isStart)
                        {
                            SetCursorPos(int.Parse(textBoxX.Text), int.Parse(textBoxY.Text));
                            if (radioButtonLeft.Checked == true && radioButtonSingle.Checked == true)
                            {
                                mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
                            }
                            else if (radioButtonLeft.Checked == true && radioButtonDouble.Checked == true)
                            {
                                mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
                                mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
                            }
                            else if (radioButtonRight.Checked == true && radioButtonSingle.Checked == true)
                            {
                                mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
                            }
                            else if (radioButtonRight.Checked == true && radioButtonDouble.Checked == true)
                            {
                                mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
                                mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
                            }
                            if (i < end - 1)
                            {
                                numericUpDown1.Value = numericUpDown1.Value - 1;
                            }
                            if (i != end - 1)
                            {
                                await Task.Delay(60000 * Convert.ToInt32(textBoxMin.Text) + 1000 * Convert.ToInt32(textBoxSec.Text) + Convert.ToInt32(textBoxMsec.Text));
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    button2.PerformClick();
                }
            }
            else
            {
                MessageBox.Show("Set the interval. Minimum 50 msec.", "Error");
            }
        }

        private void textBoxMin_KeyPress(object sender, KeyPressEventArgs e)
        {
            char num = e.KeyChar;

            if (!char.IsDigit(num) && !char.IsControl(num))
            {
                e.Handled = true;
            }
        }

        async private void buttonGetPos_Click(object sender, EventArgs e)
        {
            _isSet = false;
            buttonStart.Enabled = false;
            while (!_isSet)
            {
                GetCursorPos(ref defPnt);
                textBoxX.Text = defPnt.X.ToString();
                textBoxY.Text = defPnt.Y.ToString();
                await Task.Delay(10);
            }
        }

        private void buttonSetPos_Click(object sender, EventArgs e)
        {
            _isSet = true;
            buttonStart.Enabled = true;
            GetCursorPos(ref defPnt);
            textBoxX.Text = defPnt.X.ToString();
            textBoxY.Text = defPnt.Y.ToString();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            panelInfo.Dock = DockStyle.Fill;
            pictureBox4.Visible = false;
            pictureBox5.Visible = false;
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            _isSet = true;
            button2.PerformClick();
            buttonStart.Enabled = false;
            textBoxMin.Text = "0";
            textBoxMsec.Text = "0";
            textBoxSec.Text = "0";
            textBoxX.Text = "0";
            textBoxY.Text = "0";
            numericUpDown1.Value = 1;
        }

        private void panelInfo_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }

        private void panelInfo_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            panelInfo.Dock = DockStyle.None;
            pictureBox4.Visible = true;
            pictureBox5.Visible = true;
        }

        private void label1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }

        private void label1_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }

        private void label10_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }

        private void label10_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }
    }
}
