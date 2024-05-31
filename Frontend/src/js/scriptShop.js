let cards = [];
var carModels = {};
async function init() {
  try {
    cards = await (await fetch(`http://localhost:4043/card-service/Card`)).json();
    if (cards.length === 0) {
      console.error('Массив cards пуст');
      return;
    }
    console.log(cards);
    for (let i = 0; i < cards.length; i++) {
      if (!(cards[i].manufacturer in carModels)) {
        carModels[cards[i].manufacturer] = [];
      }
      carModels[cards[i].manufacturer].push(cards[i].model);
    }
  } catch (error) {
    console.error('Ошибка в функции init:', error);
  }
}
// Вызовите init после определения всех функций, чтобы убедиться, что все они доступны
document.addEventListener('DOMContentLoaded', function() {
  init().then(() => {
    displayProducts();
    displayOptions();
  });
});
// Проверяем, существует ли марка в объекте carModels и является ли она массивом
function populateModels() {
  const brandSelect = document.getElementById('carBrand');
  const modelSelect = document.getElementById('carModel');
  const selectedBrand = brandSelect.value;
  modelSelect.innerHTML = '<option selected disabled>Выберите модель</option>';
  if (carModels[selectedBrand] && Array.isArray(carModels[selectedBrand])) {
    carModels[selectedBrand].forEach(model => {
      const option = document.createElement('option');
      option.text = model;
      option.value = model;
      modelSelect.add(option); 
    });
  } else {
    console.error('Selected brand is not available or not an array');
  }
}
// Функция для обновления отображаемой цены при изменении ползунка
function updatePriceValue(value) {
  const formattedValue = new Intl.NumberFormat('ru-RU', { minimumFractionDigits: 0 }).format(value);
  document.getElementById('priceValue').textContent = formattedValue + ' ₽';
}
// Функция для фильтрации автомобилей
async function searchCars() {
  const selectedBrand = document.getElementById('carBrand').value;
  const selectedModel = document.getElementById('carModel').value;
  const maxPrice = +document.getElementById('priceRange').value;
  cards = await fetchData(maxPrice,selectedBrand,selectedModel);
  displayProducts();
}
// Получаем элемент carBrand и сохраняем его в переменной selectedBrand
const selectedBrand = document.getElementById('carBrand');
// Получаем элемент carModel и сохраняем его в переменной selectedModel
const selectedModel = document.getElementById('carModel');
// Изначально блокируем элемент carModel
selectedModel.disabled = true;
// Обработчик события на выбор элемента в carBrand
selectedBrand.addEventListener('change', function() {
  // Получаем выбранное значение из selectedBrand
  const brandValue = selectedBrand.value;
  if (brandValue !== '') {
    // Получаем модели для выбранного бренда
    const models = carModels[brandValue];
    // Убедитесь, что selectedModel ссылается на DOM-элемент, а не на массив
    selectedModel.disabled = false;
    // Здесь вы можете обновить список моделей в selectedModel, например:
    // selectedModel.innerHTML = models.map(model => `<option value="${model}">${model}</option>`).join('');
  } else {
    // Блокируем элемент carModel, если бренд не выбран
    selectedModel.disabled = true;
  }
});
// Функция для получения данных с сервера
async function fetchData(price,brand='',model='') {
  try {
    const url = `http://localhost:4043/card-service/Card/Filtered?maxPrice=${price}&manufacturer=${brand}&model=${model}`;
    const response = await fetch(url);
    const data = await response.json();
    console.log('Данные получены:', data);
    return data;
  } catch (error) {
    console.error('Ошибка при получении данных:', error);
    throw error;
  }
}
// Функция для отображения всех карточек товаров
function displayProducts() {
  if (Array.isArray(cards)) {
    const container = document.getElementById('car-container');
    container.innerHTML = cards.map(renderProductCard).join('');
  } else {
    console.error('Cards is not defined or not an array');
  }
}
function renderOption(value) {
  const renderHTML =`
  <option>${value}</option>
  `;
  return renderHTML;
}
function displayOptions() {
  let keys = Object.keys(carModels);
  const container = document.getElementById('carBrand');
  container.innerHTML = '<option selected disabled>Выберите бренд</option>' + keys.map(renderOption).join('');
}
// Функция для отображения данных на странице
function renderProductCard(productData) {
  try {
      const productManufacturer = productData.manufacturer;
      const productModel = productData.model;
      const productColor = productData.color;
      const productPackage = productData.package;
      const productYear = productData.year;
      const productImage = productData.image;
      const productPrice = productData.price;
      const productCardHTML = `
          <div class="product-card">
              <a href="carPage.html?id=${productData.carPageId}">
                  <h2>${productManufacturer + ' ' + productModel + ' ' + productYear}</h2>
                  <img src="${productImage}"></img>
                  <p>Комплектация: ${productPackage}</p>
                  <p>Цвет: ${productColor}</p>
                  <p>Цена: ${productPrice}₽</p>
              </a>
              <div class="card__button">
                  <button class="order-button" onclick="openOrderModal('${productData.carPageId}');">Заказать</button>
              </div>
          </div>
      `;
      return productCardHTML;
  } catch (error) {
      console.error('Ошибка при отображении карточки товара:', error);
      return null;
  }
}
// Добавление обработчиков событий
document.getElementById('carBrand').addEventListener('change', populateModels);
document.getElementById('priceRange').addEventListener('input', async function() {
  updatePriceValue(this.value);
  await searchCars();
});
populateModels();
updatePriceValue(document.getElementById('priceRange').value);
//Модальное окно формы заказа
function openOrderModal(carPageId) {
  var modal = document.getElementById('orderModal');
  modal.style.display = 'block';
  var button = document.createElement('button');
  button.setAttribute('type', 'submit');
  button.setAttribute('value', 'Отправить');
  button.setAttribute('onclick',`Submit('${carPageId}')`);
  button.setAttribute('class','modal-button');
  modal.appendChild(button);
}
async function Submit(carPageId) {
  const form = document.getElementById('orderForm');
  const fioInput = form.querySelector('input[name="fio"]');
  const phoneInput = form.querySelector('input[name="phone"]');
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
  await sendData(fioInput, phoneInput, carPageId);
  alert('Мы вам перезвоним');
  form.reset();
}
function closeModal() {
  document.getElementById('orderModal').style.display = 'none';
}
document.getElementById('orderForm').onsubmit = function(event) {
  event.preventDefault();
  alert('Спасибо за заказ! Мы скоро свяжемся с вами.');
  closeModal();
  document.getElementById('orderForm').reset();
};
document.addEventListener("DOMContentLoaded", function() {
  const form = document.getElementById('orderForm');
  const phoneInput = form.querySelector('input[name="phone"]');
  phoneInput.addEventListener('input', function(event) {
      const input = event.target.value.replace(/\D/g, '').substring(0, 11);
      const phoneNumber = input.replace(/(\d{1})(\d{3})(\d{3})(\d{2})(\d{2})/, "+7 (\$2) \$3-\$4-\$5");
      event.target.value = phoneNumber;
  });
});
async function sendData(fioInput, phoneInput, carPageId) {
  try {
    const apiUrl = 'http://localhost:4043'; // Укажите здесь ваш базовый URL
    const url = apiUrl + '/requisition-service/Requisition';
    const response = await fetch(url, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({
        fullName: fioInput.value,
        phoneNumber: phoneInput.value,
        carPageId: carPageId // Убедитесь, что carPageId получен корректно
      }) 
    });
    const data = await response.json();
    console.log(data); // Response from the server
  } catch (error) {
    console.error('Error:', error);
  }
}
