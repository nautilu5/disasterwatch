'user strict';

var mysql = require('mysql');

var connection = mysql.createConnection({
    host     : 'hostip',
    user     : 'root',
    password : 'password',
    database : 'mydb'
});

connection.connect(function(err) {
    if (err) throw err;
});

module.exports = connection;