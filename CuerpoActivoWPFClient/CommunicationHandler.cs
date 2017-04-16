using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.AspNet.SignalR.Client;
using SignalRBroadcastServiceSample.Domain;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace WindowsClient
{
    public class CommunicationHandler
    {
        private Reminder Reminder { get; set; }

        public CommunicationHandler()
        {
            InitiateCommunication();
        }

        private void InitiateCommunication()
        {
            var hubConnection = new HubConnection("http://localhost:8084");
            IHubProxy cuerpoActivoHubProxy = hubConnection.CreateHubProxy("CuerpoActivoServiceHub");

            // This line is necessary to subscribe for broadcasting messages
            cuerpoActivoHubProxy.On<Reminder>("NotifyReminder", HandleReminder);

            // Start the connection
            hubConnection.Start().Wait();
        }

        private void HandleReminder(Reminder _reminder)
        {
            Application.Current.Dispatcher.Invoke(new System.Action(() =>
            {
                TaskbarIcon taskBar = new TaskbarIcon();
                taskBar.ShowBalloonTip(_reminder.Reminder_title, _reminder.Reminder_preview_description, BalloonIcon.Info);
            }));
        }
    }
}
