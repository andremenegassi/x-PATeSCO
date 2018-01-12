define(function(require) {
    "use strict";
    var agendaDados = require("./agendaDados");
    var gerenciadorPagina = require("./gerenciadorPagina");
    var agendaCadastro = require("./agendaCadastro");

    var agendaPesquisa = {


        onEditar: function(id){
            agendaCadastro.carregarParaEditar(id, function(acaoRealizada){
                this.onPesquisar();
            }.bind(this));
            gerenciadorPagina.exibePagina("#pgCadastro", "Alterar Cadastro");
        },

        delegaEventosList: function(){
            var that = this;
            var list = document.querySelector("#listResultados");
            [].forEach.call(list.querySelectorAll("[data-id]"), function(item){
                item.addEventListener("click", function(e){
                    var id = +this.getAttribute("data-id");
                    that.onEditar(id);
                    e.preventDefault();
                });
            });
        },

        renderList: function(listResultados) {
            var templateItem = [
                '<div class="mdl-list__item">',
                '<span class="mdl-list__item-primary-content">',
                '<img class="mdl-list__item-avatar" src="{{foto}}" />',
                '<span>{{nome}}</span>',
                '</span>',
                '<a class="mdl-list__item-secondary-action" data-id="{{id}}" href="#"><i class="material-icons">edit</i></a>',
                '</div>'
            ].join("");
            var html = ["<div>Total de registros: "+ listResultados.length + "</div>"];
            listResultados.forEach(function(item, indice) {
                var itemHTML = templateItem;
                for (var propriedade in item) {
                    itemHTML = itemHTML.replace(
                        new RegExp("{{" + propriedade + "}}", "gim"),
                        item[propriedade]
                    );
                }
                html.push(itemHTML);
            });
            document.querySelector("#listResultados").innerHTML = html.join("");
            this.delegaEventosList();
        },

        onPesquisar: function() {
            var query = document.querySelector("#txtPesquisa").value.trim();
            document.querySelector("#listResultados").innerHTML = "Pesquisando...";
            agendaDados.buscaItem("nome", query).then(function(listResultados) {
                console.log(listResultados);
                this.renderList(listResultados);
            }.bind(this)).catch(function(err) {
                alert("Erro: " + err.message);
            });
        },

        init: function() {
            console.log("agendaPesquisa.js", "init");
            document.querySelector("#txtPesquisa").addEventListener("keypress", function(e) {
                var code = e.keyCode || e.which;
                if (code === 13) {
                    this.onPesquisar();
                    e.preventDefault();
                    return false;
                }
            }.bind(this));
            document.querySelector("#btnPesquisar").addEventListener("click", function(e) {
                this.onPesquisar();
            }.bind(this));
        }
    };

    return agendaPesquisa;
});
