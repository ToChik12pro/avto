
document.addEventListener('DOMContentLoaded', function () {
    var logo = document.getElementById('logo');
    var sideMenu = document.getElementById('sideMenu');
    var closeSideMenu = document.getElementById('closeSideMenu');
    var modelsLink = document.getElementById('modelsLink');
    var modelsSection = document.getElementById('modelsSection');
    var closeModelsSection = document.getElementById('closeModelsSection');

    logo.addEventListener('click', function () {
        sideMenu.style.left = '0';
    });

    closeSideMenu.addEventListener('click', function () {
        sideMenu.style.left = '-250px';
    });

    modelsLink.addEventListener('click', function (event) {
        event.preventDefault(); // Предотвратить переход по ссылке
        modelsSection.style.right = '0'; // Показать секцию моделей
    });

    closeModelsSection.addEventListener('click', function () {
        modelsSection.style.right = '-100%'; // Скрыть секцию моделей
    });

    window.addEventListener('click', function (event) {
        if (!sideMenu.contains(event.target) && !logo.contains(event.target) && !modelsSection.contains(event.target) && !modelsLink.contains(event.target)) {
            sideMenu.style.left = '-250px';
            modelsSection.style.right = '-100%'; // Скрыть секцию моделей и боковое меню при клике вне их области
        }
    });
});