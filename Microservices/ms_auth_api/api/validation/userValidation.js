const Joi = require('@hapi/joi');

//Register validation with schema
const registerValidation = (data) => {
    const schema = Joi.object({
        login: Joi.string().min(6).required(),
        password: Joi.string().min(8).required()
    });
    return schema.validate(data);
};

//Login validation with schema
const loginValidation = (data) => {
    const schema = Joi.object({
        login: Joi.string().min(6).required(),
        password: Joi.string().min(8).required()
    });
    return schema.validate(data);
};

module.exports.registerValidation = registerValidation;
module.exports.loginValidation = loginValidation;