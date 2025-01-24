// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


function voltarPagina() {
    if (window.history.length > 1) {
        history.back();
    } else {
        alert("Não há uma página anterior no histórico!");
    }
}

// Ocultar o botão se não houver histórico
window.onload = function () {
    const botaoVoltar = document.getElementById('botao-voltar');
    if (window.history.length <= 1) {
        botaoVoltar.style.display = 'none';
    }
};
