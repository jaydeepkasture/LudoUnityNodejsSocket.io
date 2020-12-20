"use strict";

var express = require('express');

var path = require('path');

var router = express.Router();

var _require = require('os'),
    type = _require.type;

var app = require('express')();

var http = require('http').createServer(app);

var io = require('socket.io')(http);

var bodyParser = require('body-parser');

var debug = require('debug')('test');

var PORT = process.env.PORT || 3000;

var startGame = require("./GameLogic");

var userInfo = require("./Users"); //------------------------------------------------------------------------------


var user = new userInfo();
var tempTwoUserQ = new Array();
var tempThreeUserQ = new Array();
var tempFourUserQ = new Array(); //------------------------------------------------------------------------------

app.get('/', function (req, res) {
  res.sendFile(path.join(__dirname + '/index.html'));
});
var onlineUserQ = new Array();
io.on('connection', function (socket) {
  debug('a user connected ' + socket.id);
  socket.on("userId", function (data) {
    if (data["userId"] == "null") {
      var id = {
        id: "Guest" + shortId.generate()
      };
      debug("new user");
      socket.emit("registerUserId", id);
      onlineUserQ.push(id.id);
    } else {
      debug("old user");
      onlineUserQ.push(data["userId"]); //push userId to DB
    }
  });
  socket.on("test", function (data) {
    debug("hey its test");
  });
  socket.on("quit", function (data) {
    debug("user ".concat(socket.id, " quit"));
    debug("userpawntype ".concat(data["players"], " quit"));
    var index = onlineUserQ.indexOf(data["userId"]);

    if (index > -1) {
      onlineUserQ.splice(index, 1);
    }

    var isInTempQ = false;

    switch (data["players"]) {
      case 0:
        return;

      case 2:
        isInTempQ = RemoveUserFromTempOnlineQ(socket.id, tempTwoUserQ);
        break;

      case 3:
        isInTempQ = RemoveUserFromTempOnlineQ(socket.id, tempThreeUserQ);
        break;

      case 4:
        isInTempQ = RemoveUserFromTempOnlineQ(socket.id, tempFourUserQ);
        break;

      default:
        break;
    }

    if (!isInTempQ) {
      user.removeUser(socket.id);
    }
  });
  socket.on("showData", function (data) {
    debug("tempTwoUserQLenght " + tempTwoUserQ.length);
    debug("tempOnlineQLength " + onlineUserQ.length);
    user.showUsers();
  });
  socket.on("joinRoom", function (data) {
    var newUserDetail;

    switch (data["players"]) {
      case 2:
        newUserDetail = {
          socket: socket,
          pawnType: data["pawnType"],
          userId: data["userId"]
        };
        tempTwoUserQ.push(newUserDetail);
        MatchMakingWhenTwoUserArePlaying(tempTwoUserQ, newUserDetail);
        break;

      case 3:
        newUserDetail = {
          socket: socket,
          pawnType: data["pawnType"],
          userId: data["userId"]
        };
        tempThreeUserQ.push(newUserDetail);
        MatchMakingWhenThreeUserArePlaying(tempTwoUserQ, newUserDetail);
        break;

      case 4:
        tempFourUserQ.push({
          socket: socket,
          pawnType: data["pawnType"],
          userId: data["userId"]
        });
        RoomChecking(4, tempTwoUserQ);
        break;

      default:
        break;
    }
  });
  socket.on('disconnect', function () {
    debug("disconnected " + socket.id);
  });
});

function RemoveUserFromTempOnlineQ(socketId, tempQ) {
  for (var i = 0; i < tempQ.length; i++) {
    var id = tempQ[i].socket.id;

    if (socketId === id) {
      tempQ.splice(i, 1);
      return true;
    }

    return false;
  }
}

function RoomChecking(noOfUserInASingleRoom, tempUserQ) {
  switch (noOfUserInASingleRoom) {
    case 2:
      if (tempUserQ.length >= 2) CreateRoom(tempUserQ, 2);
      break;

    case 3:
      if (tempUserQ.length >= 3) CreateRoom(tempUserQ, 3);
      break;

    case 4:
      if (tempUserQ.length >= 4) CreateRoom(tempUserQ, 4);
      break;

    default:
      break;
  }
}

function MatchMakingWhenTwoUserArePlaying(tempQ, newUser) {
  var currentPawnType = newUser.pawnType;
  var opponentPawnType = 0;

  if (currentPawnType % 2 === 0) {
    if (currentPawnType === 2) {
      opponentPawnType = 4;
    } else {
      opponentPawnType = 2;
    }
  } else {
    if (currentPawnType === 3) {
      opponentPawnType = 1;
    } else {
      opponentPawnType = 3;
    }
  }

  if (opponentPawnType === 0) {
    newUser.socket.emit("tostmsg", {
      msg: "Something went wrong, try again"
    });
    return;
  }

  var usersJoiningTheRoom = [];
  usersJoiningTheRoom.push(newUser);

  for (var i = 0; i < tempQ.length; i++) {
    if (tempQ[i].socket.id === newUser.socket.id) {
      continue;
    }

    if (tempQ[i].pawnType === opponentPawnType) {
      usersJoiningTheRoom.push(tempQ[i]);
      RefactorTempQ(usersJoiningTheRoom, tempQ).then(function (data) {
        CreateRoom(usersJoiningTheRoom, 2);
        debug("#user left in q " + tempQ.length);
      });
      return;
    }
  }
}

function MatchMakingWhenThreeUserArePlaying(tempQ, newUser) {
  if (tempQ.length < 2) return;
  var finalUserQToCreateARoom = [];
  finalUserQToCreateARoom.push(newUser);

  for (var i = 0; i < tempQ.length; i++) {
    if (finalUserQToCreateARoom.length === 3) {
      RefactorTempQ(finalUserQToCreateARoom, tempQ).then(function (data) {
        CreateRoom(finalUserQToCreateARoom, 3);
        debug("#user left in q " + tempQ.length);
      });
      break;
    }

    if (tempQ[i].userId === newUser.userId) continue;

    for (var j = 0; j < finalUserQToCreateARoom.length; j++) {
      var avoidSamePawnType = finalUserQToCreateARoom[j].pawnType === tempQ[i].pawnType;
      if (avoidSamePawnType) continue;
      finalUserQToCreateARoom.push(tempQ[i]);
    }
  }
}

function RefactorTempQ(users, tempQ) {
  return new Promise(function (resolve, reject) {
    for (var usersIndex = 0; usersIndex < users.length; usersIndex++) {
      for (var tempQIndex = 0; tempQIndex < tempQ.length; tempQIndex++) {
        if (users.length === 0) break;

        if (tempQ[tempQIndex].socket.id === users[usersIndex].socket.id) {
          debug("user removed from the tempQ :" + tempQ[tempQIndex].socket.id);
          tempQ.splice(tempQIndex, 1);
        }
      }

      if (users.length === 0) break;
    }

    resolve("done");
  });
} //----------------------------------------------------------------------------


var shortId = require('shortid');

var _require2 = require('fs'),
    promises = _require2.promises;

function CreateRoom(tempUserQ, roomLimit) {
  var players = roomLimit;
  debug("player:" + players);
  var roomName = shortId.generate();
  JoinRoom(tempUserQ, roomLimit, roomName).then(function (data) {
    startGame(data["userSocketObj"], data["roomName"], io);
  });
}

function JoinRoom(tempUserQ, players, roomName) {
  var userSocketObj = [];
  return new Promise(function (resolve, reject) {
    var _loop = function _loop(i) {
      user.addUser(tempUserQ[i].socket, roomName, tempUserQ[i].pawnType, tempUserQ[i].userId);
      userSocketObj.push({
        socket: tempUserQ[i].socket,
        pawnType: tempUserQ[i].pawnType
      });
      tempUserQ[i].socket.join(roomName, function () {
        debug("user ".concat(tempUserQ[i].socket.id, " joined room ").concat(roomName, " of pawnType ").concat(tempUserQ[i].pawnType));
      });
    };

    for (var i = 0; i < players; i++) {
      _loop(i);
    }

    resolve({
      userSocketObj: userSocketObj.concat(),
      roomName: roomName
    });
  });
} //----------------------------------------------------------------------------


function findClientsSocket(roomId, namespace) {
  var res = [] // the default namespace is "/"
  ,
      ns = io.of(namespace || "/");

  if (ns) {
    for (var id in ns.connected) {
      if (roomId) {
        var index = ns.connected[id].rooms.indexOf(roomId);

        if (index !== -1) {
          res.push(ns.connected[id]);
        }
      } else {
        res.push(ns.connected[id]);
      }
    }
  }

  return res;
}

function getAllRoomMembers(room, _nsp) {
  var roomMembers = [];
  var nsp = typeof _nsp !== 'string' ? '/' : _nsp;

  for (var member in io.nsps[nsp].adapter.rooms[room]) {
    roomMembers.push(member);
  }

  return roomMembers;
}

http.listen(PORT, function () {
  debug('listening on ' + PORT);
});