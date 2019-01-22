'use strict';

var Task = require('../model/appModel.js');

exports.addauser = function(req, res) {
  Task.login(req.query.fullname, req.query.email, function(err, task) {
    console.log('controller')
    if (err)
      res.send(err);
      console.log('res', task);
    res.json(task);
  });
};

exports.getthepins = function(req, res) {
  Task.getpins(req.query.email, function(err, task) {
    console.log('controller')
    if (err)
      res.send(err);
      console.log('res', task);
    res.json(task);
  });
}

exports.createapin = function(req, res) {
  Task.addpin(req.query.name,req.query.lat,req.query.lon,req.query.type,req.query.text,req.query.email, function(err, task) {
    console.log('controller')
    if (err)
      res.send(err);
      console.log('res', task);
    res.json(task);
  });
}

exports.undoapin = function(req, res) {
  Task.undopin(req.query.email, function(err, task) {
    console.log('controller')
    if (err)
      res.send(err);
      console.log('res', task);
    res.json(task);
  });
}