"use strict";
const debug = require("debug")("test");
const createRoom = require("./CreateRoomAndTheStartGame");
const clientApi=require("./GamePlay/ClientApi").eastablishConection;
const tempTwoUserQ = new Array();
const tempFourUserQ = new Array();

const MatchMaking = function (data) {
  debug("match making started.......")

  let playersPlaying = data["players"];
  switch (playersPlaying) {
    case 2:
     MatchPlayers(data,tempTwoUserQ,2);
      break;

    case 3:
      break;

    case 4:
     MatchPlayers(data,tempFourUserQ,4);
      break;
  }
  PlayerMonitor(data);
};

function MatchPlayers(newPlayerInfo,respectiveTempQ,roomLimit) {
  let lessPlayer = respectiveTempQ.length < roomLimit-1;

  if (lessPlayer) {
    respectiveTempQ.push(newPlayerInfo);
    return;
  }
  respectiveTempQ.push(newPlayerInfo);

  let matchedUserQ = [];
  
  for (let i = 0; i < respectiveTempQ.length; i++) {
    let disconnectedPlayer = !respectiveTempQ[i].socket.connected;
    if (disconnectedPlayer) break;

    matchedUserQ.push(respectiveTempQ[i]);

    if(matchedUserQ.length===roomLimit){
      createRoom(matchedUserQ, roomLimit);
      respectiveTempQ.splice(0,roomLimit);
      debug("Match making completed..... :)");
      return;
    }
  }
  debug("Match making failed..... :)");
}

function PlayerMonitor(data){
  data["socket"].on("disconnect", () => {
    disconnectPlayer();
  });
  data["socket"].on(clientApi.ON_QUIT, () => {
    disconnectPlayer();
  });

  function disconnectPlayer(){
    let userDetail = {
      players: data.players,
      socket: data.socket,
    };
    debug("user " + data.socket.id + " left");
    RemoveUserFromTempQafterClientDisconnect(userDetail);
  }
}
function RemoveUserFromTempQafterClientDisconnect(user) {
  switch (user.players) {
    case 2:
      for (let i = 0; i < tempTwoUserQ.length; i++) {
        if (tempTwoUserQ[i].socket.id === user.socket.id) {
          tempTwoUserQ.splice(i, 1);
          debug(user.socket.id + " left machmaking");
          break;
        }
      }
      break;

    case 3:
      break;

    case 4:
      for (let i = 0; i < tempFourUserQ.length; i++) {
        if (tempFourUserQ[i].socket.id === user.socket.id) {
          tempFourUserQ.splice(i, 1);
          debug(user.socket.id + " left machmaking");
          break;
        }
      }
      break;
  }
}


module.exports = MatchMaking;
