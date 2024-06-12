const apiUrl = 'http://localhost:4043';
let carPageId;
function createModal(images, index) {
  const modal = document.createElement('div');
  modal.classList.add('modal');
const prevButton = document.createElement('button');
prevButton.textContent = '←';
prevButton.classList.add('prev-button');
prevButton.onclick = () => changeImage(-1);
const nextButton = document.createElement('button');
nextButton.textContent = '→';
nextButton.classList.add('next-button');
nextButton.onclick = () => changeImage(1);
  const modalImage = document.createElement('img');
  modalImage.src = images[index];
  modal.appendChild(prevButton);
  modal.appendChild(modalImage);
  modal.appendChild(nextButton);
  function changeImage(step) {
    index = (index + step + images.length) % images.length;
    modalImage.src = images[index];
  }
  document.body.appendChild(modal);
  modal.addEventListener('click', (event) => {
    if (event.target === modal) {
      document.body.removeChild(modal);
    }
  });
}
async function fetchCarData() {
  try {
    const url = apiUrl + `/car-page-service/CarPage/${carPageId}`;
    const response = await fetch(url);
    const data = await response.json();
    return data;
  } catch (error) {
    console.error('Ошибка при получении данных:', error);
    throw error;
  }
}
function updateCarPage(carData) {
  console.log(carData);
  const mainImageElement = document.querySelector('.car-image');
  mainImageElement.src = carData.mainImage;
  mainImageElement.addEventListener('click', () => {
    const images = [carData.mainImage, ...carData.images];
    createModal(images, 0);
  });
  document.querySelector('.car-details h2').textContent = carData.manufacturer + ' ' + carData.model;
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
document.addEventListener('DOMContentLoaded', async function() {
  const params = new Proxy(new URLSearchParams(window.location.search), {
    get: (searchParams, prop) => searchParams.get(prop),
  });
  carPageId = params.id;
  console.log(carPageId);
  try {
    const carData = await fetchCarData();
    updateCarPage(carData);
  } catch (error) {
    console.error('Произошла ошибка при получении данных об автомобиле:', error);
  }
});
  const form = document.querySelector('form');
  const submitButton = form.querySelector('.submit-btn');
  const fioInput = form.querySelector('input[type="text"]');
  const phoneInput = form.querySelector('input[type="tel"]');
phoneInput.addEventListener('input', function(event) {
  const input = event.target.value.replace(/\D/g, '').substring(0, 11);
  const phoneNumber = input.replace(/(\d{1})(\d{3})(\d{3})(\d{2})(\d{2})/, "+7 (\$2) \$3-\$4-\$5");
  event.target.value = phoneNumber;
});
submitButton.addEventListener('click', function(event) {
  event.preventDefault();
  if (fioInput.value.trim() === '') {
      alert('Пожалуйста, заполните поле "ФИО"');
      return;
  }
  if (phoneInput.value.trim() === '') {
      alert('Пожалуйста, заполните поле "Телефон"');
      return;
  }
  const phoneRegex = /^\+7 \(\d{3}\) \d{3}-\d{2}-\d{2}$/;
  if (!phoneRegex.test(phoneInput.value.trim())) {
      alert('Пожалуйста, введите телефон в формате +7 (XXX) XXX-XX-XX');
      return;
  }
  alert('Спасибо за заказ! Мы скоро свяжемся с вами.');
  sendData(fioInput.value.trim(),phoneInput.value.trim());
  form.reset();
});
async function sendData(fioInput,phoneInput) {
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
  console.log(data);
  } catch (error) {
  console.error('Error:', error);
  }
}
document.querySelectorAll('.car-images img').forEach(image => {
  image.addEventListener('click', () => {
      openModal(image.src);
  });
});
function openModal(src) {
  const modal = document.createElement('div');
  modal.classList.add('modal');
  const modalImage = document.createElement('img');
  modalImage.src = src;
  modal.appendChild(modalImage);
  document.body.appendChild(modal);
  modal.addEventListener('click', () => {
      document.body.removeChild(modal);
  });
}
