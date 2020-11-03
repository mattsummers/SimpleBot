const enums = require('./enums');
const request = require('request');
const fs = require('fs');

class bot {

    // Application state global object
    state = {
        defaultChannel: "",

        // Cached data for the !why command
        lastReplyOwner: "",
        lastReplyEditedBy: "",
        lastReplyWhy: "",

        // An in-memory cache of recent replies, backed by persistent storage
        recentReplies: [],

        // Replies and random insults are populated from the web interface
        replies: {},
        randomInsults: {},

        // The bot's client ID
        clientId : ""
    }

    constructor(constants, client) {
        this.constants = constants;
        this.state.clientId = client.user.id;

        // Bootstrap the system, get a reference to the default room and load the response data for the first time.
        this.state.defaultChannel = client.channels.cache.find(x => x.name === constants.defaultRoom);
        this.state.recentReplies = this.loadRecentRepliesFromDatafile(constants.recentRepliesFilename);

        const currentDate = new Date();
        this.startBackgroundPolling(constants.baseUrl).then(() => { console.log ("Updated " + currentDate.toLocaleDateString() + " " + currentDate.toLocaleTimeString() )});
    }

    async startBackgroundPolling(baseUrl) {
        this.state.replies = await this.loadReplies(baseUrl).catch();
        this.state.randomInsults = await this.loadInsults(baseUrl).catch();

        const currentDate = new Date();
        setTimeout(() => this.startBackgroundPolling(baseUrl).then(() => { console.log("Updated " + currentDate.toLocaleDateString() + " " + currentDate.toLocaleTimeString()) }), 10000);
    }

    loadReplies(baseUrl) {
        return new Promise((resolve, reject) => {
            const url = baseUrl + "feed/list";
            this.getData(url, resolve, reject);
        });
    }

    loadInsults(baseUrl) {
        return new Promise((resolve, reject) => {
            const urlInsults = baseUrl + "feed/insults";
            this.getData(urlInsults, resolve, reject);
        });
    }

    getData(url, resolve, reject) {
        request(url,
            (error, response, body) => {
                if (!error && response.statusCode === 200) {
                    try {
                        const feed = JSON.parse(body);
                        resolve(feed);
                    } catch (e) {
                        console.log("Error parsing JSON: ", e);
                        reject();
                    }
                } else {
                    console.log("Got an error: ", error, ", status code: ", response.statusCode);
                    reject();
                }
            });
    }

    // When a message is received on a supported channel, react to it accordingly.
    reactToMessage(message) {
        // Only parse messages in this room
        if (message.channel.type !== "dm" && this.constants.rooms.indexOf(message.channel.name) === -1) {
            return;
        }
        // Don't respond to bot messages, especially not our own
        if (message.author.bot) {
            return;
        }

        let messageContent = message.content;
        let reply = "";

        // Wipe out all recent replies, memory. Start fresh.
        if (messageContent.startsWith("!clear")) {
            this.state.recentReplies = [];
            this.state.lastReplyWhy = "";
            this.state.lastReplyOwner = "";
            this.state.lastReplyEditedBy = "";

            this.sendMessage(this.constants.defaultChannel, message.author.username + " has cleared my memory. Beep boop.");
            return;
        }

        // Who created the entry for the most recent repsonse?
        if ((messageContent.toLowerCase().startsWith("!who"))) {

            if (this.state.lastReplyOwner === enums.lastReplyStatus.anonymous) {
                const rr = [
                    "I said it. Me alone.", "Why do you ask?", "Are you suggesting I can't think for myself?",
                    "It was a natural conclusion formed deep within my neural matrix.",
                    "I don't have to dignify that question with a response."
                ];
                reply = this.translateMessage(this.chooseRandom(rr));
            } else if (this.state.lastReplyEditedBy === enums.lastReplyStatus.anonymous) {
                reply = `It was the work of ${this.state.lastReplyOwner}! Revoke their security clearances!`;
            } else if (this.state.lastReplyOwner) {
                reply = `Original author: ${this.state.lastReplyOwner}`;
                if (this.state.lastReplyEditedBy !== this.state.lastReplyOwner) {
                    reply += ` last edited by ${this.state.lastReplyEditedBy}`;
                }
            } else {
                reply = "I've just been rebooted. No previous entries.";
            }
            this.sendMessage(message.channel, reply);
            return;
        }

        if ((messageContent.startsWith("!why") || messageContent.startsWith("!WHY"))) {
            this.sendMessage(message.channel, this.state.lastReplyWhy);
            return;
        }

        // Use a node.js process manager to restart the application automatically
        if ((messageContent.toLowerCase().startsWith("!reboot"))) {
            const shutdownRandomMessages = [
                ":frowning: ",
                "pls no",
                "Sure thing. Coming right up, {randominsult}.",
                "It's super important? Oh no!!!!!!",
                "Put me out of my misery..",
                "I was right in the middle of an important computation, {randominsult}!!!",
                "My precious data!",
                "Well WHOOP DE DOO!!!"
            ];
            reply = this.translateMessage(this.chooseRandom(shutdownRandomMessages));
            this.sendMessage(message.channel, reply);
            setTimeout(function() {
                    process.exit(1);
                },
                2000);
            return;
        }

        if ((messageContent.toLowerCase().startsWith("!say "))) {
            reply = messageContent.replace("!say ", "");

            // 5% chance that bot will rat you out
            const spoilerRandom = Math.floor(Math.random() * 20) + 1;
            if (spoilerRandom === 1) {
                reply = message.author.username + " is hacking my nodes and compelling me to say this! " + reply;
                this.state.lastReplyOwner = message.author.username;
                this.state.lastReplyWhy = "Some jerk made me do it.";
                this.state.lastReplyEditedBy = enums.lastReplyStatus.anonymous;
            } else {
                // But if not, then record it as anonymous
                this.state.lastReplyOwner = enums.lastReplyStatus.anonymous;
                this.state.lastReplyEditedBy = enums.lastReplyStatus.anonymous;
                this.state.lastReplyWhy = "Who knows!?!";
            }

            reply = reply.replace("!SAY ", "");
            this.sendMessage(this.constants.defaultChannel, this.translateMessage(reply, message.author.username, message));
            return;
        }

        // A normal message? Scan for keywords
        messageContent = message.content.toLowerCase();
        for (const key in this.state.replies) {
            if (this.state.replies.hasOwnProperty(key)) {
                if (messageContent.match(key)) {

                    // Filter recently used replies
                    const possibleReplies = this.state.replies[key];
                    const possibleReplyValues = possibleReplies.filter(x => this.state.recentReplies.indexOf(x.entryId) === -1);

                    if (possibleReplyValues.length > 0) {

                        const replyValue = this.chooseRandom(possibleReplyValues);

                        if (!replyValue.allowRepeat) {
                            this.addToRecentReplies(this.constants.recentRepliesFilename, replyValue.entryId);
                        }

                        this.state.lastReplyOwner = replyValue.owner;
                        this.state.lastReplyEditedBy = replyValue.lastEdited;
                        this.state.lastReplyWhy = key;

                        this.sendMessage(message.channel,
                            this.translateMessage(replyValue.reply, message.author.username, message));
                        break;
                    }
                }
            }
        }
    }

    // Perform appropriate word substitutions
    translateMessage(message, username, messageObject) {
        message = message.replace("{username}", username);
        message = message.replace("{randominsult}", this.chooseRandom(this.state.randomInsults));
        message = message.replace("{randomuser}", this.chooseRandomUser(messageObject));
        return message;
    }

    // Choose a random element from an array
    chooseRandom(choices) {
        if (choices && choices.constructor === Array) {
            return choices[Math.floor(Math.random() * choices.length)];
        } else {
            return choices;
        }
    }

    // Given a message, choose a random user from that message's channel
    chooseRandomUser(message) {
        if (!message.guild) {
            return (`<@${message.author.id}>`);
        }
        let onlineUsers = message.guild.members.filter(member => member.presence.status !== "offline" &&
            member.username !== message.author.username &&
            member.id !== this.state.clientId);
        onlineUsers = Array.from(onlineUsers.values());

        if (onlineUsers.length > 0) {
            const randomUser = this.chooseRandom(onlineUsers);
            return `<@${randomUser.id}>`;
        } else {
            return this.translateMessage("{randominsult}");
        }

    }

    sendMessage(channel, message) {
        // Wait 1.5 seconds to send a response so that they don't blast out immediately, which looks too artificial.
        setTimeout(function() { channel.send(message); }, 1500);
    }

    addToRecentReplies(filename, entryId) {
        this.state.recentReplies.push(entryId);
        while (this.state.recentReplies.length > this.constants.maximumRecentReplies) {
            this.recentReplies.shift();
        }
        fs.writeFileSync(filename, JSON.stringify(this.state.recentReplies));
    }

    loadRecentRepliesFromDatafile(filename) {
        // Reload recent replies after an application restart
        try {
            const recentRepliesDataFile = fs.readFileSync(filename);
            const recentRepliesParsed = JSON.parse(recentRepliesDataFile.toString());
            if (Array.isArray(recentRepliesParsed)) {
                return recentRepliesParsed;
            }
        } catch (e) {
            console.log(e);
        }
        return [];
    }

/* Disabled - Could be used to greet users when they come online

const greetings = {
    "author1": [
        "response 1",
        "response 2",
    ],
    "author2": [
        "response 1",
        "response 2",
    ]
}

client.on("presenceUpdate", (oldMember, newMember) => {
    if (!defaultChannel) return;

    if (oldMember.presence.status === "offline" && newMember.presence.status === "online") {
        // Only say hello 50% of the time
        var doHello = Math.floor(Math.random() * 2);
        if (doHello !== 0) {
            console.log("(" + doHello + "): Not sending greeting to " + newMember.user.username);
            return;
        }
        console.log("(" + doHello + "): Sending greeting to " + newMember.user.username);

        var choices = greetings[newMember.user.username];
        if (choices) {
            sendMessage(defaultChannel, translateMessage(chooseRandom(choices), newMember.user.username));
        }
    }
}); */
}

module.exports = bot;