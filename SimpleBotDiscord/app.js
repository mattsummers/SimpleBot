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
        console.error(err);
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