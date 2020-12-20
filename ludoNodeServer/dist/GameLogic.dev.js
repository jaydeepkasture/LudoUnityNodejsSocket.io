"use strict";

var debug = require('debug')('test');

var startGame = function startGame(pawnsDetails, roomName, io) {
  var pawns = [];

  for (var i = 0; i < pawnsDetails.length; i++) {
    pawns.push(pawnsDetails[i].pawnType);
  }

  pawns.sort();
  debug("got roomName :" + roomName + " and pawntype" + pawns[0]);
  var obj = {
    pawnType: pawns[0]
  };

  for (var _i = 0; _i < pawnsDetails.length; _i++) {
    debug("obj :" + obj);

    pawnsDetails[_i].socket.emit("startGame", obj);

    socketEvent(pawnsDetails[_i].socket, obj.pawnType, roomName, io, pawns);
  }
};

module.exports = startGame;

function socketEvent(socket, currentPawn, roomName, io, pawns) {
  socket.on("gameStarted", function () {
    debug("game started at " + socket.id);
  });
  socket.on("rollDice", function (data) {
    var diceValue = data["diceValue"];
    socket.to(roomName).emit("rollDice", {
      diceValue: diceValue
    });
    debug("rollDice game started at " + socket.id);
  });
  socket.on("pawnFinishedMoving", function (data) {
    var pawnType = data["pawnType"];
    var diceValue = data["diceValue"];
    var nextPawn = 0;
    debug("pawntype " + data["pawnType"] + "dicevalue :" + data["diceValue"] + " ,is win " + data["richedTheDestination"]);

    if (diceValue === 6 || data["richedTheDestination"]) {
      debug("diceValue: ".concat(diceValue, " so dont switch pawns"));
      nextPawn = data["pawnType"];
      socket.to(roomName).emit("pawnFinishedMoving", {
        nextPawn: nextPawn,
        diceValue: diceValue
      });
      socket.emit("pawnFinishedMoving", {
        nextPawn: nextPawn,
        diceValue: diceValue
      });
    } else {
      debug("diceValue: ".concat(diceValue, " pawnType :").concat(pawnType, " switch the god damn pawn"));
      nextPawn = SwitchPawns(data["pawnType"], pawns);
      var tempObj = {
        nextPawn: nextPawn,
        diceValue: diceValue
      };
      socket.to(roomName).emit("pawnFinishedMoving", tempObj);
      socket.emit("pawnFinishedMoving", tempObj);
      debug("----nextPawn :".concat(nextPawn, "----diceValue-").concat(diceValue));
    }

    debug("pawnFinishedMoving pawnType" + pawnType + " diceValue" + diceValue);
  });
  socket.on("switchPawns", function (data) {
    debug("Switch pawns pawnType" + data["pawn"]);
    debug("dat " + JSON.stringify(data));
    var nextPawn = SwitchPawns(data["pawn"], pawns);
    var diceValue = data["diceValue"];
    debug("next pawn " + nextPawn);
    socket.to(roomName).emit("switchPawns", {
      nextPawn: nextPawn,
      diceValue: diceValue
    });
    socket.emit("switchPawns", {
      nextPawn: nextPawn,
      diceValue: diceValue
    });
    debug("game started at " + socket.id);
  });
  socket.on("avoidSwitchingPawns", function (data) {
    debug("avoidSwitchingPawns " + JSON.stringify(data));
    var diceValue = data["diceValue"];
    socket.to(roomName).emit("avoidSwitchingPawns", {
      diceValue: diceValue
    });
    debug("game started at " + socket.id);
  });
  socket.on("movePawn", function (data) {
    var pawnNo = data["pawnNo"];
    var diceValue = data["diceValue"];
    var pawnType = data["pawnType"];
    debug("value " + JSON.stringify(data));
    debug("pawn ".concat(pawnNo, " move ").concat(diceValue, " of type ").concat(pawnType));
    socket.to(roomName).emit("pawnMoved", {
      pawnNo: pawnNo,
      diceValue: diceValue,
      pawnType: pawnType
    });
  });
  socket.on("exitRoom", function (data) {
    debug("exit room trigger");
    var pawnType = data["pawnType"];
    var players = data["players"];
    debug("player Exit pawnType ".concat(pawnType));
    socket.leave(roomName);
    var remainingUsers = GetUsersIdInARoom(roomName, io);
    if (remainingUsers === null) return;
    debug("remaining users-");

    if (remainingUsers.length === 1) {
      remainingUsers[0].emit("youWin");
    }

    socket.to(roomName).emit("exitRoom", {
      pawnType: pawnType
    });
  });
}

function SwitchPawns(currentPawnType, pawns) {
  var nextPawn = 0;

  for (var i = 0; i < pawns.length; i++) {
    if (pawns[i] === currentPawnType) {
      if (++i < pawns.length) {
        nextPawn = pawns[i];
      } else {
        nextPawn = pawns[0];
      }
    }
  }

  return nextPawn;
}

var GetUsersIdInARoom = function GetUsersIdInARoom(roomName, io) {
  try {
    var clients = io.sockets.adapter.rooms[roomName].sockets;
    var clientSocket = []; //to get the number of clients

    var numClients = typeof clients !== 'undefined' ? Object.keys(clients).length : 0;
    if (numClients === 0) return null;

    for (var clientId in clients) {
      //this is the socket of each client in the room.
      clientSocket.push(io.sockets.connected[clientId]);
      debug("clients " + clientSocket.id + " in " + roomName);
    }

    return clientSocket;
  } catch (err) {
    console.log(err);
    return null;
  }
};