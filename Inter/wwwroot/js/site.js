// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

let config;

document.addEventListener('DOMContentLoaded', function () {
    let requestUrl = "/Forum/GetConfig";
    let request = new XMLHttpRequest();
    
    request.open('get', requestUrl);
    request.responseType = "json";
    request.send();
    
    request.addEventListener('load', function () {
        config = JSON.parse(request.response);
    }, false);
}, false);

document.addEventListener('DOMContentLoaded', function () {
    document.querySelectorAll('textarea, input').forEach(function (element) {
        let value = sessionStorage[element.localName];
        let elementValue = element.getAttribute('value');

        if (elementValue === '' && (value !== '' || value !== null)) {
            elementValue = value;
        }

        element.addEventListener('input', function () {
            value = elementValue;
        }, false);
    });
}, false);

function changeTheme() {
    const darkName = config['DarkThemeName'];
    const lightName = config['LightThemeName'];
    //const darkName = 'dark';
    //const lightName = 'light';
    let btnTheme = document.getElementById('theme-button');
    let theme = btnTheme.value;
    
    theme = theme === lightName ? darkName : lightName;
    btnTheme.firstElementChild.src = '/files/_system/images/theme_' + theme + '_icon.svg';
    btnTheme.value = theme;
    
    ajaxLoadTheme(theme);
    loadTheme(theme);
}

function loadTheme(theme) {
    let styleSheets = document.getElementsByTagName('link');
    let styleSheet;
    
    for (let i = 0; i < styleSheets.length; ++i) {
        if (styleSheets[i].href.endsWith('Theme.css')) {
            styleSheet = styleSheets[i];
            break;
        }
    }
    
    styleSheet.href = '/css/' + theme + 'Theme.css';
}

function ajaxLoadTheme(theme) {
    $.ajax({
        url: "/Forum/LoadTheme",
        type: "POST",
        data: {theme: theme},
        dataType: "text",
        success: function (result) {
            console.log("Theme status: " + result);
        }
    });
}

function toggleClass(elem, className) {
    elem.classList.toggle(className);
}

function changeClass(elem, className, changedClassName) {
    if (className === '') {
        elem.classList.add(changedClassName);
        return;
    } else if (changedClassName === '') {
        elem.classList.remove(className);
        return;
    }

    elem.classList.replace(className, changedClassName);
}

function getCoords(element) {
    let box = element.getBoundingClientRect();

    return {
        top: box.top + scrollY,
        left: box.left + scrollX
    };
}

let searchInput = document.getElementById('searchPattern');

if (searchInput !== null && searchInput !== undefined) {
    searchInput.addEventListener('keyup', function (event) {
        submitSearchForm(event);
    }, false);
}

function submitSearchForm(event) {
    if (event.key === 'Enter') {
        let formElement = document.getElementById('searchPostForm');

        if (formElement !== null && formElement !== undefined) {
            formElement.submit();
        }
    }
}
