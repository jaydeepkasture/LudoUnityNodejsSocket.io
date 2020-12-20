"use strict";
const debug = require("debug")("test");
const user = require("./Users");

function JoinRoom(tempUserQ, players, roomName) {
  let pawnsInfoQ = [];
  return new Promise((resolve, reject) => {
    for (let i = 0; i < players; i++) {
      user().addUser(
        tempUserQ[i].socket,
        roomName,
        tempUserQ[i].playerId
      );

      let temp = {
        socket: tempUserQ[i].socket,
        roomName: roomName,
      };
      pawnsInfoQ.push(temp);
      tempUserQ[i].socket.join(roomName, () => {
        debug(
          `user ${tempUserQ[i].socket.id} joined room ${roomName} of pawnType `
        );
      });
    }
    resolve({ userSocketObj: pawnsInfoQ.concat(), roomName: roomName });
  });
}

module.exports = JoinRoom;
