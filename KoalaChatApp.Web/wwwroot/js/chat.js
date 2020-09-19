"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/koalachat").build();

document.getElementById("sendMessage").disabled = true;

connection.on(document.getElementById("chatRoomId").value, function (user, date, message) {
    var chatMessagesCount = document.getElementById("chatMessagesCount").value;
    var chatMessagesCountLimit = document.getElementById("chatMessagesCountLimit").value;
    if (chatMessagesCount > chatMessagesCountLimit) {
        document.getElementById("chatMessages").removeChild(document.getElementById("chatMessages").childNodes[0]);
    }
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = "<strong>" + user + ":</strong> " + msg + "<span class='float-right'>" + date + "</span>";
    //encodedMsg = encodedMsg.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var div = document.createElement("div");
    div.innerHTML = encodedMsg;
    div.className = "alert alert-info col-md-12";
    var chatMessages = document.getElementById("chatMessages");
    chatMessages.appendChild(div);
    chatMessages.scrollTop = chatMessages.scrollHeight;
});

connection.start().then(function () {
    document.getElementById("sendMessage").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendMessage").addEventListener("click", function (event) {
    var message = document.getElementById("message").value;
    document.getElementById("message").value = "";
    var chatRoomId = document.getElementById("chatRoomId").value;
    connection.invoke("SendMessage", chatRoomId, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

document.getElementById("clearMessage").addEventListener("click", function (event) {
    document.getElementById("message").value = "";
});