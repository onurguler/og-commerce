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

// Select lists
$(function () {
    window.TomSelect && $(".select-tags").length && (new TomSelect(".select-tags", {
        maxOptions: Number.MAX_SAFE_INTEGER,
        copyClassesToDropdown: false,
        dropdownClass: 'dropdown-menu ts-dropdown',
        optionClass: 'dropdown-item',
        controlInput: '<input>',
        render: {
            item: function (data, escape) {
                if (data.customProperties) {
                    return '<div><span class="dropdown-item-indicator">' + data.customProperties + '</span>' + escape(data.text) + '</div>';
                }
                return '<div>' + escape(data.text) + '</div>';
            },
            option: function (data, escape) {
                if (data.customProperties) {
                    return '<div><span class="dropdown-item-indicator">' + data.customProperties + '</span>' + escape(data.text) + '</div>';
                }
                return '<div>' + escape(data.text) + '</div>';
            },
        },
    }));
});