using System;
using System.Windows.Forms;

namespace CodeCoverage
{
    public partial class TrayAppContextMenu : Form
    {
        private NotifyIcon trayIcon;
        private ContextMenu trayMenu;

        public TrayAppContextMenu()
        {
            trayMenu = new ContextMenu();
            trayIcon = new NotifyIcon();
            trayIcon.Text = "Code Coverage";
            trayIcon.Icon = Properties.Resources.CodeCoverage;
            trayIcon.ContextMenu = trayMenu;
            trayIcon.Visible = true;
            WireUpEvents();
        }

        protected override void OnLoad(EventArgs e)
        {
            this.Visible = false; // Hide form window.
            this.ShowInTaskbar = false; // Remove from taskbar.
            base.OnLoad(e);
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                trayIcon.Dispose();
            }
            base.Dispose(isDisposing);
        }
    }
}
