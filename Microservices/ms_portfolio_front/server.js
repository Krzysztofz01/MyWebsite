const express = require('express');
const app = express();
require('dotenv').config();

app.use(express.static('public', {
    dotfiles: "ignore",
    index: false,
    redirect: false,
    setHeaders: function(res, path, stat) {
        res.set("x-Timestamp", Date.now());
    }
}));

app.get('*', (req, res) => {
    res.sendFile('./public/home.html', { root: __dirname });
});

app.listen(process.env.APP_PORT);