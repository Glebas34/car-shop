let cards = [];
var carModels = {};
document.addEventListener('DOMContentLoaded', function() {
  init().then(() => {
    displayProducts();
    displayOptions();
    document.getElementById('carBrand').addEventListener('change', populateModels);
    document.getElementById('priceRange').addEventListener('input', async function() {
      updatePriceValue(this.value);
    });
    populateModels();
    updatePriceValue(document.getElementById('priceRange').value);
  });
});
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
function populateModels() {
  const brandSelect = document.getElementById('carBrand');
  const modelSelect = document.getElementById('carModel');
  const selectedBrand = brandSelect.value;
  modelSelect.innerHTML = '<option selected>Любая модель</option>';
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
function updatePriceValue(value) {
  const formattedValue = new Intl.NumberFormat('ru-RU', { minimumFractionDigits: 0 }).format(value);
  document.getElementById('priceValue').textContent = formattedValue + ' ₽';
}
async function searchCars() {
  let selectedBrand = document.getElementById('carBrand').value;
  let selectedModel = document.getElementById('carModel').value;
   if(selectedBrand === "Любой бренд"){
    selectedBrand = "";
   }
   if (selectedModel=== "Любая модель"){
    selectedModel = "";
  }
  const maxPrice = +document.getElementById('priceRange').value;
  cards = await fetchData(maxPrice,selectedBrand,selectedModel);
  displayProducts();
}
const selectedBrand = document.getElementById('carBrand');
const selectedModel = document.getElementById('carModel');
selectedModel.disabled = true;
selectedBrand.addEventListener('change', function() {
  const brandValue = selectedBrand.value;
  if (brandValue !== '') {
    const models = carModels[brandValue];
    selectedModel.disabled = false;
  } else {
    selectedModel.disabled = true;
  }
});
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
  container.innerHTML = '<option selected>Любой бренд</option>' + keys.map(renderOption).join('');
}
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
populateModels();
updatePriceValue(document.getElementById('priceRange').value);
function openOrderModal(carPageId) {
  var modal = document.getElementById('orderModal');
  modal.style.display = 'block';
  var button = document.getElementById('submitBtn');
    button.setAttribute('onclick',`Submit('${carPageId}')`);
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
    const apiUrl = 'http://localhost:4043';
    const url = apiUrl + '/requisition-service/Requisition';
    const response = await fetch(url, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({
        fullName: fioInput.value,
        phoneNumber: phoneInput.value,
        carPageId: carPageId
      }) 
    });
    const data = await response.json();
    console.log(data);
  } catch (error) {
    console.error('Error:', error);
  }
}
