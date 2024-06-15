let carPages = [];
async function init() {
    try {
      const response = await fetch(`http://localhost:4043/car-page-service/CarPage`);
      carPages = await response.json();
      if (carPages.length === 0) {
        console.error('Массив carPages пуст');
        return;
      }
      console.log(carPages);
      renderTable(carPages);
    } catch (error) {
      console.error('Ошибка в функции init:', error);
    }
  }
  function renderTable(carPages) {
    const tbody = document.querySelector('#carTable tbody');
    tbody.innerHTML = '';
    carPages.forEach(carPage => {
      const tr = document.createElement('tr');
      tr.innerHTML = `
        <td>${carPage.id}</td>
        <td>${carPage.manufacturer} ${carPage.model} (${carPage.year})</td>
        <td><a href="carPage.html?id=${carPage.id}">Ссылка на автомобиль</a></td>
        <td>
          <button class="carButton" onclick="deleteCar('${carPage.id}')">Удалить</button>
          <button class="carButton" onclick="createCard('${carPage.id}')">Создать карточку</button>
        </td>
      `;
      tbody.appendChild(tr);
    });
  }
  async function deleteCar(id) {
    console.log(`Удалить авто с ID: ${id}`);
    try {
      const response = await fetch(`http://localhost:4043/car-page-service/CarPage/${id}`, { 
        method: 'DELETE' 
      });
      if (!response.ok) {
        throw new Error('Ошибка при удалении: ' + response.statusText);
      }
      await response.json();
      carPages = carPages.filter(carPage => carPage.id !== id);
      renderTable(carPages);
    } catch (error) {
      console.error('Ошибка при удалении:', error);
    }
    location.reload();
  }
  async function createCard(id) {
    console.log(`Создать карточку для авто с ID: ${id}`);
    try {
      const response = await fetch(`http://localhost:4043/card-service/Card/${id}`, {
        method: 'POST',
      });
  
      if (!response.ok) {
        throw new Error('Ошибка при создании карточки: ' + response.statusText);
      }
  
      const data = await response.json();
      console.log('Карточка создана:', data);
      alert('Карточка успешно создана');
    } catch (error) {
      console.error('Ошибка при создании карточки:', error);
    }
    location.reload();
  }
  const modal = document.getElementById("addCarModal");
  const btn = document.getElementById("addCarButton");
  const span = document.getElementsByClassName("close")[0];
  btn.onclick = function() {
    modal.style.display = "block";
  }
  
  span.onclick = function() {
    modal.style.display = "none";
  }
  
  window.onclick = function(event) {
    if (event.target == modal) {
      modal.style.display = "none";
    }
  }
  
  document.getElementById('addCarModal').addEventListener('submit', async (event) => {
    event.preventDefault();
    const newCarData = {
      manufacturer: document.getElementById('manufacturer').value,
      model: document.getElementById('model').value,
      price: document.getElementById('price').value,
      color: document.getElementById('color').value,
      warranty: document.getElementById('warranty').value,
      speed: document.getElementById('speed').value,
      power: document.getElementById('power').value,
      acceleration: document.getElementById('acceleration').value,
      fuelConsumption: document.getElementById('fuelConsumption').value,
      package: document.getElementById('package').value,
      year: document.getElementById('year').value
    };
    try {
      const response = await fetch(`http://localhost:4043/car-page-service/CarPage`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(newCarData)
      });
  
      if (!response.ok) {
        throw new Error('Ошибка при добавлении: ' + response.statusText);
      }
      location.reload();
    } catch (error) {
      console.error('Ошибка при добавлении:', error);
    }
  });
  
  window.onload = init;
  