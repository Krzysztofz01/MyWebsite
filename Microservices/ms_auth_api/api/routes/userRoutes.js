const router = require('express').Router();
const controller = require('../controllers/userController.js');

//All routes from /auth API
router.post('/register', controller.register);
router.post('/login', controller.login);

module.exports = router;