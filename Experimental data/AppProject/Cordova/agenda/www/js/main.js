(function() {
    "use strict";
    var ready = (window.cordova ? "deviceready" : "DOMContentLoaded");
    document.addEventListener(ready, function() {
        require(["js/app", "js/libs/fastclick"], function(app, FastClick) {
            FastClick.attach(document.body);
            app.init();
        });
    }, false);
})();
