define(function(require) {
    "use strict";


    var arquivo = {


        salvar: function(caminhoDestino, pastaDestino, nomeArquivo, txt) {
            pastaDestino = pastaDestino.toLowerCase();
            var that = this;
            return new Promise(function(resolve, reject) {
                window.requestFileSystem(window.LocalFileSystem.PERSISTENT, 0, function(fs) {
                    window.resolveLocalFileSystemURL(caminhoDestino,
                        function(dir) {
                            dir.getDirectory(pastaDestino, {
                                create: true,
                                exclusive: false
                            }, function(fileEntry) {
                                fileEntry.getFile(
                                    nomeArquivo.toLowerCase(), {
                                        create: true,
                                        exclusive: false
                                    },
                                    function(fileEntry) {

                                        fileEntry.createWriter(function(fileWriter) {

                                            fileWriter.onwriteend =
                                                function() {
                                                    console.log(
                                                        "Successful file write..."
                                                    );
                                                    console.dir(fileEntry);
                                                    resolve(fileEntry.toURL());
                                                };

                                            fileWriter.onerror = function(e) {
                                                console.log(
                                                    "Failed file write: " +
                                                    e.toString());
                                                reject(e);
                                            };
                                            fileWriter.write(
                                                txt
                                            );
                                        });
                                    },
                                    function(err) {
                                        console.error(err);
                                        reject(err);
                                    });
                            }, function(err) {
                                console.error(err);
                                reject(err);
                            });
                        },
                        function(err) {
                            console.error(err);
                            reject(err);
                        });
                }, reject);
            });
        },

        lerArquivo: function(caminhoDestino, pastaDestino, nomeArquivo) {
            pastaDestino = pastaDestino.toLowerCase();
            var that = this;
            return new Promise(function(resolve, reject) {
                window.requestFileSystem(window.LocalFileSystem.PERSISTENT, 0, function(fs) {
                    window.resolveLocalFileSystemURL(caminhoDestino,
                        function(dir) {
                            dir.getDirectory(pastaDestino, {
                                create: true,
                                exclusive: false
                            }, function(fileEntry) {
                                fileEntry.getFile(
                                    nomeArquivo.toLowerCase(), {
                                        create: true,
                                        exclusive: false
                                    },
                                    function(fileEntry) {
                                        fileEntry.file(function(file) {
                                            var reader = new FileReader();
                                            reader.onloadend = function(e) {
                                                resolve(this.result);
                                            };
                                            reader.onerror = reject;
                                            reader.readAsText(file);
                                        });
                                    },
                                    function(err) {
                                        console.error(err);
                                        reject(err);
                                    });
                            }, function(err) {
                                console.error(err);
                                reject(err);
                            });
                        },
                        function(err) {
                            console.error(err);
                            reject(err);
                        });
                }, reject);
            });
        }
    };


    return arquivo;

});
