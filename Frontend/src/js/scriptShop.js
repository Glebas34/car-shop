let cards;
var carModels = {};
async function init() {
  cards = await fetchData(30000000);
  for(let i = 0; i<cards.length(); i++)
    {
      if (!(cards[i].manufacturer in carModels)){
        carModels[cards[i].manufacturer] = [];
      }
      carModels[cards[i].manufacturer].push(cards[i].model);
    }
  // Дальнейшие действия с переменной 'cards'
}
init();
const apiUrl = 'https://localhost:5053';
// Объект с моделями автомобилей по маркам
// Функция для заполнения моделей автомобилей в зависимости от выбранной марки
function populateModels() {
  const brandSelect = document.getElementById('carBrand');
  const modelSelect = document.getElementById('carModel');
  const selectedBrand = brandSelect.value;
  modelSelect.innerHTML = '';
  carModels[selectedBrand].forEach(model => {
    const option = document.createElement('option');
    option.text = model;
    option.value = model;
    modelSelect.add(option); 
  });
}
// Функция для обновления отображаемой цены при изменении ползунка
function updatePriceValue(value) {
  const formattedValue = new Intl.NumberFormat('ru-RU', { minimumFractionDigits: 0 }).format(value);
  document.getElementById('priceValue').textContent = formattedValue + ' ₽';
}
// Функция для фильтрации автомобилей
function searchCars() {
  const selectedBrand = document.getElementById('carBrand').value;
  const selectedModel = document.getElementById('carModel').value;
  const maxPrice = +document.getElementById('priceRange').value;
  cards = fetchData(maxPrice,selectedBrand,selectedModel);
  displayProducts();
}
// Делаем элемент carModel недоступным при загрузке страницы
selectedModel.disabled = true;
// Обработчик события на выбор элемента в carBrand
selectedBrand.addEventListener('change', function() {
  const selectedBrand = document.getElementById('carBrand').value;
    if (selectedBrand !== '') {
      selectedModel = carModels[selectedBrand];
      selectedModel.disabled = false; // Разблокируем элемент carModel
    } else {
      selectedModel.disabled = true; // Блокируем элемент carModel
    }
});
// Функция для получения данных с сервера
async function fetchData(price,brand='',model='') {
  try {
    const url = `http://localhost:4043/card-service/Card?manufactorer=${brand}&model=${model}&price=${price}`;
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
  const container = document.getElementById('car-container');
  container.innerHTML = cards.map(renderProductCard).join('');
}
document.addEventListener('DOMContentLoaded', function() {
displayProducts();
});
function renderOption(value) {
  const renderHTML =`
  <option>${value}</>
  `;
  return renderHTML;
}
function displayOptions() {
  let keys = Object.keys(carModels);
  const container = document.getElementById('select-style');
  container.innerHTML = keys.map(renderProductCard).join('');
}
displayOptions();
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
                  <p>Цена: ${productPrice}</p>
              </a>
              <div class="card__button">
                  <button class="order-button" onclick="event.stopPropagation(); openOrderModal();">Заказать</button>
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
document.getElementById('priceRange').addEventListener('input', function() {
  updatePriceValue(this.value);
  searchCars();
});
populateModels();
updatePriceValue(document.getElementById('priceRange').value);
//Модальное окно формы заказа
function openOrderModal() {
  document.getElementById('orderModal').style.display = 'block';
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
  const form = document.querySelector('#orderForm');
  const button = form.querySelector('input[type="submit"]');

  button.addEventListener('click', function(event) {
      event.preventDefault();
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
      alert('Мы вам перезвоним');
      form.reset();
  }); 
  const phoneInput = form.querySelector('input[name="phone"]');
  phoneInput.addEventListener('input', function(event) {
      const input = event.target.value.replace(/\D/g, '').substring(0, 11);
      const phoneNumber = input.replace(/(\d{1})(\d{3})(\d{3})(\d{2})(\d{2})/, "+7 (\$2) \$3-\$4-\$5");
      event.target.value = phoneNumber;
  });
});
