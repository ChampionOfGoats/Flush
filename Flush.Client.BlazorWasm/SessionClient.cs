using System;
using Flush.Models;
using Microsoft.AspNetCore.SignalR.Client;

namespace Flush.Client.BlazorWasm
{
    public class SessionClient : IDisposable
    {
        public event EventHandler<ReceiveSessionResponse> ReceiveSession;
        public event EventHandler<PlayerConnectedResponse> PlayerConnected;
        public event EventHandler<PlayerDisconnectedResponse> PlayerDisconnected;
        public event EventHandler<PlayerRemovedResponse> PlayerRemoved;
        public event EventHandler<ReceiveVoteResponse> ReceiveVote;
        public event EventHandler<TransitionResponse> Transition;
        public event EventHandler<RoleChangedResponse> RoleChanged;

        private readonly HubConnection hubConnection;

        public SessionClient()
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
            hubConnection.On<ReceiveSessionResponse>("ReceiveSession",
                eventArgs => ReceiveSession?.Invoke(this, eventArgs));
            hubConnection.On<PlayerConnectedResponse>("PlayerConnected",
                eventArgs => PlayerConnected?.Invoke(this, eventArgs));
            hubConnection.On<PlayerDisconnectedResponse>("PlayerDisconnected",
                eventArgs => PlayerDisconnected?.Invoke(this, eventArgs));
            hubConnection.On<PlayerRemovedResponse>("PlayerRemoved",
                eventArgs => PlayerRemoved?.Invoke(this, eventArgs));
            hubConnection.On<ReceiveVoteResponse>("ReceiveVote",
                eventArgs => ReceiveVote?.Invoke(this, eventArgs));
            hubConnection.On<TransitionResponse>("Transition",
                eventArgs => Transition?.Invoke(this, eventArgs));
            hubConnection.On<RoleChangedResponse>("RoleChanged",
                eventArgs => RoleChanged?.Invoke(this, eventArgs));
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
