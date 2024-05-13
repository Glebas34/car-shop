// Функция для получения данных об автомобиле
function fetchCarData() {
    fetch('URL_сервера/api/car') // Замените URL_сервера на фактический URL вашего API
        .then(response => response.json())
        .then(data => {
            updateCarPage(data);
        })
        .catch(error => console.error('Ошибка при получении данных:', error));
}

// Функция для обновления страницы данными об автомобиле
function updateCarPage(carData) {
    // Обновление названия автомобиля и фотографии
    document.querySelector('.car-image').src = carData.imageUrl;
    document.querySelector('.car-details h2').textContent = carData.name;
    // Обновление технических характеристик
    document.querySelector('.car-details').innerHTML += `
        <p>Производитель: ${carData.manufacturer}</p>
        <p>Модель: ${carData.model}</p>
        <p>Цена: ${carData.price.toLocaleString()}₽</p>
        <p>Цвет: ${carData.color}</p>
        <p>Гарантия: ${carData.warranty}</p>
        <p>Скорость: ${carData.speed} км/ч</p>
        <p>Мощность: ${carData.power} л.с.</p>
        <p>Разгон до 100 км/ч: ${carData.acceleration} сек</p>
        <p>Расход топлива: ${carData.fuelConsumption} л/100км</p>
        <p>Комплектация: ${carData.package}</p>
        <p>Год выпуска: ${carData.year}</p>
    `;
    // Обновление селектора цветов
    const colorSelector = document.getElementById('color-selector');
    carData.availableColors.forEach(color => {
        const option = document.createElement('option');
        option.value = color;
        option.textContent = color;
        colorSelector.appendChild(option);
    });
}

// Обработчик событий при загрузке страницы
document.addEventListener('DOMContentLoaded', function() {
    // Вызов функции для получения данных об автомобиле
    fetchCarData();

    // Получение элементов формы
    const form = document.querySelector('form');
    const submitButton = form.querySelector('.submit-btn');
    const fioInput = form.querySelector('input[type="text"]');
    const phoneInput = form.querySelector('input[type="tel"]');

    // Обработчик клика по кнопке отправки формы
    submitButton.addEventListener('click', function(event) {
        event.preventDefault();
        // Проверка заполнения полей
        if (fioInput.value.trim() === '') {
            alert('Пожалуйста, заполните поле "ФИО"');
            return;
        }
        if (phoneInput.value.trim() === '') {
            alert('Пожалуйста, заполните поле "Телефон"');
            return;
        }
        // Проверка формата телефона
        const phoneRegex = /^\+7 \(\d{3}\) \d{3}-\d{2}-\d{2}$/;
        if (!phoneRegex.test(phoneInput.value.trim())) {
            alert('Пожалуйста, введите телефон в формате +7 (XXX) XXX-XX-XX');
            return;
        }
        // Если все данные введены корректно
        alert('Спасибо за заказ! Мы скоро свяжемся с вами.');
        form.reset();
    });

    // Обработчик ввода телефонного номера
    phoneInput.addEventListener('input', function(event) {
        const input = event.target.value.replace(/\D/g, '').substring(0, 11);
        const phoneNumber = input.replace(/(\d{1})(\d{3})(\d{3})(\d{2})(\d{2})/, "+7 (\$2) \$3-\$4-\$5");
        event.target.value = phoneNumber;
    });
});
