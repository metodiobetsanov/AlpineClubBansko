// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
$('#goTop').click(function () {
    $('body,html').animate({
        scrollTop: 0
    }, 500);
});