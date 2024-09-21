using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DLLInjector
{
    public partial class MainForm : Form
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint dwSize, out int lpNumberOfBytesWritten);

        [DllImport("kernel32.dll")]
        private static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, out uint lpThreadId);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        private const uint PROCESS_ALL_ACCESS = 0x1F0FFF;
        private const uint MEM_COMMIT = 0x1000;
        private const uint MEM_RESERVE = 0x2000;
        private const uint PAGE_EXECUTE_READWRITE = 0x40;

        public MainForm()
        {
            InitializeComponent();
            this.MaximizeBox = false; // منع تكبير النافذة
            SetupUI();
        }

        private void SetupUI()
        {
            this.Text = "DLL Injector";
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(500, 300);

            Label lblProcess = new Label
            {
                Text = "Select Process:",
                Location = new System.Drawing.Point(20, 20),
                AutoSize = true
            };
            this.Controls.Add(lblProcess);

            ComboBox cbProcesses = new ComboBox
            {
                Name = "cbProcesses",
                Location = new System.Drawing.Point(20, 50),
                Size = new System.Drawing.Size(350, 25)
            };
            cbProcesses.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Controls.Add(cbProcesses);
            LoadProcesses(cbProcesses);

            Label lblDll = new Label
            {
                Text = "Select DLL:",
                Location = new System.Drawing.Point(20, 90),
                AutoSize = true
            };
            this.Controls.Add(lblDll);

            TextBox txtDllPath = new TextBox
            {
                Name = "txtDllPath",
                Location = new System.Drawing.Point(20, 120),
                Size = new System.Drawing.Size(350, 25)
            };
            this.Controls.Add(txtDllPath);

            Button btnSelectDll = new Button
            {
                Text = "Choose DLL",
                Location = new System.Drawing.Point(380, 120),
                Size = new System.Drawing.Size(100, 25),
                BackColor = System.Drawing.Color.DarkCyan,
                ForeColor = System.Drawing.Color.White
            };
            btnSelectDll.Click += (s, e) => ChooseDll(txtDllPath);
            this.Controls.Add(btnSelectDll);

            Button btnInject = new Button
            {
                Text = "Inject DLL",
                Location = new System.Drawing.Point(200, 180),
                Size = new System.Drawing.Size(100, 30),
                BackColor = System.Drawing.Color.Green,
                ForeColor = System.Drawing.Color.White
            };
            btnInject.Click += (s, e) => InjectDLL(cbProcesses.SelectedItem.ToString(), txtDllPath.Text);
            this.Controls.Add(btnInject);
        }

        private void LoadProcesses(ComboBox cbProcesses)
        {
            foreach (var process in Process.GetProcesses())
            {
                cbProcesses.Items.Add(process.ProcessName);
            }
            if (cbProcesses.Items.Count > 0)
                cbProcesses.SelectedIndex = 0; // Select the first process by default
        }

        private void ChooseDll(TextBox txtDllPath)
        {
            using (var dllSelectDialog = new OpenFileDialog())
            {
                dllSelectDialog.Filter = "DLL Files (*.dll)|*.dll";
                if (dllSelectDialog.ShowDialog() == DialogResult.OK)
                {
                    txtDllPath.Text = dllSelectDialog.FileName;
                }
            }
        }

        private void InjectDLL(string processName, string dllPath)
        {
            if (string.IsNullOrEmpty(processName) || string.IsNullOrEmpty(dllPath))
            {
                MessageBox.Show("Please select a process and a DLL.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Process process = Process.GetProcessesByName(processName)[0];
            IntPtr hProcess = OpenProcess(PROCESS_ALL_ACCESS, false, process.Id);

            IntPtr allocMemAddress = VirtualAllocEx(hProcess, IntPtr.Zero, (uint)((dllPath.Length + 1) * Marshal.SizeOf(typeof(char))), MEM_COMMIT | MEM_RESERVE, PAGE_EXECUTE_READWRITE);
            byte[] dllBytes = System.Text.Encoding.Default.GetBytes(dllPath);
            WriteProcessMemory(hProcess, allocMemAddress, dllBytes, (uint)dllBytes.Length, out _);

            IntPtr loadLibraryAddr = GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA");
            CreateRemoteThread(hProcess, IntPtr.Zero, 0, loadLibraryAddr, allocMemAddress, 0, out _);

            CloseHandle(hProcess);
            MessageBox.Show("DLL injected successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
