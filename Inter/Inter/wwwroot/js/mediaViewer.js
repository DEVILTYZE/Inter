
const divId = 'media-view';
const images = document.getElementsByName('media');
let mouseX = -1;
let mouseY = -1;

for (let i = 0; i < images.length; ++i) {
    images[i].firstElementChild.addEventListener('click',  function () {
        let height = document.documentElement.clientHeight;
        let width = document.documentElement.clientWidth;
        
        if (width > height) {
            viewMedia(images[i].lastElementChild.value);
        } else {
            
        }
    }, false);
}

function viewMedia(url) {
    if (isDivExist())
        return;

    let scale = 1;
    let div = document.createElement('div');
    div.id = divId;
    div.style.visibility = 'hidden';
    div.classList.add('media-viewer');
    div.classList.add('shadow');
    div.addEventListener('mousedown',  function (event) { 
        moveImage(event);
    }, false);
    div.addEventListener('dragstart', function () {
        return false;
    }, false);
    
    let img = document.createElement('img');
    img.src = url;
    img.draggable = false;
    img.addEventListener('mousedown',  function (event) {
        mouseX = event.clientX;
        mouseY = event.clientY;
    }, false);
    img.addEventListener('mouseup',  function (event) {
        let currentMouseX = event.clientX;
        let currentMouseY = event.clientY;
        
        if (mouseX === currentMouseX && mouseY === currentMouseY) {
            mouseX = mouseY = -1;
            closeMedia(div);
        }
    }, false);
    img.addEventListener('load',  function() {
        setStartPosition(div);
        img.style.height = '100%';
        img.style.width = '100%';
        div.style.visibility = null;
    });
    img.addEventListener('wheel', function (event) {
        let delta = event.deltaY;
        
        if (delta > 0) {
            scale -= 0.15;
        } else {
            scale += 0.15;
        }
        
        div.style.transform = 'scale(' + scale + ')';
        img.style.transform = 'scale(' + scale + ')';
        
        event.preventDefault();
    }, false);

    div.appendChild(img);
    document.body.insertBefore(div, document.body.firstElementChild);
}

function isDivExist() {
    let divElement = document.getElementById(divId);

    if (divElement !== null) {
        closeMedia(divElement);
        
        return true;
    }
    
    return false;
}

function closeMedia(element) {
    element.remove();
}

function setStartPosition(element) {
    let height = document.documentElement.clientHeight / 2 + window.scrollY;
    let width = document.documentElement.clientWidth / 2 + window.scrollX;
    
    element.style.top = height - element.clientHeight / 2 + 'px';
    element.style.left = width - element.clientWidth / 2 + 'px';
}

function moveImage(event) {
    let element = event.currentTarget;
    let shiftX = event.clientX - element.getBoundingClientRect().left;
    let shiftY = event.clientY - element.getBoundingClientRect().top;
    
    moveAt(event.pageX, event.pageY);
    
    function moveAt(x, y) {
        element.style.left = x - shiftX + 'px';
        element.style.top = y - shiftY + 'px';
    }
    
    function onMouseMove(event) {
        moveAt(event.pageX, event.pageY)
    }
    
    document.addEventListener('mousemove', onMouseMove, false);
    element.addEventListener('mouseup', function () {
        document.removeEventListener('mousemove', onMouseMove, false);
        element.onmouseup = null;
    }, false);
}
