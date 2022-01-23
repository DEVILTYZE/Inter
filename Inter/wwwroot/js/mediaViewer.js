
const divId = 'media-view';
const images = document.getElementsByName('media');
let mouseX = -1;
let mouseY = -1;
let minHeight = document.documentElement.clientHeight * 0.2;
let minWidth = document.documentElement.clientWidth * 0.2;
let maxHeight = document.documentElement.clientHeight * 10;
let maxWidth = document.documentElement.clientWidth * 10;
let lastOpenedImageUrl;

for (let i = 0; i < images.length; ++i) {
    images[i].firstElementChild.addEventListener('click',  function () {
        let height = document.documentElement.clientHeight;
        let width = document.documentElement.clientWidth;
        
        if (width > height) {
            viewMedia(images[i].lastElementChild.value, images[i].firstElementChild.alt);
        } else {
            
        }
        
        lastOpenedImageUrl = images[i].lastElementChild.value;
    }, false);
}

function viewMedia(url, name) {
    if (isImgExist() && lastOpenedImageUrl === url) {
        return;
    }

    let scaleHeight = 1;
    let scaleWidth = 1;
    
    let span = document.createElement('span');
    span.innerText = name;
    span.style.height = '100%';
    
    let div = document.createElement('div');
    div.id = divId;
    div.style.visibility = 'hidden';
    div.classList.add('media-viewer');
    div.classList.add('shadow');
    div.addEventListener('mousedown',  function (event) { moveImage(event); }, false);
    
    let img = document.createElement('img');
    img.src = url;
    img.draggable = false;
    img.addEventListener('dragstart', function () { return false; }, false);
    img.addEventListener('mousedown',  function (event) {
        mouseX = event.clientX;
        mouseY = event.clientY;
        moveImage(event);
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
        setStartSize(div);
        div.style.visibility = null;
        scaleHeight = img.clientHeight * 0.3;
        scaleWidth = img.clientWidth * 0.3;
    });
    img.addEventListener('wheel', function (event) {
        let delta = event.deltaY;
        
        if (delta > 0 && scaleHeight > 0 || delta < 0 && scaleHeight < 0) {
            scaleHeight *= -1;
            scaleWidth *= -1;
        }

        changeSize(img, scaleHeight, scaleWidth);
        changeSize(div, scaleHeight, scaleWidth);
        event.preventDefault();
    }, false);
    
    div.appendChild(span);
    div.appendChild(img);
    document.body.children[1].append(div);
}

function isImgExist() {
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

function setStartSize(element) {
    let heightCoef = document.documentElement.clientHeight / element.clientHeight;
    let widthCoef = document.documentElement.clientWidth / element.clientWidth;
    let coef = 1;
    
    if (heightCoef < 1 && widthCoef < 1) {
        coef = heightCoef < widthCoef ? heightCoef : widthCoef;
    } else if (heightCoef < 1) {
        coef = heightCoef;
    } else if (widthCoef < 1) {
        coef = widthCoef;
    }
    
    element.style.height = element.clientHeight * coef + 'px';
    element.style.width = element.clientWidth * coef + 'px';
}

function changeSize(element, scaleHeight, scaleWidth) {
    let height = element.offsetHeight + scaleHeight;
    let width = element.offsetWidth + scaleWidth;
    
    if (height > minHeight && width > minWidth && scaleHeight < 0 || height < maxHeight && width < maxWidth && scaleHeight > 0) {
        element.style.height = height + 'px';
        element.style.width = width + 'px';
        element.style.top = element.offsetTop - scaleHeight / 2 + 'px';
        element.style.left = element.offsetLeft - scaleWidth / 2 + 'px';
    }
}

function setStartPosition(element) {
    let height = document.documentElement.clientHeight / 2;
    let width = document.documentElement.clientWidth / 2;
    
    element.style.top = height - element.clientHeight / 2 + 'px';
    element.style.left = width - element.clientWidth / 2 + 'px';
}

function moveImage(event) {
    let element = event.currentTarget;
    let bounding = element.getBoundingClientRect();
    let shiftX = event.clientX - bounding.left;
    let shiftY = event.clientY - bounding.top;
    
    moveAt(event.clientX, event.clientY);
    
    function moveAt(x, y) {
        element.style.left = x - shiftX + 'px';
        element.style.top = y - shiftY + 'px';
    }
    
    function onMouseMove(event) {
        moveAt(event.clientX, event.clientY)
    }
    
    document.addEventListener('mousemove', onMouseMove, false);
    element.addEventListener('mouseup', function () {
        document.removeEventListener('mousemove', onMouseMove, false);
        element.onmouseup = null;
    }, false);
}
