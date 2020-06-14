const mysql = require('../config/database.js');
const env = require('../config/env.js');
const bcrypt = require('bcrypt');
const jwt = require('jsonwebtoken');

//User model
const User = function(user) {
    this.login = user.login,
    this.password = user.password,
    this.permissions = user.permissions
};

//User register method
User.register = async (newUser, result) => {
    //Check if user already exists
    mysql.query(`SELECT login FROM users WHERE login = "${ newUser.login }"`, async (selectErr, selectRes) => {
        if(selectErr) {
            result({
                code: 500,
                info: selectErr
            }, null);
            return;
        }
        
        if(selectRes.length) {
            result({
                code: 409,
                info: "User with that login already exist."
            }, null);
            return;
        }

        //Hash password
        const salt = await bcrypt.genSalt(10);
        const hashPassword = await bcrypt.hash(newUser.password, salt);

        //Add user to database
        mysql.query(`INSERT INTO users (login, password) VALUES ("${ newUser.login }", "${ hashPassword }")`, async (createErr, createRes) => {
            if(createErr) {
                result({
                    code: 500,
                    info: createErr
                }, null);
                return;
            }
            result(null, { message: "User has been created."});
        });
    });
};

//User login method
User.login = async (newUser, result) => {
    //Check if user with specific login exists
    mysql.query(`SELECT id, login, password FROM users WHERE login = "${ newUser.login }" LIMIT 1`, async (selectErr, selectRes) => {
        if(selectErr) {
            result({
                code: 500,
                info: selectErr
            }, null);
            return;
        }

        //User found
        if(!selectRes.length) {
            result({
                code: 404,
                info: "No user found with given credentials."
            }, null);
            return;
        }

        //Password check
        const validPass = await bcrypt.compare(newUser.password, selectRes[0].password);  
        if(!validPass) {
            result({
                code: 403,
                info: "Wrong login or password."
            }, null);
            return;
        }
        
        //Select all user permissions from database
        mysql.query(`SELECT p.name FROM userhaspermission uhp INNER JOIN permission p ON (uhp.idPermission = p.id) WHERE uhp.idUser = ${ selectRes[0].id }`, async (permErr, permRes) => {
            if(permErr) {
                result({
                    code: 500,
                    info: permErr
                }, null);
                return;
            }
            
            //Create a JsonWebToken
            const token = jwt.sign({
                login: selectRes[0].login,
                permissions: permRes
            }, env.default.tokenSecret);

            result(null, { token: token });
        });

    });
};

module.exports = User;