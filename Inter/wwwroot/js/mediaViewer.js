
document.addEventListener('DOMContentLoaded', () => {
    const media = document.getElementsByName('media');
    const images = Array(media.length);
    
    for (let i = 0; i < images.length; ++i) {
        images[i] = [media[i].firstElementChild, media[i].lastElementChild];
    }
    
    for (let i = 0; i < images.length; ++i) {
        images[i][0].addEventListener('click',  () => {
            let height = document.documentElement.clientHeight;
            let width = document.documentElement.clientWidth;
            
            if (width > height) {
                viewMediaDesktop(images, [i, i], true);
            } else {
                viewMediaMobile(images, i);
            }

        }, false);
    }
}, false);

function viewMediaDesktop(images, indexes, isFirstOpen) {
    const divId = 'media-view';
    let mouseX = -1;
    let mouseY = -1;
    const currentImage = images[indexes[0]];
    const url = currentImage[1].getAttribute('value').split(' ')[0];
    const name = currentImage[0].getAttribute('alt');
    let size = getSize(name);
    
    if (isImgExist(divId) && indexes[1] === indexes[0] && !isFirstOpen) {
        return;
    }

    let scaleHeight = 1;
    let scaleWidth = 1;

    const div = document.createElement('div');
    div.id = divId;
    div.style.visibility = 'hidden';
    div.setAttribute('media-info', name.split('(')[0] + ' (' + name.split('(')[1]);
    div.classList.add('media-viewer', 'shadow', 'rounded');
    div.addEventListener('mousedown',  function (event) { moveImage(event); }, false);

    const img = document.createElement('img');
    img.src = url;
    img.draggable = false;
    img.addEventListener('dragstart', () => { return false; }, false);
    img.addEventListener('mousedown', (event) => {
        mouseX = event.clientX;
        mouseY = event.clientY;
        moveImage(event);
    }, false);
    img.addEventListener('mouseup', (event) => {
        if (mouseX === event.clientX && mouseY === event.clientY) {
            mouseX = mouseY = -1;
            closeMedia(div);
        }
    }, false);
    img.addEventListener('wheel', (event) => {
        if (event.deltaY > 0 && scaleHeight > 0 || event.deltaY < 0 && scaleHeight < 0) {
            scaleHeight *= -1;
            scaleWidth *= -1;
        }

        changeSize(div, scaleHeight, scaleWidth);
        event.preventDefault();
    }, false);

    size = setStartSize(div, size, 50);
    setStartPosition(div, size);
    div.style.visibility = null;
    scaleHeight = size[1] * 0.3;
    scaleWidth = size[0] * 0.3;
    
    div.appendChild(img);
    document.body.children[1].append(div);
    createChangePicButtons(images, indexes);
}

function createChangePicButtons(images, indexes) {
    let values = ['<', '>'];
    let classNames = ['left', 'right'];
    let steps = [-1, 1];
    
    if (images.length < 2)
        return;
    
    for (let buttonIndex = 0; buttonIndex < 2; ++buttonIndex) {
        let button = document.createElement('button');
        button.innerText = values[buttonIndex];
        button.classList.add('media-viewer-button', classNames[buttonIndex]);
        button.addEventListener('click', () => {
            indexes[1] = indexes[0];
            indexes[0] += steps[buttonIndex];
            indexes[0] = indexes[0] === images.length ? 0: indexes[0];
            indexes[0] = indexes[0] === -1 ? images.length - 1: indexes[0];
            
            viewMediaDesktop(images, indexes, false);
        }, false);

        document.body.children[1].append(button);
    }
}

function isImgExist(id) {
    let divElement = document.getElementById(id);

    if (divElement !== null) {
        closeMedia(divElement);

        return true;
    }

    return false;
}

function closeMedia(element) {
    let buttons = document.getElementsByClassName('media-viewer-button');
    
    while (buttons.length > 0) {
        buttons[0].remove();
    }
    
    element.remove();
}

function getSize(name) {
    let splitName = name.split('.');
    let sizeString = splitName[0].split('(');
    let width = sizeString[1].split('x')[0];
    let height = sizeString[1].split('x')[1].slice(0, sizeString[1].split('x')[1].length - 1);
    
    return [width, height];
}

function setStartSize(element, size, padding) {
    let heightCoef = (document.documentElement.clientHeight - padding) / size[1];
    let widthCoef = (document.documentElement.clientWidth - padding) / size[0];
    let coef = 1;

    if (heightCoef < 1 && widthCoef < 1) {
        coef = heightCoef < widthCoef ? heightCoef : widthCoef;
    } else if (heightCoef < 1) {
        coef = heightCoef;
    } else if (widthCoef < 1) {
        coef = widthCoef;
    }
    
    element.style.width = size[0] * coef + 'px';
    
    return [size[0] * coef, size[1] * coef];
}

function setStartPosition(element, size) {
    const height = document.documentElement.clientHeight;
    const width = document.documentElement.clientWidth;
    
    element.style.top = (height - size[1]) / 2 + 'px';
    element.style.left = (width - size[0]) / 2 + 'px';
}

function changeSize(element, scaleHeight, scaleWidth) {
    let minHeight = document.documentElement.clientHeight * 0.2;
    let minWidth = document.documentElement.clientWidth * 0.2;
    let maxHeight = document.documentElement.clientHeight * 10;
    let maxWidth = document.documentElement.clientWidth * 10;
    let height = element.offsetHeight + scaleHeight;
    let width = element.offsetWidth + scaleWidth;

    if (height > minHeight && width > minWidth && scaleHeight < 0 || height < maxHeight && width < maxWidth && scaleHeight > 0) {
        element.style.width = width + 'px';
        element.style.top = element.offsetTop - scaleHeight / 2 + 'px';
        element.style.left = element.offsetLeft - scaleWidth / 2 + 'px';
    }
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
    element.addEventListener('mouseup', () => {
        document.removeEventListener('mousemove', onMouseMove, false);
        element.onmouseup = null;
    }, false);
}
