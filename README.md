# LudoUnityNodejsSocket.io
Game ready project

HOW TO SETUP THIS PROJECT

INSALLATION:
1: Nodejs
2: Unity3d

Following are the steps to run the project:

1: Run the Server.js file inside ludoNodeServer using "node server.js" cmd.
2: Import the unity project in unity.
3: Open the Game scene.
4: hit run.

NOTE:PASS AND PLAY OPTION IS NOT AVAILABLE YOU CAN EITHER PLAY IT ONLINE BY HOSTING THE NODE SERVER PROJECT
ON A SERVER OF YOUR CHOICE LIKE HEROKU, AWS, AZUR, GOOGLE CLOUD. AND FIREBASE WILL NOT WORK. YOU CAN ALSO PLAY IT ON LAN.
IF SOMETHING WENT WRONG CHECK THE PORT NO. IN UNITY SocketIO PERFAB INSIDE HIERARCHY AND IN server.js FILE IT SHOULD BE SAME.

Nodejs Modules list:
express
socketio
shortid
nodemon
debug

Note: if your running the server on your local machine and debuging statment will not appear setup the npm node module by hiting the following command on:
for Windows:set DEBUG="test"
for MacOs and Linux : DEBUG=test

