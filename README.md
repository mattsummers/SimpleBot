# SimpleBot

## Website
The web app does not currently create the database for you. Manual action is required.

Create a database and user for the site. MySQL / MariaDB are tested working.

Execute the included /database.sql script

Edit the user's table and set the administrator account's email to yourself

Edit the config file "appsettings.json" and set your database parameters, email relay, and optionally the base URL.

Complile the app

    cd SimpleBotWeb
    dotnet publish -o ../publish -c Release

Start the app

    cd ../publish
    dotnet SimpleBotWeb.dll

Reset your password to log in for the first time. The username is 'admin'

The site can be made persistent and listen at a proper URL through a variety of means. [Linux](https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/linux-nginx?view=aspnetcore-3.1) [Windows](https://docs.microsoft.com/en-us/aspnet/core/tutorials/publish-to-iis?view=aspnetcore-3.1&tabs=visual-studio) 

To rebuild the CSS & JS bundles if you've made changes

    npm install (One time only, if you don't already have gulp installed globally)
    gulp

## Discord client

Install Node.js

Inside the /SimpleBotDiscord folder

    npm install

[Create a Discord bot](https://discordpy.readthedocs.io/en/latest/discord.html)

Follow the instructions to join the bot to your channel

Update /SimpleBotDiscord/constants.js with your bot's token and other data.

    node app.js
    
The bot can be made persistent with a utility like [PM2](!https://pm2.keymetrics.io/)

    pm2 start app.js
    pm2 startup
    pm2 save

## Troubleshooting
Google the error messages. Fiddle with stuff until it's not broken. Kick the server until it works. 
