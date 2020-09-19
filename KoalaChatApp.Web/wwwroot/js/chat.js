"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/koalachat").build();

document.getElementById("sendMessage").disabled = true;

connection.on(document.getElementById("chatRoomId").value, function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = user + ": " + msg;
    var div = document.createElement("div");
    div.textContent = encodedMsg;
    div.className = "alert alert-info col-md-12";
    document.getElementById("chatMessages").appendChild(div);
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