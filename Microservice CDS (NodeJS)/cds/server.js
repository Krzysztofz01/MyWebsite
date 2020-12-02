'use strict';

const express = require('express');
const serveIndex = require('serve-index');
const path = require('path');
const app = express();

const imageFilesPath = path.join(__dirname, 'images');
app.use('/images', express.static(imageFilesPath), serveIndex(imageFilesPath, {
    hidden: false,
    icons: true,
    view: 'details'
}));

const projectsFilesPath = path.join(__dirname, 'projects');
app.use('/projects', express.static(projectsFilesPath), serveIndex(projectsFilesPath, {
    hidden: false,
    icons: true,
    view: 'details'
}));

app.get('/', (req, res) => {
    res.send('Content Delivery Network. Time:' + Date.now());
});

app.listen(3005);