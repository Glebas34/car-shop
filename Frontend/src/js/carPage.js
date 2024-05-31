// URL сервера, с которого нужно получить данные
const apiUrl = 'http://localhost:4043';
// Функция для получения данных с сервера
function createModal(images, index) {
  const modal = document.createElement('div');
  modal.classList.add('modal');

  // Создание кнопок для листания
const prevButton = document.createElement('button');
prevButton.textContent = '←';
prevButton.classList.add('prev-button');
prevButton.onclick = () => changeImage(-1);

const nextButton = document.createElement('button');
nextButton.textContent = '→';
nextButton.classList.add('next-button');
nextButton.onclick = () => changeImage(1);

  // Создание изображения внутри модального окна
  const modalImage = document.createElement('img');
  modalImage.src = images[index];
  modal.appendChild(prevButton);
  modal.appendChild(modalImage);
  modal.appendChild(nextButton);

  // Функция для смены изображения
  function changeImage(step) {
    index = (index + step + images.length) % images.length;
    modalImage.src = images[index];
  }
  
  // Добавление модального окна в body
  document.body.appendChild(modal);

  // Обработчик клика для закрытия модального окна
  modal.addEventListener('click', (event) => {
    if (event.target === modal) {
      document.body.removeChild(modal);
    }
  });
}
// Функция для получения данных с сервера
async function fetchCarData(id) {
  try {
    const url = apiUrl + `/car-page-service/CarPage/${id}`;
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
  const mainImageElement = document.querySelector('.car-image');
  mainImageElement.src = carData.mainImage;
  mainImageElement.addEventListener('click', () => {
    const images = [carData.mainImage, ...carData.images];
    createModal(images, 0);
  });
  document.querySelector('.car-details h2').textContent = carData.manufacturer + ' ' + carData.model;
    // Обновление технических характеристик
    document.querySelector('.car-details').innerHTML += `
        <p>Производитель: ${carData.manufacturer}</p>
        <p>Модель: ${carData.model}</p>
        <p>Цена: ${carData.price.toLocaleString()}₽</p>
        <p>Гарантия: ${carData.warranty}</p>
        <p>Скорость: ${carData.speed}</p>
        <p>Мощность: ${carData.power}</p>
        <p>Разгон до 100 км/ч: ${carData.acceleration}</p>
        <p>Расход топлива: ${carData.fuelConsumption}</p>
        <p>Комплектация: ${carData.package}</p>
        <p>Год выпуска: ${carData.year}</p>
    `;
    // Добавление обработчиков событий к дополнительным изображениям
  const carImagesContainer = document.querySelector('.car-images');
  if (carImagesContainer) {
    carData.images.forEach((image, index) => {
      const imageElement = document.createElement('img');
      imageElement.src = image;
      imageElement.addEventListener('click', () => {
        const images = [carData.mainImage, ...carData.images];
        createModal(images, index + 1);
      });
      carImagesContainer.appendChild(imageElement);
    });
  } else {
    console.error('Элемент с классом .car-images не найден в DOM');
  }
}
// Обработчик событий при загрузке страницы
document.addEventListener('DOMContentLoaded', async function() {
  const params = new Proxy(new URLSearchParams(window.location.search), {
    get: (searchParams, prop) => searchParams.get(prop),
  });
  let carId = params.id;
  console.log(carId);
  try {
    const carData = await fetchCarData(carId);
    updateCarPage(carData);
  } catch (error) {
    console.error('Произошла ошибка при получении данных об автомобиле:', error);
  }
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
  const response = await fetch(url, {
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
//МОДАЛЬНОЕ ОКНО ЛИСТАНИЯ
// Получение всех изображений и добавление обработчика клика
document.querySelectorAll('.car-images img').forEach(image => {
  image.addEventListener('click', () => {
      openModal(image.src); // Функция открытия модального окна
  });
});

// Функция для открытия модального окна
function openModal(src) {
  const modal = document.createElement('div');
  modal.classList.add('modal');

  // Создание изображения внутри модального окна
  const modalImage = document.createElement('img');
  modalImage.src = src;
  modal.appendChild(modalImage);

  // Добавление модального окна в body
  document.body.appendChild(modal);

  // Обработчик клика для закрытия модального окна
  modal.addEventListener('click', () => {
      document.body.removeChild(modal);
  });
}
