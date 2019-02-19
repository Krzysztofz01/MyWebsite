var i = 0;
var images = [];
var time = 3000;

//Wszystkie zdjęcia
images[0] = '../content/gallery/pic1.jpg';
images[1] = '../content/gallery/pic2.png';
images[2] = '../content/gallery/pic3.png';
images[3] = '../content/gallery/pic4.png';

//Zamiana zdjęcia
function changeImg(){
    document.slide.src = images[i];

    if(i < images.length - 1){
        i++;
    } else {
        i = 0;
    }

    setTimeout("changeImg()", time);
}
window.onload = changeImg;