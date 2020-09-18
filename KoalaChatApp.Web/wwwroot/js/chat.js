"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/koalachat").build();

document.getElementById("sendMessage").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = user + ": " + msg;
    var li = document.createElement("div");
    li.textContent = encodedMsg;
    li.className = "alert alert-light";
    document.getElementById("chatMessages").appendChild(li);
});

connection.start().then(function () {
    document.getElementById("sendMessage").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendMessage").addEventListener("click", function (event) {
    var message = document.getElementById("message").value;
    connection.invoke("SendMessage", message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

document.getElementById("clearMessage").addEventListener("click", function (event) {
    document.getElementById("message").value = "";
});