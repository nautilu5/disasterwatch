'user strict';
var sql = require('./db.js');

var Task = function(task){
    this.task = task.task;
    this.status = task.status;
    this.created_at = new Date();
};

Task.login = function createUser(name, email, result) {   
    sql.query("SELECT COUNT(1) FROM login WHERE email = ?", email, function (err, res) {
        if(err) {
            console.log("error: ", err);
            result(err, null);
        }
        else{
            if(res[0]["COUNT(1)"] == 1) {
                sql.query("UPDATE login SET dateaccessed = NOW() WHERE email = '" + email + "';", function (err2, res2) {
                    
                    if(err2) {
                        console.log("error: ", err2);
                        result(err2, null);
                    }
                    else{
                        console.log(res2.insertId);
                        result(null, res2.insertId);
                    }
                });
            }

            else {
                sql.query("INSERT INTO login (fullname, email, datecreated, dateaccessed) VALUES ('" + name + "', '" + email + "', NOW(), NOW());", function (err3, res3) {
                    
                    if(err3) {
                        console.log("error: ", err3);
                        result(err3, null);
                    }
                    else{
                        console.log(res3.insertId);
                        result(null, res3.insertId);
                    }
                });
            }         

        }
    });
};

Task.getpins = function createUser(email, result) {    
    // WHERE storedpins.createdby = '"+email+"'
    sql.query("SELECT storedpins.name,storedpins.latitude,storedpins.longitude,storedpins.type FROM storedpins ORDER BY storedpins.latitude ASC;", function (err, res) {       
        if(err) {
            console.log("error: ", err);
            result(err, null);
        }
        else{
            result(null, res);
        }
    });   
};

Task.addpin = function createUser(name,lat,lon,type,text,email,result) {    
    sql.query("INSERT INTO storedpins (name, latitude, longitude, type, textbox, createdby, datecreated) VALUES ('"+name+"',"+lat+","+lon+",'"+type+"','"+text+"','"+email+"', NOW());", function (err, res) {
        if(err) {
            console.log("error: ", err);
            result(err, null);
        }
        else{
            result(null, res);
        }
    });   
};

Task.undopin = function createUser(email, result) {
     
    sql.query("DELETE FROM storedpins WHERE createdby='"+email+"' AND datecreated = (SELECT * FROM (select MAX(datecreated) from storedpins) AS p);", function (err, res) {       
        if(err) {
            console.log("error: ", err);
            result(err, null);
        }
        else{
            result(null, res);
        }
    });   
};

module.exports= Task;