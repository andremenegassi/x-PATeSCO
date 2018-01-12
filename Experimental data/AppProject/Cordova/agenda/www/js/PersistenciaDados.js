define(function(require) {
    "use strict";
    var arquivo = require("./arquivo");
    var _diretorio = "app-agenda";

    var PersistenciaDados = function(c) {
        this.key = c.key;
    };

    var _plat = {
        ios: false,
        android: false,
        navegador: false
    };

    PersistenciaDados.prototype = {
        constructor: PersistenciaDados,

        getInitalState: function() {
            return {
                seq: 0,
                list: []
            };
        },

        dados: null,

        load: function() {
            var that = this;
            return new Promise(function(resolve, reject) {
                try {
                    if (!_plat.navegador) {
                        var caminhoDestino = window.cordova.file.externalRootDirectory;
                        if (_plat.ios) {
                            caminhoDestino = window.cordova.file.documentsDirectory;
                        }
                        var pastaDestino = _diretorio;
                        var nomeArquivo = this.key;
                        arquivo.lerArquivo(
                            caminhoDestino,
                            pastaDestino,
                            nomeArquivo).then(function(txt) {
                            if (!txt) {
                                that.dados = that.getInitalState();
                                resolve(that.dados);
                                return;
                            }
                            that.dados = JSON.parse(txt);
                            resolve(that.dados);
                        }).catch(reject);
                        return;
                    }
                    //==============navegador==================//
                    if (!window.localStorage[this.key]) {
                        this.dados = this.getInitalState();
                        resolve(this.dados);
                        return;
                    }
                    this.dados = JSON.parse(window.localStorage[this.key]);
                    resolve(this.dados);
                } catch (err) {
                    reject(err);
                }
            }.bind(this));
        },

        getCaminho: function(){
            var caminhoDestino = window.cordova.file.externalRootDirectory;
            if (_plat.ios) {
                caminhoDestino = window.cordova.file.documentsDirectory;
            }
            return caminhoDestino;
        },

        persiste: function() {
            return new Promise(function(resolve, reject) {
                if (!_plat.navegador) {
                    var caminhoDestino =  this.getCaminho();
                    var pastaDestino = _diretorio;
                    var nomeArquivo = this.key;
                    var txt = JSON.stringify(this.dados);
                    arquivo.salvar(caminhoDestino, pastaDestino, nomeArquivo, txt).then(function(){
                        resolve();
                    }).catch(reject);
                    return;
                }
                //navegador
                try {
                    window.localStorage.setItem(this.key, JSON.stringify(this.dados));
                    resolve();
                } catch (err) {
                    reject(err);
                }
            }.bind(this));
        },

        getSeqId: function() {
            return this.dados.seq + 1;
        },

        addItem: function(obj) {
            obj.id = this.getSeqId();
            this.dados.list.push(obj);
            this.dados.seq = obj.id;
            var that = this;
            return new Promise(function(resolve, reject) {
                that.persiste().then(function() {
                    resolve(obj.id);
                }).catch(reject);
            });
        },

        getIndiceById: function(id) {
            for (var i = 0; i < this.dados.list.length; i++) {
                if (this.dados.list[i].id === id) {
                    return i;
                }
            }
            return -1;
        },

        getItemById: function(id) {
            var that = this;
            return new Promise(function(resolve, reject) {
                var indice = that.getIndiceById(id);
                if (indice === -1) {
                    reject(new Error("Não encontrou id: " + id));
                    return;
                }
                resolve(that.dados.list[indice]);
            });
        },

        atualizarItem: function(obj) {
            console.log(obj);
            var that = this;
            return new Promise(function(resolve, reject) {
                var indice = that.getIndiceById(obj.id);
                if (indice === -1) {
                    reject(new Error("Não encontrou id: " + obj.id));
                    return;
                }
                that.dados.list[indice] = obj;
                that.persiste().then(resolve).catch(reject);
            });
        },

        removerItem: function(id) {
            var that = this;
            return new Promise(function(resolve, reject) {
                var indice = that.getIndiceById(id);
                if (indice === -1) {
                    reject(new Error("Não encontrou id: " + id));
                    return;
                }
                that.dados.list.splice(indice, 1);
                that.persiste().then(resolve).catch(reject);
            });
        },

        buscaItem: function(prop, query) {
            var listResultados = [];
            return new Promise(function(resolve, reject) {
                if(!query){
                    resolve(this.dados.list);
                    return;
                }
                this.dados.list.forEach(function(item) {
                    if (item[prop].toLowerCase().indexOf(query.toLowerCase()) >= 0) {
                        listResultados.push(item);
                    }
                });
                resolve(listResultados);
            }.bind(this));
        },

        getList: function() {
            return this.dados.list;
        },

        init: function() {
            var plat = window.device && window.device.platform.toLowerCase() || "";
            if (plat.indexOf("android") >= 0) {
                _plat.android = true;
            } else if (plat.indexOf("ios") >= 0) {
                _plat.android = true;
            } else {
                _plat.navegador = true;
            }

            console.log("persistenciaDados:" + this.key, "init");
            this.load().then(function() {
                console.log("persistenciaDados", "load");
            }).catch(function(e) {
                throw e;
            });
        }
    };

    return PersistenciaDados;
});
