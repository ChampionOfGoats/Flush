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

    connection.on('ReceiveMessage', function (sendMessageResponse) {
        const html = `<li>${sendMessageResponse.message}</li>`
        messages.append(html);
    });

    $('#messaging').on('submit', function (e) {
        e.preventDefault();
        const message = $('#myMessage');
        if (message.text().length > 0) {
            connection.invoke("SendMessage", {
                Message: message.text()
            }).then(function () {
                const html = `<li class='from-me'>${message.text()}</li>`
                messages.append(html);
                message.text('');
            }).catch(console.error);
        }
    });

    connection.start().catch(console.error)
});
