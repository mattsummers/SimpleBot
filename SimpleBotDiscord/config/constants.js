module.exports = {
    // The token of your bot - https://discordapp.com/developers/applications/me
    token: 'xxxxxxxx',

    // Default server
    defaultGuild: 'xxxx',

    // Default room, used in cases where a command initiates an action that isn't in reply to a particular message from a user
    defaultRoom: 'general',

    // Rooms that the bot will listen to for messages. Messages that come in from other rooms will be ignored
    rooms: ['bot-test', 'general'],

    // The web interface, where chat data is stored
    baseUrl = "http://localhost:62551/",

    // The bot can remember recent responses and avoid duplicating them. This sets how far back it should remember a response before allowing it to recycle
    maximumRecentReplies: 50,

    // The recent responses are stored in a file that can persists across app restarts
    recentRepliesFilename: 'recentreplies.json'
}