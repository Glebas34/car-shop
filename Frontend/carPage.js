// URL сервера, с которого нужно получить данные
const apiUrl = 'https://localhost:5053';
// Функция для получения данных с сервера
async function fetchCarData() {
  try {
    const url = apiUrl + '/car-page-service/CarPage/${Id}';
    const response = await fetch(url);
    const data = await response.json();
    return data;
  } catch (error) {
    console.error('Ошибка при получении данных:', error);
    throw error;
  }
}
// Функция для обновления страницы данными об автомобиле
function updateCarPage(carData) {
    // Обновление названия автомобиля и фотографии
    document.querySelector('.car-image').src = carData.mainImage;
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
    // Вставка всех изображений из списка Images
    carData.Images.forEach(image => {
        const imageElement = document.createElement('img');
        imageElement.src = image;
        document.querySelector('.car-images').appendChild(imageElement);
    });
}
// Обработчик событий при загрузке страницы
document.addEventListener('DOMContentLoaded', function() {
  const params = new Proxy(new URLSearchParams(window.location.search), {
    get: (searchParams, prop) => searchParams.get(prop),
  });
  let carId = params.id;
  const carData = fetchCarData(carId);
  updateCarPage(carData);
});
  const form = document.querySelector('form');
  const submitButton = form.querySelector('.submit-btn');
  const fioInput = form.querySelector('input[type="text"]');
  const phoneInput = form.querySelector('input[type="tel"]');
// Обработчик ввода телефонного номера
phoneInput.addEventListener('input', function(event) {
  const input = event.target.value.replace(/\D/g, '').substring(0, 11);
  const phoneNumber = input.replace(/(\d{1})(\d{3})(\d{3})(\d{2})(\d{2})/, "+7 (\$2) \$3-\$4-\$5");
  event.target.value = phoneNumber;
});
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
        sendData(fioInput.value.trim(),phoneInput.value.trim(),carData.id);
        form.reset();
    });
async function sendData(fioInput,phoneInput,carPageId ) {
    try {
    const url =  apiUrl + '/requisition-service/Requisition';
    const response = await fetch('https://example.com/submit-application', {
    method: 'POST',
    headers: {
    'Content-Type': 'application/json'
    },
    body: JSON.stringify({
    fullName: fioInput,
    phoneNumber: phoneInput,
    carPageId: carPageId,
    }) 
    });
    const data = await response.json();
    console.log(data); // Response from the server
    } catch (error) {
    console.error('Error:', error);
    }
    }