using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;

namespace DLLInjector
{
    public partial class MainForm : Form
    {

        // Import necessary functions from the Windows API




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

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("kernel32.dll")]
        static extern IntPtr OpenThread(int dwDesiredAccess, bool bInheritHandle, int dwThreadId);

        [DllImport("kernel32.dll")]
        static extern bool QueueUserAPC(IntPtr pfnAPC, IntPtr hThread, uint dwData);


        private const uint PROCESS_ALL_ACCESS = 0x1F0FFF;
        private const uint MEM_COMMIT = 0x1000;

        private const uint MEM_RESERVE = 0x2000;
        private const uint PAGE_EXECUTE_READWRITE = 0x40;
        const int THREAD_ALL_ACCESS = 0x1F0FFF; // This constant allows full access to the thread.

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
            this.ClientSize = new System.Drawing.Size(500, 500);

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
                Location = new System.Drawing.Point(100, 180),
                Size = new System.Drawing.Size(100, 30),
                BackColor = System.Drawing.Color.Green,
                ForeColor = System.Drawing.Color.White
            };
            btnInject.Click += (s, e) => InjectDLL(cbProcesses.SelectedItem.ToString(), txtDllPath.Text);
            this.Controls.Add(btnInject);

            Button btnAdvancedInject = new Button
            {
                Text = "Advanced Inject",
                Location = new System.Drawing.Point(250, 180),
                Size = new System.Drawing.Size(150, 30),
                BackColor = System.Drawing.Color.Blue,
                ForeColor = System.Drawing.Color.White
            };
            btnAdvancedInject.Click += (s, e) => AdvancedInjectDLL(cbProcesses.SelectedItem.ToString(), txtDllPath.Text);
            this.Controls.Add(btnAdvancedInject);

            // زر حقن إضافي 1
            Button btnInjectMethod2 = new Button
            {
                Text = "Inject Method 2",
                Location = new System.Drawing.Point(100, 230),
                Size = new System.Drawing.Size(150, 30),
                BackColor = System.Drawing.Color.Orange,
                ForeColor = System.Drawing.Color.White
            };
            btnInjectMethod2.Click += (s, e) => InjectMethod2(cbProcesses.SelectedItem.ToString(), txtDllPath.Text);
            this.Controls.Add(btnInjectMethod2);

            // زر حقن إضافي 2
            Button btnInjectMethod3 = new Button
            {
                Text = "Inject Method 3",
                Location = new System.Drawing.Point(250, 230),
                Size = new System.Drawing.Size(150, 30),
                BackColor = System.Drawing.Color.Red,
                ForeColor = System.Drawing.Color.White
            };
            btnInjectMethod3.Click += (s, e) => InjectMethod3(cbProcesses.SelectedItem.ToString(), txtDllPath.Text);
            this.Controls.Add(btnInjectMethod3);

            // زر حقن إضافي 3
            Button btnInjectMethod4 = new Button
            {
                Text = "Inject Method 4",
                Location = new System.Drawing.Point(100, 280),
                Size = new System.Drawing.Size(150, 30),
                BackColor = System.Drawing.Color.Purple,
                ForeColor = System.Drawing.Color.White
            };
            btnInjectMethod4.Click += (s, e) => InjectMethod4(cbProcesses.SelectedItem.ToString(), txtDllPath.Text);
            this.Controls.Add(btnInjectMethod4);

            // زر حقن إضافي 4
            Button btnInjectMethod5 = new Button
            {
                Text = "Inject Method 5",
                Location = new System.Drawing.Point(250, 280),
                Size = new System.Drawing.Size(150, 30),
                BackColor = System.Drawing.Color.Teal,
                ForeColor = System.Drawing.Color.White
            };
            btnInjectMethod5.Click += (s, e) => InjectMethod5(cbProcesses.SelectedItem.ToString(), txtDllPath.Text);
            this.Controls.Add(btnInjectMethod5);

            // زر حقن إضافي 5
            Button btnInjectMethod6 = new Button
            {
                Text = "Inject Method 6",
                Location = new System.Drawing.Point(100, 330),
                Size = new System.Drawing.Size(150, 30),
                BackColor = System.Drawing.Color.Blue,
                ForeColor = System.Drawing.Color.White
            };
            btnInjectMethod6.Click += (s, e) => InjectMethod6(cbProcesses.SelectedItem.ToString(), txtDllPath.Text);
            this.Controls.Add(btnInjectMethod6);

            // زر حقن إضافي 6
            Button btnInjectMethod7 = new Button
            {
                Text = "Inject Method 7",
                Location = new System.Drawing.Point(250, 330),
                Size = new System.Drawing.Size(150, 30),
                BackColor = System.Drawing.Color.Orange,
                ForeColor = System.Drawing.Color.White
            };
            btnInjectMethod7.Click += (s, e) => InjectMethod7(cbProcesses.SelectedItem.ToString(), txtDllPath.Text);
            this.Controls.Add(btnInjectMethod7);


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

            try
            {
                Process process = Process.GetProcessesByName(processName)[0];
                IntPtr hProcess = OpenProcess(PROCESS_ALL_ACCESS, false, process.Id);

                IntPtr allocMemAddress = VirtualAllocEx(hProcess, IntPtr.Zero, (uint)((dllPath.Length + 1) * Marshal.SizeOf(typeof(char))), MEM_COMMIT | MEM_RESERVE, PAGE_EXECUTE_READWRITE);

                if (allocMemAddress == IntPtr.Zero)
                {
                    MessageBox.Show("Failed to allocate memory in the target process.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                byte[] dllBytes = System.Text.Encoding.Default.GetBytes(dllPath);
                if (!WriteProcessMemory(hProcess, allocMemAddress, dllBytes, (uint)dllBytes.Length, out _))
                {
                    MessageBox.Show("Failed to write memory in the target process.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                IntPtr loadLibraryAddr = GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA");
                CreateRemoteThread(hProcess, IntPtr.Zero, 0, loadLibraryAddr, allocMemAddress, 0, out _);

                CloseHandle(hProcess);
                MessageBox.Show("DLL injected successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AdvancedInjectDLL(string processName, string dllPath)
        {
            if (string.IsNullOrEmpty(processName) || string.IsNullOrEmpty(dllPath))
            {
                MessageBox.Show("Please select a process and a DLL.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                Process process = Process.GetProcessesByName(processName)[0];
                IntPtr hProcess = OpenProcess(PROCESS_ALL_ACCESS, false, process.Id);

                IntPtr allocMemAddress = VirtualAllocEx(hProcess, IntPtr.Zero, (uint)((dllPath.Length + 1) * Marshal.SizeOf(typeof(char))), MEM_COMMIT | MEM_RESERVE, PAGE_EXECUTE_READWRITE);

                if (allocMemAddress == IntPtr.Zero)
                {
                    MessageBox.Show("Failed to allocate memory in the target process.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                byte[] dllBytes = System.Text.Encoding.Default.GetBytes(dllPath);
                if (!WriteProcessMemory(hProcess, allocMemAddress, dllBytes, (uint)dllBytes.Length, out _))
                {
                    MessageBox.Show("Failed to write memory in the target process.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                IntPtr loadLibraryAddr = GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA");
                CreateRemoteThread(hProcess, IntPtr.Zero, 0, loadLibraryAddr, allocMemAddress, 0, out _);

                CloseHandle(hProcess);
                MessageBox.Show("Advanced DLL injected successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // طريقة الحقن 2
        private void InjectMethod2(string processName, string dllPath)
        {
            if (string.IsNullOrEmpty(processName) || string.IsNullOrEmpty(dllPath))
            {
                MessageBox.Show("Please select a process and a DLL.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                Process process = Process.GetProcessesByName(processName)[0];
                IntPtr hProcess = OpenProcess(PROCESS_ALL_ACCESS, false, process.Id);

                IntPtr allocMemAddress = VirtualAllocEx(hProcess, IntPtr.Zero, (uint)((dllPath.Length + 1) * Marshal.SizeOf(typeof(char))), MEM_COMMIT | MEM_RESERVE, PAGE_EXECUTE_READWRITE);

                if (allocMemAddress == IntPtr.Zero)
                {
                    MessageBox.Show("Failed to allocate memory in the target process.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                byte[] dllBytes = System.Text.Encoding.Default.GetBytes(dllPath);
                if (!WriteProcessMemory(hProcess, allocMemAddress, dllBytes, (uint)dllBytes.Length, out _))
                {
                    MessageBox.Show("Failed to write memory in the target process.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // حقن DLL باستخدام تقنية `Reflective DLL Injection`
                IntPtr reflectiveInjectionMethod = GetProcAddress(GetModuleHandle("kernel32.dll"), "CreateRemoteThread");
                CreateRemoteThread(hProcess, IntPtr.Zero, 0, reflectiveInjectionMethod, allocMemAddress, 0, out _);

                CloseHandle(hProcess);
                MessageBox.Show("DLL injected using Reflective Injection method!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // طريقة الحقن 3
        private void InjectMethod3(string processName, string dllPath)
        {
            if (string.IsNullOrEmpty(processName) || string.IsNullOrEmpty(dllPath))
            {
                MessageBox.Show("Please select a process and a DLL.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                Process process = Process.GetProcessesByName(processName)[0];
                IntPtr hProcess = OpenProcess(PROCESS_ALL_ACCESS, false, process.Id);

                IntPtr allocMemAddress = VirtualAllocEx(hProcess, IntPtr.Zero, (uint)((dllPath.Length + 1) * Marshal.SizeOf(typeof(char))), MEM_COMMIT | MEM_RESERVE, PAGE_EXECUTE_READWRITE);

                if (allocMemAddress == IntPtr.Zero)
                {
                    MessageBox.Show("Failed to allocate memory in the target process.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                byte[] dllBytes = System.Text.Encoding.Default.GetBytes(dllPath);
                if (!WriteProcessMemory(hProcess, allocMemAddress, dllBytes, (uint)dllBytes.Length, out _))
                {
                    MessageBox.Show("Failed to write memory in the target process.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // حقن DLL باستخدام تقنية `APC Injection`
                IntPtr apcInjectionMethod = GetProcAddress(GetModuleHandle("kernel32.dll"), "QueueUserAPC");
                CreateRemoteThread(hProcess, IntPtr.Zero, 0, apcInjectionMethod, allocMemAddress, 0, out _);

                CloseHandle(hProcess);
                MessageBox.Show("DLL injected using APC Injection method!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // طريقة الحقن 4
        private void InjectMethod4(string processName, string dllPath)
        {
            if (string.IsNullOrEmpty(processName) || string.IsNullOrEmpty(dllPath))
            {
                MessageBox.Show("Please select a process and a DLL.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                Process process = Process.GetProcessesByName(processName)[0];
                IntPtr hProcess = OpenProcess(PROCESS_ALL_ACCESS, false, process.Id);

                IntPtr allocMemAddress = VirtualAllocEx(hProcess, IntPtr.Zero, (uint)((dllPath.Length + 1) * Marshal.SizeOf(typeof(char))), MEM_COMMIT | MEM_RESERVE, PAGE_EXECUTE_READWRITE);

                if (allocMemAddress == IntPtr.Zero)
                {
                    MessageBox.Show("Failed to allocate memory in the target process.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                byte[] dllBytes = System.Text.Encoding.Default.GetBytes(dllPath);
                if (!WriteProcessMemory(hProcess, allocMemAddress, dllBytes, (uint)dllBytes.Length, out _))
                {
                    MessageBox.Show("Failed to write memory in the target process.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // حقن DLL باستخدام تقنية `SetWindowsHookEx`
                IntPtr hookMethod = GetProcAddress(GetModuleHandle("user32.dll"), "SetWindowsHookExA");
                CreateRemoteThread(hProcess, IntPtr.Zero, 0, hookMethod, allocMemAddress, 0, out _);

                CloseHandle(hProcess);
                MessageBox.Show("DLL injected using SetWindowsHookEx method!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InjectMethod6(string processName, string dllPath)
        {
            if (string.IsNullOrEmpty(processName) || string.IsNullOrEmpty(dllPath))
            {
                MessageBox.Show("Please select a process and a DLL.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                Process process = Process.GetProcessesByName(processName)[0];
                IntPtr hProcess = OpenProcess(PROCESS_ALL_ACCESS, false, process.Id);

                IntPtr allocMemAddress = VirtualAllocEx(hProcess, IntPtr.Zero, (uint)((dllPath.Length + 1) * Marshal.SizeOf(typeof(char))), MEM_COMMIT | MEM_RESERVE, PAGE_EXECUTE_READWRITE);

                if (allocMemAddress == IntPtr.Zero)
                {
                    MessageBox.Show("Failed to allocate memory in the target process.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                byte[] dllBytes = System.Text.Encoding.Default.GetBytes(dllPath);
                if (!WriteProcessMemory(hProcess, allocMemAddress, dllBytes, (uint)dllBytes.Length, out _))
                {
                    MessageBox.Show("Failed to write memory in the target process.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // حقن DLL باستخدام تقنية `NtCreateThreadEx`
                IntPtr ntCreateThreadExAddr = GetProcAddress(GetModuleHandle("ntdll.dll"), "NtCreateThreadEx");
                CreateRemoteThread(hProcess, IntPtr.Zero, 0, ntCreateThreadExAddr, allocMemAddress, 0, out _);

                CloseHandle(hProcess);
                MessageBox.Show("DLL injected using NtCreateThreadEx method!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }







        // طريقة الحقن 5
        private void InjectMethod5(string processName, string dllPath)
        {
            if (string.IsNullOrEmpty(processName) || string.IsNullOrEmpty(dllPath))
            {
                MessageBox.Show("Please select a process and a DLL.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                Process process = Process.GetProcessesByName(processName)[0];
                IntPtr hProcess = OpenProcess(PROCESS_ALL_ACCESS, false, process.Id);

                IntPtr allocMemAddress = VirtualAllocEx(hProcess, IntPtr.Zero, (uint)((dllPath.Length + 1) * Marshal.SizeOf(typeof(char))), MEM_COMMIT | MEM_RESERVE, PAGE_EXECUTE_READWRITE);

                if (allocMemAddress == IntPtr.Zero)
                {
                    MessageBox.Show("Failed to allocate memory in the target process.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                byte[] dllBytes = System.Text.Encoding.Default.GetBytes(dllPath);
                if (!WriteProcessMemory(hProcess, allocMemAddress, dllBytes, (uint)dllBytes.Length, out _))
                {
                    MessageBox.Show("Failed to write memory in the target process.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // حقن DLL باستخدام تقنية `Memory Mapped File`
                IntPtr mapViewOfFileMethod = GetProcAddress(GetModuleHandle("kernel32.dll"), "MapViewOfFile");
                CreateRemoteThread(hProcess, IntPtr.Zero, 0, mapViewOfFileMethod, allocMemAddress, 0, out _);

                CloseHandle(hProcess);
                MessageBox.Show("DLL injected using Memory Mapped File method!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InjectMethod7(string processName, string dllPath)
        {
            if (string.IsNullOrEmpty(processName) || string.IsNullOrEmpty(dllPath))
            {
                MessageBox.Show("Please select a process and a DLL.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                Process process = Process.GetProcessesByName(processName)[0];
                IntPtr hProcess = OpenProcess(PROCESS_ALL_ACCESS, false, process.Id);

                IntPtr allocMemAddress = VirtualAllocEx(hProcess, IntPtr.Zero, (uint)((dllPath.Length + 1) * Marshal.SizeOf(typeof(char))), MEM_COMMIT | MEM_RESERVE, PAGE_EXECUTE_READWRITE);

                if (allocMemAddress == IntPtr.Zero)
                {
                    MessageBox.Show("Failed to allocate memory in the target process.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                byte[] dllBytes = System.Text.Encoding.Default.GetBytes(dllPath);
                if (!WriteProcessMemory(hProcess, allocMemAddress, dllBytes, (uint)dllBytes.Length, out _))
                {
                    MessageBox.Show("Failed to write memory in the target process.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // حقن DLL باستخدام تقنية `Thread Hijacking`
                IntPtr threadHijackMethod = GetProcAddress(GetModuleHandle("kernel32.dll"), "CreateRemoteThread");
                IntPtr targetThread = OpenThread(THREAD_ALL_ACCESS, false, process.Threads[0].Id);


                // Convert allocMemAddress to uint for the third parameter
                ulong allocMemAddressULong = (ulong)allocMemAddress.ToInt64();
                QueueUserAPC(threadHijackMethod, targetThread, (uint)allocMemAddressULong);
                CloseHandle(targetThread);
                CloseHandle(hProcess);
                MessageBox.Show("DLL injected using Thread Hijacking method!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }
}


