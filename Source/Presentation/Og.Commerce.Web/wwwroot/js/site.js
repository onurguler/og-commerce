// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(function () {
    var storageName = "themeMode";
    var theme = localStorage.getItem(storageName) || "light";
    setTheme(theme);

    $(".hide-theme-dark").click(function () {
        setTheme("dark");
    });

    $(".hide-theme-light").click(function () {
        setTheme("light");
    });

    function setTheme(t) {
        localStorage.setItem(storageName, t);
        document.body.classList.remove("theme-dark", "theme-light");
        document.body.classList.add("theme-".concat(t));
    }
});