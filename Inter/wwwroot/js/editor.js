
/// <reference path="site.ts" />

const createThreadString = 'Создать тред';
const closePanelString = 'Закрыть панель';
const editorMaxHeight = 0.65;
const form = document.getElementsByClassName('owp-form')[0];
const owpTranslate = document.getElementsByClassName('owp-translate')[0];
const postText = document.getElementById('post-text');
const postMedia = document.getElementById('post-media');
const loginInputs = document.getElementsByName('login-input');
const owpBtn = document.getElementById('owp-btn');
const btnSubmit = document.getElementById('btnSubmit');

let isThreadFormReady = false;

;['resize', 'load'].forEach(eventName => {
    window.addEventListener(eventName, changePostView, false);
    window.addEventListener(eventName, changeMaxHeight, false);
    window.addEventListener(eventName, changePostPanelHeight, false);
});

owpBtn.addEventListener('click',  function () {
    toggleClass(owpTranslate, 'owp-translate');

    if (owpBtn.innerText === createThreadString) {
        owpBtn.innerText = closePanelString;
    } else {
        owpBtn.innerText = createThreadString;
    }
}, false);

function changePostPanelHeight() {
    let height = form.clientHeight;
    let root = document.documentElement;
    root.style.setProperty('--height-inter', ' ' + height + 'px');
}

function changeMaxHeight() {
    let height = document.documentElement.clientHeight * editorMaxHeight;
    let root = document.documentElement;
    root.style.setProperty('--max-height-inter', ' ' + height + 'px');
}

function changePostView() {
    let height = document.documentElement.clientHeight;
    let width = document.documentElement.clientWidth;

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
    let textArea = postText.getElementsByTagName('textarea')[0];

    if (textArea.value.length === 0) {
        setStatusThreadButton(true);
    }

    textArea.addEventListener('input',
        function (event) {
            if (event.currentTarget.value.length === 0) {
                isThreadFormReady = false;
                setStatusThreadButton(true);
            } else {
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