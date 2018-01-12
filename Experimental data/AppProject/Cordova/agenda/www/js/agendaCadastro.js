define(function(require) {
    "use  strict";
    var gerenciadorPagina = require("./gerenciadorPagina");
    var agendaDados = require("./agendaDados");
    var _objCadastro = {}


    var agendaCadastro = {
        getInitalState: function() {
            return {
                id: "",
                foto: "./img/no-photo.png",
                nome: "",
                email: "",
                telefone: ""
            };
        },

        aoRealizarAcaoItem: function() {
            //definido  na pesquisa
        },

        carregarParaEditar: function(id, aoRealizarAcaoItem) {
            this.aoRealizarAcaoItem = aoRealizarAcaoItem;
            agendaDados.getItemById(id).then(function(item) {
                this.render(item);
            }.bind(this)).catch(function(err) {
                alert("Erro: " + err.message);
                console.error(err);
            });
        },

        onGravar: function() {
            this.carregarDados();
            if (!_objCadastro.nome ||
                !_objCadastro.email ||
                !_objCadastro.telefone) {
                alert("Informe todos os campos");
                return;
            }
            if (_objCadastro.id) {
                //alteracao
                console.log("alteracao");
                agendaDados.atualizarItem(_objCadastro).then(function() {
                    alert("cadastrado atualizado com sucesso");
                    this.onCancelar();
                    this.aoRealizarAcaoItem("atualizou");
                }.bind(this)).catch(function(err) {
                    alert("Erro ao alterar: " + err.message);
                });

                return;
            }
            agendaDados.addItem(_objCadastro).then(function(id) {
                alert("cadastrado com sucesso id: " + id);
                this.render(this.getInitalState());
            }.bind(this)).catch(function(err) {
                alert(err.message || "Erro ao gravar");
            });
        },

        onCancelar: function() {
            if (_objCadastro.id) {
                //voltar para o pesquisa
                gerenciadorPagina.voltarHistorico();
            } else {
                gerenciadorPagina.exibePagina("#pgHome");
            }
            this.render(this.getInitalState());
        },

        onExcluir: function() {
            if (window.confirm("Deseja Remover?")) {
                agendaDados.removerItem(_objCadastro.id).then(function() {
                    alert("Removido com sucesso");
                    this.onCancelar();
                    this.aoRealizarAcaoItem("removeu");
                }.bind(this)).catch(function(err) {
                    alert(err.message || "Erro ao excluir");
                });
            }
        },

        getFoto: function(sourceType) {
            var config = {
                allowEdit: true,
                quality: 70,
                encodingType: Camera.EncodingType.PNG,
                destinationType: Camera.DestinationType.DATA_URL,
                sourceType: sourceType,
                targetWidth: 250,
                targetHeight: 500,
                mediaType: Camera.MediaType.PICTURE
            };
            navigator.camera.getPicture(function(imageData) {
                imageData = "data:image/png;base64," + imageData;
                document.querySelector("#imgCadastro").src = imageData;
            }, function() {
                alert("Imagem não foi selecionada");
            }, config);
        },

        isPluginCamera: function() {
            if (!window.Camera) {
                alert("Plugin camera nao carregado!!");
                return false;
            }
            return true;
        },

        onSelecionarFoto: function() {
            if (this.isPluginCamera()) {
                this.getFoto(Camera.PictureSourceType.PHOTOLIBRARY); //SAVEDPHOTOALBUM;
            }
        },

        onTirarFoto: function() {
            if (this.isPluginCamera()) {
                this.getFoto(Camera.PictureSourceType.CAMERA);
            }
        },

        carregarDados: function() {
            _objCadastro = {
                id: document.querySelector("#hfId").value.trim(),
                foto: document.querySelector("#imgCadastro").src,
                nome: document.querySelector("#txtNome").value.trim(),
                email: document.querySelector("#txtEmail").value.trim(),
                telefone: document.querySelector("#txtTelefone").value.trim()
            };
            if (_objCadastro.id) {
                _objCadastro.id = +_objCadastro.id;
            }
        },

        render: function(objCadastro) {
            _objCadastro = objCadastro;
            document.querySelector("#hfId").value = objCadastro.id;
            document.querySelector("#imgCadastro").src = objCadastro.foto;
            document.querySelector("#txtNome").value = objCadastro.nome;
            document.querySelector("#txtEmail").value = objCadastro.email;
            document.querySelector("#txtTelefone").value = objCadastro.telefone;
            document.querySelector("#btnExcluir").disabled = !objCadastro.id;


            //gambiarra para os inputs do material design voltar ao estado correto
            window.setTimeout(function() {
                [
                    document.querySelector("#txtNome"),
                    document.querySelector("#txtEmail"),
                    document.querySelector("#txtTelefone")
                ].forEach(function(item) {
                    item.parentNode.MaterialTextfield.checkDirty();
                    if (!_objCadastro.id) {
                        //no novo cadastro precisa do change
                        item.parentNode.MaterialTextfield.change();
                    }
                });
            }, 10);
        },

        init: function() {
            console.log("agendaCadastro.js", "init");

            _objCadastro = this.getInitalState();

            [
                document.querySelector("form"),
                document.querySelector("#txtNome"),
                document.querySelector("#txtEmail"),
                document.querySelector("#txtTelefone")
            ].forEach(function(item) {
                item.addEventListener("keypress", function(e) {
                    var code = e.keyCode || e.which;
                    if (code === 13) {
                        e.preventDefault();
                        return false;
                    }
                });
            });
            document.querySelector("#btnExcluir").addEventListener("click", function(e) {
                e.preventDefault();
                this.onExcluir();
                return false;
            }.bind(this));
            document.querySelector("#btnCancelar").addEventListener("click", function(e) {
                e.preventDefault();
                this.onCancelar();
                return false;
            }.bind(this));
            document.querySelector("#btnGravar").addEventListener("click", function(e) {
                e.preventDefault();
                this.onGravar();
                return false;
            }.bind(this));
            document.querySelector("#btnTirarFoto").addEventListener("click", function(e) {
                e.preventDefault();
                this.onTirarFoto();
                return false;
            }.bind(this));
            document.querySelector("#btnSelecionarFoto").addEventListener("click", function(e) {
                e.preventDefault();
                this.onSelecionarFoto();
                return false;
            }.bind(this));
            this.render(_objCadastro);
        }
    };


    return agendaCadastro;
});
