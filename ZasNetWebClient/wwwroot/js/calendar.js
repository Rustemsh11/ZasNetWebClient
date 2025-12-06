window.scrollToHour = (element, scrollPercent) => {
    if (element && element.scrollWidth) {
        const scrollPosition = (element.scrollWidth * scrollPercent) / 100;
        element.scrollLeft = scrollPosition;
    }
};

// Функция для прокрутки к определенному часу при загрузке
window.scrollToHourOnLoad = () => {
    const scrollableElements = document.querySelectorAll('.calendar-timeline');
    scrollableElements.forEach(element => {
        if (element && element.scrollWidth) {
            // Прокручиваем к 8 часу (32% от общей ширины для 25 столбцов: 0-24)
            const scrollPercent = (8 / 25.0) * 100;
            const scrollPosition = (element.scrollWidth * scrollPercent) / 100;
            element.scrollLeft = scrollPosition;
        }
    });
};

// Выполняем прокрутку после загрузки DOM
if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', () => {
        setTimeout(window.scrollToHourOnLoad, 300);
    });
} else {
    setTimeout(window.scrollToHourOnLoad, 300);
}

