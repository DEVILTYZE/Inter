
const owpBtn = document.getElementById('owp-btn');
const btnSubmit = document.getElementById('btnSubmit');

let isThreadFormReady = false;

;['resize', 'load'].forEach(eventName => {
    window.addEventListener(eventName, changePostView, false);
    window.addEventListener(eventName, changeMaxHeight, false);
    window.addEventListener(eventName, changePostPanelHeight, false);
});

owpBtn.addEventListener('click',  function () {
    const createThreadString = 'Создать тред';
    const closePanelString = 'Закрыть панель';
    const owpTranslate = document.getElementById('owp-main-panel');
    toggleClass(owpTranslate, 'owp-translate');

    if (owpBtn.innerText === createThreadString) {
        owpBtn.innerText = closePanelString;
    } else {
        owpBtn.innerText = createThreadString;
    }
}, false);

function changePostPanelHeight() {
    let height = document.getElementsByClassName('owp-form')[0].clientHeight;
    let root = document.documentElement;
    root.style.setProperty('--height-inter', ' ' + height + 'px');
}

function changeMaxHeight() {
    let coef = 1;
    const editorMaxHeight = Number(config['EditorMaxHeight']);
    //const editorMaxHeight = 0.65;
    
    if (document.documentElement.clientHeight > document.documentElement.clientWidth)
        coef = 1.5;
    
    let height = document.documentElement.clientHeight * editorMaxHeight / coef;
    let root = document.documentElement;
    root.style.setProperty('--max-height-inter', ' ' + height + 'px');
}

function changePostView() {
    let height = document.documentElement.clientHeight;
    let width = document.documentElement.clientWidth;
    const postText = document.getElementById('post-text');
    const postMedia = document.getElementById('post-media');
    const loginInputs = document.getElementsByName('login-input');

    if (height > width) {
        if (postText !== null) {
            postText.classList.replace('col-8', 'col-12');
        }

        if (postMedia !== null) {
            postMedia.classList.replace('col-4', 'col-12');
        }

        if (loginInputs !== null) {
            loginInputs.forEach(element => {
                element.firstElementChild.classList.replace('col-6', 'col-12');
            });
        }
    } else {
        if (postText !== null) {
            postText.classList.replace('col-12', 'col-8');
        }

        if (postMedia !== null) {
            postMedia.classList.replace('col-12', 'col-4');
        }

        if (loginInputs !== null) {
            loginInputs.forEach(element => {
                element.firstElementChild.classList.replace('col-12', 'col-6');
            });
        }
    }
}

if (btnSubmit !== null && btnSubmit !== undefined) {
    const maxTextLength = Number(config['MaxTextLength']);
    //const maxTextLength = 35000;
    const className = "text-danger-inter"
    const postText = document.getElementById('post-text');
    const textArea = postText.getElementsByTagName('textarea')[0];
    const textLengthCounter = document.getElementById('text-length-counter');

    if (textArea.value.length === 0) {
        setStatusThreadButton(true);
    }

    textArea.addEventListener('input', function () {
        textLengthCounter.innerText = (maxTextLength - textArea.value.length).toString();
    }, false);

    textArea.addEventListener('input',
        function (event) {
            if (event.currentTarget.value.length === 0) {
                isThreadFormReady = false;
                setStatusThreadButton(true);
            } else if (Number(textLengthCounter.innerText) < 0) {
                textLengthCounter.classList.add(className);
            } else {
                textLengthCounter.classList.remove(className);
                isThreadFormReady = true;
                setStatusThreadButton();
            }
        }, false);
}

function setStatusThreadButton(isDisable = false) {
    if (isDisable) {
        btnSubmit.classList.add('disabled');
    } else {
        btnSubmit.classList.remove('disabled');
    }
}

