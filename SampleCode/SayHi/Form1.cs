﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LeanCloud.Realtime;
using LeanCloud;

namespace SayHi
{
    public partial class Form1 : Form
    {
        AVRealtime realtime = new AVRealtime("s01a0v363ltv1a63yoj4wx34616kvp33nhjtbre4pydc66zt", "ext6lwnff90qdr1pfdlgvfigh7syciwqmc9zf0gph3nqcwj7");
        AVIMClient client;
        AVIMConversation conversation;
        BindingList<AVIMMessage> data = new BindingList<AVIMMessage>();
        public Form1()
        {
            InitializeComponent();
            Websockets.Net.WebsocketConnection.Link();
            AVRealtime.WebSocketLog(AppendLogs);
            AVClient.Initialize("s01a0v363ltv1a63yoj4wx34616kvp33nhjtbre4pydc66zt", "ext6lwnff90qdr1pfdlgvfigh7syciwqmc9zf0gph3nqcwj7");
        }

        private async void btn_logIn_Click(object sender, EventArgs e)
        {
            client = await realtime.CreateClientAsync(txb_clientId.Text.Trim());
            client.OnMessageReceived += Client_OnMessageReceived;
            lbx_messages.DisplayMember = "Content";
            lbx_messages.ValueMember = "Id";
            lbx_messages.DataSource = data;
        }

        private void Client_OnMessageReceived(object sender, AVIMMessageEventArgs e)
        {
            if (e.Message is AVIMMessage)
            {
                var baseMeseage = e.Message as AVIMMessage;
               
                lbx_messages.Invoke((MethodInvoker)(() =>
                {
                    data.Add(baseMeseage);
                    lbx_messages.Refresh();
                }));
            }
        }

        public void AppendLogs(string log)
        {
            txb_logs.Invoke((MethodInvoker)(() =>
            {
                txb_logs.AppendText(log + "\n");
            }));
        }

        private async void btn_create_Click(object sender, EventArgs e)
        {
            conversation = await client.CreateConversationAsync(txb_friend.Text.Trim());
        }
    }
}
