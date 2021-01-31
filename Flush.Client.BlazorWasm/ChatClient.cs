using System;
using Flush.Models;
using Microsoft.AspNetCore.SignalR.Client;

namespace Flush.Client.BlazorWasm
{
    public class ChatClient : IDisposable
    {
        public event EventHandler<ChatMessage> ReceiveMessage;

        private readonly HubConnection hubConnection;

        public ChatClient()
        {
            hubConnection = new HubConnectionBuilder()
                .WithUrl("http://localhost:42069/ws/sessionhub")
                .WithAutomaticReconnect()
                .Build();

            ConfigureConnection(hubConnection);

            hubConnection.StartAsync()
                .GetAwaiter()
                .GetResult();
        }

        private void ConfigureConnection(HubConnection hubConnection)
        {
            hubConnection.On<ChatMessage>("ReceiveMessage",
                eventArgs => ReceiveMessage?.Invoke(this, eventArgs));
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            hubConnection.StopAsync()
                .GetAwaiter()
                .GetResult();
        }
    }
}
