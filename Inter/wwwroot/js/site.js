// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

document.addEventListener('load', function () {
    document.querySelectorAll('textarea, input').forEach(function (element) {
        let value = window.sessionStorage[element.name];
        
        if (element.value === '' && (value !== '' || value !== null)) {
            element.value = value;
        }
        
        element.addEventListener('input', function () {
            value = element.value;
        }, false);
    });
}, false);

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

function getCoords(elem) {
    let box = elem.getBoundingClientRect();

    return {
        top: box.top + scrollY,
        left: box.left + scrollX
    };
}
