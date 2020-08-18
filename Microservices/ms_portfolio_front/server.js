const express = require('express');
const serveIndex = require('serve-index');
const app = express();
require('dotenv').config();

//Static files middleware
app.use(express.static('public', {
    dotfiles: "ignore",
    index: false,
    redirect: false,
    setHeaders: function(res, path, stat) {
        res.set("x-Timestamp", Date.now());
    }
}));

//Enable dir indexing for gallery images
app.use('/galleryImages', serveIndex(__dirname + '/public/galleryImages'));

//Index route
app.get('/', (req, res) => {
    res.sendFile('./public/home.html', { root: __dirname });
});

//Gallery route
app.get('/gallery', (req, res) => {
    res.sendFile('./public/gallery.html', { root: __dirname });
});

//Server listen on port defined in .env file
app.listen(process.env.APP_PORT);