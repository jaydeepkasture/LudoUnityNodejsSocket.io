"use strict";
const debug = require("debug")("test");
const user = require("../Users");
const gamePlayApi = require("./ClientApi").gamePlayApi;

let startGame = function (data) {

  let playersData = data.userSocketObj,
    roomName = data.roomName,
    allPlayersId = data.allPlayersId,
    socketDictionary = data.socketDictionary,
    allProfiles = data.profiles;

  debug("got roomName :" + roomName + " and playerId" + allPlayersId[0]);
  let obj = {
    allPlayersId: allPlayersId,
    allProfiles: allProfiles,
  };

  for (let i = 0; i < playersData.length; i++) {
    playersData[i].socket.emit(gamePlayApi.START_GAME, obj);

    let createUserObj = {
      socket: playersData[i].socket,
      roomName: roomName,
      allPlayersId: allPlayersId,
      socketDictonary:socketDictionary,
    };
    socketEvent(createUserObj);
  }
}

module.exports = startGame;

function socketEvent(User) {
  let socket = User.socket,
    roomName = User.roomName,
    allPlayersId = User.allPlayersId,
    socketDictionary = User.socketDictionary,
    thisPlayerId = null;

    debug("dictionary");
    debug(socketDictionary);
  socket.on(gamePlayApi.ON_GAME_STARTED, (data) => {
    debug("game started at " + data.playerId);
    thisPlayerId = data.playerId;
  });

  socket.on(gamePlayApi.ROLL_DICE, (data) => {
    let diceValue = data["diceValue"];
    socket.to(roomName).emit(gamePlayApi.ROLL_DICE, { diceValue: diceValue });
    debug("rollDice game started at " + socket.id);
  });

  socket.on(gamePlayApi.PLAYER_FINISHED_MOVING, (data) => {
    let playerId = data["playerId"];
    let diceValue = data["diceValue"];
    let nextPlayerId = 0;

    debug(
      "player " +
      playerId +
      "dicevalue :" +
      data["diceValue"] +
      "is win" +
      data["richedTheDestination"]
    );

    if (diceValue === 6 || data["richedTheDestination"]) {
      debug(`diceValue: ${diceValue} so dont switch pawns`);
      nextPlayerId = data["playerId"];
      socket.to(roomName).emit(gamePlayApi.PLAYER_FINISHED_MOVING, {
        nextPlayerId: nextPlayerId,
        diceValue: diceValue,
      });

      socket.emit(gamePlayApi.PLAYER_FINISHED_MOVING, {
        nextPlayerId: nextPlayerId,
        diceValue: diceValue,
      });
    } else {
      debug(
        `diceValue: ${diceValue} pawnId :${playerId} switch the god damn player`
      );

      nextPlayerId = GetNextPlayer(data["playerId"], allPlayersId);
      let tempObj = { nextPlayerId: nextPlayerId, diceValue: diceValue };
      socket.to(roomName).emit(gamePlayApi.PLAYER_FINISHED_MOVING, tempObj);
      socket.emit(gamePlayApi.PLAYER_FINISHED_MOVING, tempObj);
      debug(`----nextPawn :${nextPlayerId}----diceValue-${diceValue}`);
    }
    debug(
      "playerFinishedMoving playerId" + playerId + " diceValue" + diceValue
    );
  });

  socket.on(gamePlayApi.SWITCH_PLAYER, (data) => {
    debug("Switch player " + data["playerId"]);
    debug("dat " + JSON.stringify(data));
    let nextPlayerId = GetNextPlayer(data["playerId"], allPlayersId);
    let diceValue = data["diceValue"];
    debug("next player " + nextPlayerId);
    if (allPlayersId.length === 1)
      return;
    socket.to(roomName).emit(gamePlayApi.SWITCH_PLAYER, {
      nextPlayerId: nextPlayerId,
      diceValue: diceValue,
    });
    socket.emit(gamePlayApi.SWITCH_PLAYER, {
      nextPlayerId: nextPlayerId,
      diceValue: diceValue,
    });
    debug("game started at " + socket.id);
  });

  socket.on(gamePlayApi.AVOID_SWITCH_PLAYER, (data) => {
    debug("avoidSwitchingPlayer " + JSON.stringify(data));
    let diceValue = data["diceValue"];
    socket
      .to(roomName)
      .emit(gamePlayApi.AVOID_SWITCH_PLAYER, { diceValue: diceValue });
    debug("game started at " + socket.id);
  });

  socket.on(gamePlayApi.MOVE_PLAYER, (data) => {
    let pawnNo = data["pawnNo"];
    let diceValue = data["diceValue"];
    let playerId = data["playerId"];
    debug(`player ${pawnNo} move ${diceValue} of type ${playerId}`);
    socket.to(roomName).emit(gamePlayApi.PLAYER_MOVED, {
      pawnNo: pawnNo,
      diceValue: diceValue,
      playerId: playerId,
    });
  });

  socket.on(gamePlayApi.ON_PLAYER_WIN, (data) => {
    let playerId = data["playerId"];
    socket.to(roomName).emit(gamePlayApi.ON_PLAYER_WIN, { playerId });
  })

  socket.on(gamePlayApi.EXIT_ROOM, (data) => {
    let usersinfo = { socket, thisPlayerId, allPlayersId,  roomName }
    OnPlayerExitOrDissconceted(usersinfo);
  });

  socket.on("disconnect", () => {
    let usersinfo = { socket, thisPlayerId, allPlayersId,  roomName }
    OnPlayerExitOrDissconceted(usersinfo);
  });


}

function GetNextPlayer(currentPlayerId, allPlayersId) {
  let nextPlayerId = null;

  for (let i = 0; i < allPlayersId.length; i++) {
    if (allPlayersId[i] === currentPlayerId) {
      if (++i < allPlayersId.length) {
        nextPlayerId = allPlayersId[i];
        break;
      } else {
        nextPlayerId = allPlayersId[0];
        break;
      }
    }
  }
  return nextPlayerId;
}

function OnPlayerExitOrDissconceted(usersinfo) {
  let socket = usersinfo.socket,
    thisPlayerId = usersinfo.thisPlayerId,
    allPlayersId = usersinfo.allPlayersId,
    roomName = usersinfo.roomName;
  try {
    debug("exit room trigger");
    debug(`player ${thisPlayerId} exit`);
    user().removeUser(socket.id);

    let remainingPlayers = GetRemainingPlayersInTheRoom(
      thisPlayerId,
      allPlayersId
    );

    let invalideUser = remainingPlayers === 0 || remainingPlayers === undefined;
    if (invalideUser) return;
    debug("remaining users-" + remainingPlayers);
    if (remainingPlayers === 1) {
      debug(`player ${allPlayersId[0]} wins`);
      socket.to(roomName).emit(gamePlayApi.YOU_WIN, { playerId: thisPlayerId });
      user().removeUser(allPlayersId[0]);
      return;
    }
    socket.to(roomName).emit(gamePlayApi.EXIT_ROOM, { playerId: thisPlayerId });
  } catch (error) {
    debug("error:" + error);

  }
}

let GetRemainingPlayersInTheRoom = function (leftPlayer, allPlayers) {

  if (allPlayers.length === 1)
    return;

  for (let i = 0; i < allPlayers.length; i++) {
    if (allPlayers[i] === leftPlayer) {
      allPlayers.splice(i, 1);
      return allPlayers.length;
    }
  }
}