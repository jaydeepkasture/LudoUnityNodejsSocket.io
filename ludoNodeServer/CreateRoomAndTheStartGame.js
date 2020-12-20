"use strict";
const JoinRoom = require("./JoinRoom");
const debug = require("debug")("test");
const startGame = require("./GamePlay/GameLogic");
const shortId = require("shortid");

async function CreateRoom(tempUserQ, roomLimit) {
  let players = roomLimit;
  let roomName = shortId.generate();
  let data = await JoinRoom(tempUserQ, roomLimit, roomName);
  let allPlayersId = [];
  let socketDictionary = {};
  let profiles = {};
  for (let i = 0; i < tempUserQ.length; i++) {
    allPlayersId.push(tempUserQ[i].playerId);
    socketDictionary[tempUserQ[i].playerId] = tempUserQ[i].socket;
    profiles[tempUserQ[i].playerId] = tempUserQ[i].profile;
  }
  let playerData = {
    userSocketObj: data["userSocketObj"],
    roomName: data["roomName"],
    allPlayersId,
    socketDictionary,
    profiles
  }
  startGame(playerData);
}

module.exports = CreateRoom;
