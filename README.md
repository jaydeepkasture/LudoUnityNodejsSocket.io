# LudoUnityNodejsSocket.io
Game ready project

HOW TO SETUP THIS PROJECT

INSALLATION:
1: Nodejs : https://nodejs.org/en/
2: Unity3d : https://unity3d.com/get-unity/download

Following are the steps to run the project:

1: Run the Server.js file inside ludoNodeServer using "node server.js" cmd.</br>
2: Import the unity project in unity.</br>
3: Open the Game scene.</br>
4: hit run.</br>

NOTE:PASS AND PLAY OPTION IS NOT AVAILABLE YOU CAN EITHER PLAY IT ONLINE BY HOSTING THE NODE SERVER PROJECT
ON A SERVER OF YOUR CHOICE LIKE HEROKU, AWS, AZUR, GOOGLE CLOUD. YOU CAN ALSO PLAY IT ON LAN.
IF SOMETHING WENT WRONG CHECK THE PORT NO. IN UNITY SocketIO PERFAB INSIDE HIERARCHY AND IN server.js FILE IT SHOULD BE SAME.

NOTE:FIREBASE WILL NOT WORK

Nodejs Modules list:
express : https://www.npmjs.com/package/express</br>
socketio : https://www.npmjs.com/package/socket.io</br>
shortid : https://www.npmjs.com/package/shortid</br>
nodemon : https://www.npmjs.com/package/express</br>
debug : https://www.npmjs.com/package/debug</br>

Note: if your running the server on your local machine and debuging statment will not appear setup the npm node module by hiting the following command

Windows:set DEBUG="test"</br>
     
MacOs and Linux : DEBUG=test</br>

