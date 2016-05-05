using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CodeCoverage
{
    internal partial class CodeCoverageContext : ApplicationContext
    {
        // Declaration of components
        private NotifyIcon TrayIcon;
        private ContextMenu TrayIconContextMenu;
        private MenuItem SessionMenuItem;
        private MenuItem StartSessionMenuItem;
        private MenuItem StopSessionMenuItem;
        private MenuItem OpenCoverageReportMenuItem;
        private MenuItem SettingsMenuItem;
        private MenuItem ViewLogMenuItem;
        private MenuItem ExitMenuItem;
        private StringBuilder OutputLog;
        private StringBuilder ErrorLog;

        /// <summary>
        /// CodeCoverageContext
        /// </summary>
        public CodeCoverageContext()
        {
            InitializeComponent();
        }

        /// <summary>
        /// InitializeComponent
        /// </summary>
        private void InitializeComponent()
        {
            TrayIcon = new NotifyIcon();
            SessionMenuItem = new MenuItem();
            StartSessionMenuItem = new MenuItem();
            StopSessionMenuItem = new MenuItem();
            OpenCoverageReportMenuItem = new MenuItem();
            ViewLogMenuItem = new MenuItem();
            SettingsMenuItem = new MenuItem();
            ExitMenuItem = new MenuItem();

            SessionMenuItem.Name = "SessionMenuItem";
            SessionMenuItem.Text = "New Session";

            StartSessionMenuItem.Name = "StartSessionMenuItem";
            StartSessionMenuItem.Text = "Start";
            StartSessionMenuItem.Click += StartSessionMenuItem_Click;

            StopSessionMenuItem.Name = "StopSessionMenuItem";
            StopSessionMenuItem.Text = "Stop";
            StopSessionMenuItem.Click += StopSessionMenuItem_Click;

            OpenCoverageReportMenuItem.Name = "OpenCoverageReportMenuItem";
            OpenCoverageReportMenuItem.Text = "Open Report";
            OpenCoverageReportMenuItem.Click += ViewCoverageReportMenuItem_Click;

            ViewLogMenuItem.Name = "ViewLogMenuItem";
            ViewLogMenuItem.Text = "View Log";
            ViewLogMenuItem.Click += ViewLogMenuItem_Click;

            SettingsMenuItem.Name = "SettingsMenuItem";
            SettingsMenuItem.Text = "Settings";
            SettingsMenuItem.Click += SettingsMenuItem_Click;

            ExitMenuItem.Name = "ExitMenuItem";
            ExitMenuItem.Text = "Exit";
            ExitMenuItem.Click += ExitMenuItem_Click;

            SessionMenuItem.MenuItems.AddRange(new MenuItem[] 
            {
                StartSessionMenuItem,
                StopSessionMenuItem
            });

            TrayIconContextMenu = new ContextMenu();
            TrayIconContextMenu.MenuItems.AddRange(new MenuItem[]
            {
                SessionMenuItem,
                OpenCoverageReportMenuItem,
                //ViewLogMenuItem,
                SettingsMenuItem,
                ExitMenuItem
            });

            TrayIcon.Text = "Code Coverage";
            TrayIcon.Icon = Properties.Resources.CodeCoverage;
            TrayIcon.ContextMenu = TrayIconContextMenu;
            TrayIcon.Visible = true;

            OutputLog = new StringBuilder();
            ErrorLog = new StringBuilder();
        }        

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing && TrayIcon != null)
            {
                TrayIcon.Dispose();
            }
            base.Dispose(isDisposing);
        }
    }
}