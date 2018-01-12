define(function(require) {
    "use strict";

    var gerenciadorPagina = {
        pgAnterior: null,

        listHistorico: [],

        indiceAtual: 0,

        exibePagina: function(seletor, titulo) {
            var atual = document.querySelector(seletor);
            if(atual === this.pgAnterior){
                return;
            }
            if (this.pgAnterior) {
                this.pgAnterior.style.display = "none";
            }
            atual.style.display = "block";
            this.pgAnterior = atual;
            //senao tiver titulo pega a da pagina ou a padrao
            if(!titulo){
                titulo = atual.getAttribute("data-pagina-titulo") || "Agenda";
            }
            this.listHistorico.push({
                seletor: seletor,
                titulo: titulo
            });
            this.indiceAtual = this.listHistorico.length - 1;
            document.querySelector("#pagina-titulo").innerHTML = titulo;
        },

        voltarHistorico: function() {
            var anterior = this.listHistorico[this.indiceAtual - 1];
            if(!anterior){
                return;
            }
            this.listHistorico.splice(this.indiceAtual - 1, 2); //remove  os  dois ultimos
            this.exibePagina(anterior.seletor, anterior.titulo);
        },

        fecharMenuLateral: function() {
            var d = document.querySelector(".mdl-layout");
            if (d.MaterialLayout.drawer_.className.indexOf("is-visible") >= 0) {
                d.MaterialLayout.toggleDrawer();
            }
        },

        init: function() {
            console.log("gerenciadorPagina.js", "init");
            var that = this;
            [].forEach.call(document.querySelectorAll("[data-pagina]"), function(item, indice) {
                item.addEventListener("click", function(e) {
                    e.preventDefault();
                    var seletorPagina = item.getAttribute("data-pagina");
                    var tituloPagina = item.getAttribute("data-pagina-titulo");
                    that.fecharMenuLateral();
                    that.exibePagina(seletorPagina, tituloPagina);
                    return false;
                });
            });
        }
    };

    return gerenciadorPagina;
});
