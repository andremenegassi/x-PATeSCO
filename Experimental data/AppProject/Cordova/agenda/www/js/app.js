define(function(require) {
    "use strict";
    var gerenciadorPagina = require("./gerenciadorPagina");
    var agendaCadastro = require("./agendaCadastro");
    var agendaDados = require("./agendaDados");
    var agendaPesquisa = require("./agendaPesquisa");

    var app = {

        init: function() {
            console.log("app.js", "init");
            gerenciadorPagina.init();
            gerenciadorPagina.exibePagina("#pgHome");
            agendaCadastro.init();
            agendaPesquisa.init();
            agendaDados.init();
        }
    };


    return app;
});
