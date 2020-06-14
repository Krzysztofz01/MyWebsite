const mysql = require('mysql2');
const env = require('./env.js');

//Connection to database
const connection = mysql.createConnection({
    host: env.default.dbHost,
    user: env.default.dbUser,
    password: env.default.dbPass,
    database: env.default.dbName
});

connection.connect(error => {
    if(error) throw error;
});

module.exports = connection;