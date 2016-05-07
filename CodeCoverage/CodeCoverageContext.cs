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
        private MenuItem SelectAssembliesMenuItem;
        private MenuItem NewSessionMenuItem;
        private MenuItem OpenCoverageReportMenuItem;
        private MenuItem SettingsMenuItem;
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
            SelectAssembliesMenuItem = new MenuItem();
            NewSessionMenuItem = new MenuItem();
            OpenCoverageReportMenuItem = new MenuItem();
            SettingsMenuItem = new MenuItem();
            ExitMenuItem = new MenuItem();

            SelectAssembliesMenuItem.Name = "SelectAssembliesMenuItem";
            SelectAssembliesMenuItem.Text = "Select Assemblies";
            SelectAssembliesMenuItem.Click += SelectAssembliesMenuItem_Click;

            NewSessionMenuItem.Name = "NewSessionMenuItem";
            NewSessionMenuItem.Text = "Start New Session";
            NewSessionMenuItem.Click += StartSessionMenuItem_Click;

            OpenCoverageReportMenuItem.Name = "OpenCoverageReportMenuItem";
            OpenCoverageReportMenuItem.Text = "Open Report";
            OpenCoverageReportMenuItem.Click += ViewCoverageReportMenuItem_Click;

            SettingsMenuItem.Name = "SettingsMenuItem";
            SettingsMenuItem.Text = "Settings";
            SettingsMenuItem.Click += SettingsMenuItem_Click;

            ExitMenuItem.Name = "ExitMenuItem";
            ExitMenuItem.Text = "Exit";
            ExitMenuItem.Click += ExitMenuItem_Click;

            TrayIconContextMenu = new ContextMenu();
            TrayIconContextMenu.MenuItems.AddRange(new MenuItem[]
            {
                SelectAssembliesMenuItem,
                NewSessionMenuItem,
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