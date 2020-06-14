const express = require('express');
const bodyParser = require('body-parser');
const authRoutes = require('./api/routes/userRoutes.js');
const env = require('./api/config/env.js');

//Create app
const app = express();

//Middleware
app.use(bodyParser.json());
app.use(bodyParser.urlencoded({ extended: true }));
app.use('/auth', authRoutes);

app.listen(env.default.port);