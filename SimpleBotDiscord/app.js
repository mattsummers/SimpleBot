'use strict';

const discord = require('discord.js');
const SimpleBot = require('./bot');

var constants;
if (process.env.NODE_ENV === "development") {
    constants = require('./config/constants.development.js');
} else {
    constants = require('./config/constants.js');
}

// Create an instance of a Discord client
const client = new discord.Client();

// Don't crash the entire app if there's a minor error. Or even a major one.
client.on('error', (err) => {
    console.error(currentDate.toLocaleDateString() + " " + err);
});

client.on("disconnect", () => {
    console.log(currentDate.toLocaleDateString() + " disconnected from Discord server");
    process.exit(1);
});

client.on("reconnecting", () => {
    console.log(currentDate.toLocaleDateString() + " reconnecting");
    process.exit(1);
});

client.on('shardError', error => {
    console.error(currentDate.toLocaleDateString() + " shardError " + error);
    process.exit(1);
});

// Only initialize once the client is fully connected to Discord
client.on('ready', () => {
    const bot = new SimpleBot(constants, client);
    console.log('Ready');
    // Create an event listener for messages
    client.on('message', message => {
        bot.reactToMessage(message);
    });
});

// Log our bot in
client.login(constants.token).then(function() { console.log('Login completed');});