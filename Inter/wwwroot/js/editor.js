
const owpBtn = document.getElementById('owp-btn');
const btnSubmit = document.getElementById('btnSubmit');
const buttons = document.getElementById('owp-main-panel').getElementsByClassName('btn-svg');

let isThreadFormReady = false;

;['resize', 'load'].forEach(eventName => {
    window.addEventListener(eventName, changePostView, false);
    window.addEventListener(eventName, changeMaxHeight, false);
    window.addEventListener(eventName, changePostPanelHeight, false);
});

document.addEventListener('DOMContentLoaded', () => {
    const notExpandLength = 2000;
    let postTexts = document.getElementsByName('post-text');
    
    for (let i = 0; i < postTexts.length; ++i) {
        
        if (postTexts[i].innerText.length > notExpandLength) {
            let btn = document.createElement('button');
            btn.classList.add('expand-link-inter', 'mt-2');
            btn.innerText = 'Раскрыть текст';
            
            let div = document.createElement('div');
            div.style.display = 'none';
            div.innerText = postTexts[i].innerText;
            
            postTexts[i].innerText = postTexts[i].innerText.slice(0, notExpandLength - 3) + '... ';
            postTexts[i].parentElement.append(btn);
            btn.addEventListener('click', function () {
                if (btn.innerText === 'Раскрыть текст') {
                    btn.innerText = 'Скрыть текст';
                    postTexts[i].innerText = div.innerText;
                } else {
                    btn.innerText = 'Раскрыть текст';
                    div.innerText = postTexts[i].innerText;
                    postTexts[i].innerText = postTexts[i].innerText.slice(0, notExpandLength - 3) + '... ';
                }
            }, false);
        }
    }
}, false);

owpBtn.addEventListener('click',  () => {
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
    
    if (document.documentElement.clientHeight > document.documentElement.clientWidth)
        coef = 1.5;
    
    let height = document.documentElement.clientHeight * 0.65 / coef;
    let root = document.documentElement;
    root.style.setProperty('--max-height-inter', ' ' + height + 'px');
}

function changePostView() {
    let height = document.documentElement.clientHeight;
    let width = document.documentElement.clientWidth;
    const postText = document.getElementById('post-text-editor');
    const postMedia = document.getElementById('post-media-editor');
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
    const className = "text-danger-inter";
    const textLengthCounter = document.getElementById('text-length-counter');
    const postText = document.getElementById('post-text-editor');
    const textArea = postText.getElementsByTagName('textarea')[0];

    if (textArea.value.length === 0) {
        setStatusThreadButton(true);
    }

    textArea.addEventListener('input', () => { changeCounterOfLastLetters(textArea) } , false);

    textArea.addEventListener('input',
        function (event) {
            if (event.currentTarget.value.length === 0) {
                if (textLengthCounter.classList.contains(className)) {
                    textLengthCounter.classList.add(className);
                }
                
                isThreadFormReady = false;
                setStatusThreadButton(true);
            } else if (Number(textLengthCounter.innerText) < 0) {
                textLengthCounter.classList.add(className);
                isThreadFormReady = false;
                setStatusThreadButton(true);
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

function changeCounterOfLastLetters(textArea) {
    const textLengthCounter = document.getElementById('text-length-counter');
    const maxTextLength = Number(config['MaxTextLength']);
    textLengthCounter.innerText = (maxTextLength - textArea.value.length).toString();
}

if (buttons.length > 0) {
    const postText = document.getElementById('post-text-editor');
    const textArea = postText.getElementsByTagName('textarea')[0];
    
    for (let i = 0; i < buttons.length; ++i) {

        buttons[i].addEventListener('click', () => {
            let tags = buttons[i].getAttribute('value').split(' ');
            let indexes = [textArea.selectionStart, textArea.selectionEnd];

            textArea.value = textArea.value.slice(0, indexes[0]) + tags[0] + textArea.value.slice(indexes[0], indexes[1]) 
                + tags[1] + textArea.value.slice(indexes[1]);
            textArea.focus();
            
            changeCounterOfLastLetters(textArea);
        }, false);
    }
}
