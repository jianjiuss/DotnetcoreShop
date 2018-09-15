function getQueryVariable(variable) {
    var query = window.location.search.substring(1);
    var vars = query.split("&");
    for (var i = 0; i < vars.length; i++) {
        var pair = vars[i].split("=");
        if (pair[0] == variable) { return pair[1]; }
    }
    return (false);
}

function appendWindowOnload(onloadFunction) {
    var oldOnload = window.onload;
    if (typeof window.onload == 'function') {
        window.onload = function () {
            oldOnload();
            onloadFunction();
        }
    }
    else {
        window.onload = onloadFunction;
    }
}

appendWindowOnload(function () {
    var loadingMain = document.getElementsByClassName("jui-loading");
    if (loadingMain.length == 0) { return; }
    for (var i = 0; i < loadingMain.length; i++){
        var currentDiv = loadingMain[i];

        currentDiv.style.width = "100px";
        currentDiv.style.margin = "auto";
        currentDiv.style.padding = "20px";

        var loadingIconDiv = document.createElement("div");
        var loadingIcon = document.createElement("span");
        loadingIcon.classList.add("mui-spinner");
        loadingIconDiv.appendChild(loadingIcon);

        loadingIconDiv.style.width = "100%";
        loadingIconDiv.style.textAlign = "center";

        var textDiv = document.createElement("div");
        var text = document.createElement("h6");
        text.appendChild(document.createTextNode("加载中"));
        textDiv.appendChild(text);

        textDiv.style.width = "100%";
        textDiv.style.textAlign = "center";

        currentDiv.appendChild(loadingIconDiv);
        currentDiv.appendChild(textDiv);
    }
    
})