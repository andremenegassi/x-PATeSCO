define(function(require){
    "use strict";
    var PersistenciaDados = require("./PersistenciaDados");
    var agendaDados = new PersistenciaDados({
        key: "_agenda_dados.json"
    });

    return agendaDados;
});
