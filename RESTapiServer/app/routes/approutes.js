'use strict';
module.exports = function(app) {
  var controll = require('../controller/Controller.js');

  app.route('/users')
    .get(controll.addauser)
    .post(controll.getthepins);

  app.route('/pins')
    .get(controll.createapin)
    .post(controll.undoapin);
};