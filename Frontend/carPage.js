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
    <p>Гарантия: ${carData.warranty}</p>
    <p>Скорость: ${carData.speed} км/ч</p>
    <p>Мощность: ${carData.power} л.с.</p>
    <p>Разгон до 100 км/ч: ${carData.acceleration} сек</p>
    <p>Расход топлива: ${carData.fuelConsumption} л/100км</p>
    <p>Комплектация: ${carData.package}</p>
    <p>Год выпуска: ${carData.year}</p>
`;
// Обновление остальных фотографий
const imageGallery = document.querySelector('.image-gallery');
carData.imageGallery.forEach(image => {
    const img = document.createElement('img');
    img.src = image;
    img.classList.add('gallery-image');
    imageGallery.appendChild(img);
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
/*const sendData = async () => {
    try {
    const response = await fetch('https://example.com/submit-application', {
    method: 'POST',
    headers: {
    'Content-Type': 'application/json'
    },
    body: JSON.stringify({
    manufacturer: carData.manufacturer,
    model: carData.model,
    price: carData.price,
    warranty: carData.warranty,
    speed: carData.speed,
    power: carData.power,
    acceleration: carData.acceleration,
    fuelConsumption: carData.fuelConsumption,
    package: carData.package,
    year: carData.year
    })
    });
    const data = await response.json();
    console.log(data); // Response from the server
    } catch (error) {
    console.error('Error:', error);
    }
    }
    
    sendData();*/
    
/*const carData = {
  manufacturer: "Toyota",
  model: "Camry",
  price: 2000000,
  warranty: "3 years",
  speed: 200,
  power: 180,
  acceleration: 8,
  fuelConsumption: 8,
  package: "Premium",
  year: 2021
};

fetch('https://example.com/submitApplication', {
  method: 'POST',
  headers: {
    'Content-Type': 'application/json'
  },
  body: JSON.stringify(carData)
})
  .then(response => response.json())
  .then(data => {
    console.log('Application submitted:', data);
  })
  .catch(error => {
    console.error('Error submitting application:', error);
  });*/