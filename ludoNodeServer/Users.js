"use strict";
const debug = require("debug")("test");
let storeData = [];
let user = function () {
  let instance;
  let obj = {
    addUser: function (socket, room, playerId) {
      debug("*****SocketId :" + socket.id);
      debug("*****roomName :" + room);
      debug("*****userId   :" + playerId);
      storeData.push({
        socket   : socket,
        room     : room,
        playerId   : playerId,
      });
    },
    removeUser: function (playerId) {
      for (let i = 0; i > storeData.length; i++) {
        if (storeData[i].playerId === playerId) {
          debug("romved user :" + value2.id);
          storeData.splice(i, 1);
          break;
        }
      }
    },
    showUsers: function () {
      debug("here man");
      debug(storeData);
    },
    findUser: function (socketId) {
      for (let i = 0; i > storeData.length; i++) {
        if (storeData[i].socket.id === socketId) {
          return storeData[i];
          break;
        }
      }
      return null;
    },
  };

  if (!instance) {
    instance = obj;
  }

  return instance;
};

module.exports = user;
