using PusherClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace _1CareAssist.Desktop
{
    public partial class mainMenu : Form
    {
        private NotifyIcon trayIcon;
        private ContextMenu trayMenu;

        static Pusher _pusher = null;
        static Channel _chatChannel = null;
 
        public mainMenu()
        {
            InitializeComponent();

            // Create a simple tray menu with only one item.
            trayMenu = new ContextMenu();
            trayMenu.MenuItems.Add("Exit", OnExit);

            // Create a tray icon. In this example we use a
            // standard system icon for simplicity, but you
            // can of course use your own custom icon too.
            trayIcon = new NotifyIcon();
            trayIcon.Text = "1CareAssist Notifer";
            trayIcon.Icon = new Icon(SystemIcons.Information, 40, 40);
            trayIcon.Icon = new Icon(@"icon_40_2x_2tm_icon.ico");

            // Add menu to tray icon and show it.
            trayIcon.ContextMenu = trayMenu;
            trayIcon.Visible = true;

            StandupPusher();
        }

        private void StandupPusher()
        {
            _pusher = new Pusher("456257a27b92ba4ec1dc", new PusherOptions()
            {

                //Authorizer = new HttpAuthorizer("http://localhost:8888/auth/" + HttpUtility.UrlEncode(_name))
            });
           // _pusher.ConnectionStateChanged += _pusher_ConnectionStateChanged;
           // _pusher.Error += _pusher_Error;

            // Setup private channel
            _chatChannel = _pusher.Subscribe("test_channel");
           

            // Inline binding!
            _chatChannel.Bind("my_event", (dynamic data) =>
            {
                //MessageBox.Show("[" + data.name + "] " + data.message);
                showBalloon(data.name.ToString(), data.message.ToString(), data.url.ToString());
                
            });

            

            _pusher.Connect();
         
            

        }

        private void showBalloon(string title, string body, string url)
        {
            title = "OneCare Alert - " + title;
            NotifyIcon notifyIcon = trayIcon;
            

            if (title != null)
            {
                notifyIcon.BalloonTipTitle = title;
            }

            if (body != null)
            {
                notifyIcon.BalloonTipText = body;
            }
            notifyIcon.Tag = url;

            notifyIcon.BalloonTipClicked += notifyIcon_BalloonTipClicked;
            notifyIcon.ShowBalloonTip(30000);
        }

        void notifyIcon_BalloonTipClicked(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = @"iexplore.exe";
            p.StartInfo.Arguments = ((NotifyIcon)sender).Tag.ToString();

            p.Start();
        }

        private void SendMessage(string msg)
        {
            MessageBox.Show(msg);
        }

        protected override void OnLoad(EventArgs e)
        {
            Visible = false; // Hide form window.
            ShowInTaskbar = false; // Remove from taskbar.

            base.OnLoad(e);
        }
        private void OnExit(object sender, EventArgs e)
        {
            Application.Exit();
        }

     
    }
}
