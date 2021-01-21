/* chat functionality */
$(document).ready(function () {
    const messages = $('#messagelist')

    const connection = new signalR.HubConnectionBuilder()
        .withUrl('/app/chathub', {
            accessTokenFactory: function () {
                return window.sessionStorage.getItem('spc_user_token')
            }
        })
        .withAutomaticReconnect()
        .build();

    connection.on('ReceiveMessage', function (chatMessage) {
        const html = `<li>${chatMessage.message}</li>`
        messages.append(html);
    });

    $('#messaging').on('submit', function (e) {
        e.preventDefault();
        const message = $('#myMessage');
        if (message.text().length > 0) {
            /* Flush 0.5
             * Messages are posted to server.
             * Responses are received over sockets.
             */
            $.post(`https://${window.location.hostname}/api/v2/chat`, {
                Message: message.text()
            }, function (data) {
                console.log(data)
            });
        }
    });

    connection.start().catch(console.error)
});
