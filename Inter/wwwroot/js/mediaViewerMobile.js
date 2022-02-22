
function viewMediaMobile(images, index) {
    const currentImage = images[index];
    const url = currentImage[1].getAttribute('value').split(' ')[0];
    const div = document.createElement('div');
    const name = currentImage[0].getAttribute('alt');
    let size = getSize(name);
    div.style.height = document.documentElement.clientHeight + 'px';
    div.style.width = document.documentElement.clientWidth + 'px';
    div.classList.add('media-viewer-mobile');
    div.addEventListener('click', () => {
        div.remove();
    }, false);
    
    const img = document.createElement('img');
    img.src = url;
    size = setStartSize(img, size, 0);
    setStartPosition(img, size);
    
    div.appendChild(img);
    document.body.children[1].append(div);
}
