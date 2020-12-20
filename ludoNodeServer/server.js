"use strict";
const express = require("express");
const path = require("path");
const router = express.Router();
const { type } = require("os");
const app = require("express")();
const http = require("http").createServer(app);
const io = require("socket.io")(http);
const bodyParser = require("body-parser");
const debug = require("debug")("test");
const matchMaking = require("./MatchMaking");
const user = require("./Users");
const PORT = process.env.PORT || 3000;
const shotId = require("shortid");
const eastablishConection = require("./GamePlay/ClientApi").eastablishConection;


let onlineUserQ = [];

io.on("connection", (socket) => {
  debug("a user connected " + socket.id);

  onlineUserQ.push(socket);
  for (let i = 0; i < onlineUserQ.length; i++) {
    onlineUserQ[i].emit(eastablishConection.ONLINE_PLAYERS, {
      onlinePlayers: onlineUserQ.length,
    });
  }

  socket.on(eastablishConection.ONLINE_PLAYERS, (data) => {
    socket.emit(eastablishConection.ONLINE_PLAYERS, { onlinePlayers: onlineUserQ.length });
  });
  socket.on(eastablishConection.PLAYER_REGISTRATION, (data) => {
    let invalidId = data["playerId"] === "null" || data["playerId"] === "";
    if (invalidId) {
      debug("new user");
      let id = "guest" + shotId.generate();
      socket.emit(eastablishConection.PLAYER_REGISTRATION, { id });
    } else {
      debug("old user");
    }
  });
  socket.on("quit", (data) => {
    debug(`user ${socket.id} quit`);
  });
  socket.on("showData", (data) => {
    console.log(user().showUsers());
  });
  socket.on(eastablishConection.MATCH_MAKING, (data) => {


    let newUserDetail = {
      socket: socket,
      playerId: data["playerId"],
      players: data["players"],
      profile: data["profilePic"],
    };
    socket.players = data["players"];
    matchMaking(newUserDetail);
  });
  socket.on("disconnect", () => {
    debug("disconnected " + socket.id);
    let length = onlineUserQ.length - 1;
    for (let i = 0; i < onlineUserQ.length; i++) {
      onlineUserQ[i].emit(eastablishConection.ONLINE_PLAYERS, { onlinePlayers: length });
      if (onlineUserQ[i].id === socket.id) {
        onlineUserQ.splice(i, 1);
      }
    }
  });
});

http.listen(PORT, () => {
  debug("listening on " + PORT);
});
