
let dropArea = document.getElementById('drop-area');
let input = document.getElementById('drag-and-drop-input');
let buttonClicked = false;

dropArea.addEventListener('drop', handleDrop, false);
dropArea.addEventListener('dragleave', fuckBlinks, false);
input.addEventListener('change', handleInput, false);
window.addEventListener('load', setButtonEvent, false);
window.addEventListener('beforeunload', unloadFiles, false);

;['dragenter', 'dragover', 'dragleave', 'drop'].forEach(eventName => {
    dropArea.addEventListener(eventName, preventDefaults, false);
})

function setButtonEvent() {
    document.getElementById('btnSubmit').onclick = function () {
        buttonClicked = true;
    }
}

function unloadFiles() {
    let fileString = document.getElementsByName('filePathInput')[0].getAttribute('value');

    if (fileString === '' || fileString === null || buttonClicked)
        return;

    $.ajax({
        url: getController() + '/Unload',
        type: 'POST',
        data: {data: fileString},
        dataType: "text",
        success: function (result) {
            console.log(result);
        }
    });
}

function preventDefaults(e) {
    e.preventDefault();
    e.stopPropagation();
}

function fuckBlinks(e) {
    const dragAndDropLight = 'drag-and-drop-highlight';
    let coords = getCoords(dropArea);
    let dx = e.pageX - coords.left;
    let dy = e.pageY - coords.top;

    if ((dx < 0) || (dx > dropArea.offsetWidth) || (dy < 0) || (dy > dropArea.offsetHeight)) {
        dropArea.classList.remove(dragAndDropLight);
    }
}

function handleDrop(e) {
    const dragAndDropLight = 'drag-and-drop-highlight';
    let dt = e.dataTransfer;
    let files = dt.files;

    changeClass(dropArea, dragAndDropLight, '');
    sendFiles(files);
}

function handleInput() {
    let files = this.files;

    sendFiles(files);
}

function sendFiles(files) {
    const maxFileSize = Number(config['MaxFileSize']);
    //const maxFileSize = 10485760;
    let formData = new FormData();
    let fileArray = [];
    let count = 0;
    
    for (let i = 0; i < files.length; ++i) {
        if (files[i].size <= maxFileSize) {
            formData.append('file_' + i, files[i]);
            fileArray[count] = files[i];
            ++count;
        }
    }

    $.ajax({
        url: getController() + '/Upload',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function(result) { // Result — путь файла, его стоит передать в скрытый инпут, если это возможно
            document.getElementsByName('filePathInput')[0].setAttribute('value', result);
            previewImages(fileArray);
        }
    });
}

function previewImages(files) {
    const threadController = '/Thread';
    let pic = document.getElementById('uploadFile');
    let preview = dropArea.getElementsByClassName('row')[1];
    let anyImageInFiles = false;
    
    if (getController() === threadController) {
        changePostPanelHeight();
    }

    for (let i = 0; i < files.length; ++i) {
        const file = files[i];

        if (!file.type.startsWith('image/')) {
            continue;
        }

        if (!anyImageInFiles) {
            preview.removeChild(pic.parentNode);
            anyImageInFiles = true;
        }

        if (getController() === threadController) {
            setStatusThreadButton();
        }
        
        let col = document.createElement('div')
        col.classList.add('col-6');
        const img = document.createElement('img');
        img.classList.add('img-fluid');
        col.appendChild(img);
        preview.appendChild(col);

        const reader = new FileReader();
        reader.onload = (function(aImg) {
            return function(e) {
                aImg.src = e.target.result;
            };
        })(img);
        reader.readAsDataURL(file);
    }
}

function getController() {
    let controller = window.location.pathname.slice(0, window.location.pathname.lastIndexOf('/'));

    if (controller === '')
        controller = window.location.pathname;

    return controller;
}
