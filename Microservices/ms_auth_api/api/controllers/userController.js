const User = require('../models/userModel.js');
const { registerValidation, loginValidation } = require('../validation/userValidation.js');

//Register route controller
exports.register = async (req, res) => {
    //Validation
    const { error } = registerValidation(req.body);
    if(error) return res.status(400).send({ message: error.details[0].message });

    //Creating a new user object
    const user = new User({
        login: req.body.login,
        password: req.body.password
    });
    
    //Calling the models register method
    await User.register(user, (err, data) => {
        if(err) { 
            res.status(err.code || 500).send({ message: err.info || err.message || "Some error occurred while creating a new record." }); 
        } else {
            res.send(data);
        } 
    });
};

//Login route controller
exports.login = async (req, res) => {
    //Validation
    const { error } = loginValidation(req.body);
    if(error) return res.status(400).send({ message: error.details[0].message });

    //Creating a new user object
    const user = new User({
        login: req.body.login,
        password: req.body.password
    });

    //Calling the models login method
    await User.login(user, (err, data) => {
        if(err) { 
            res.status(err.code || 500).send({ message: err.info || err.message || "Some error occurred while creating a new record." }); 
        } else {
            res.header('auth-token', data.token).send(data);
        } 
    });
};
