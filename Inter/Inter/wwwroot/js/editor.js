
const maxHeight = 0.65;
const form = document.getElementsByClassName('owp-form')[0];
const owpTranslate = document.getElementsByClassName('owp-translate')[0];
const postText = document.getElementById('post-text');
const postMedia = document.getElementById('post-media');
const loginInputs = document.getElementsByName('login-input');

;['resize', 'load'].forEach(eventName => {
    window.addEventListener(eventName, changePostView, false);
    window.addEventListener(eventName, changeMaxHeight, false);
    window.addEventListener(eventName, changePostPanelHeight, false);
});

document.getElementById('owp-btn').addEventListener('click',  function () {
    toggleClass(owpTranslate, 'owp-translate');
}, false);

function changePostPanelHeight() {
    let height = form.clientHeight + 2;
    let root = document.documentElement;
    root.style.setProperty('--height-inter', ' ' + height + 'px');
}

function changeMaxHeight() {
    let height = document.documentElement.clientHeight * maxHeight;
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
                element.children[0].classList.replace('col-3', 'col-12');
                element.children[1].classList.replace('col-6', 'col-12');
                element.children[0].firstElementChild.classList.remove('text-right');
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
                element.children[0].classList.replace('col-12', 'col-3');
                element.children[1].classList.replace('col-12', 'col-6');
                element.children[0].firstElementChild.classList.add('text-right');
            });
        }
    }
}