async function fetchRequisitions() {
    const response = await fetch('http://localhost:4043/requisition-service/Requisition');
    const requisitions = await response.json();
    const requisitionList = document.getElementById('requisition-list');
  const header = document.createElement('div');
  header.classList.add('requisition-header');
  header.innerHTML = `
    <div>Номер телефона</div>
    <div>ID заявки</div>
    <div>Бренд</div>
    <div>Модель</div>
    <div>Год выпуска</div>
    <div>Цена</div>
    <div>Цвет</div>
    <div>Комплектация</div>
    <div></div>
  `;
  requisitionList.appendChild(header);
  requisitions.forEach(req => {
    const reqElement = document.createElement('div');
    reqElement.classList.add('requisition');
    reqElement.innerHTML = `
      <div>${req.phoneNumber}</div>
      <div>${req.id}</div>
      <div>${req.manufacturer}</div>
      <div>${req.model}</div>
      <div>${req.year}</div>
      <div>${req.price.toLocaleString('ru-RU')} ₽</div>
      <div>${req.color}</div>
      <div>${req.package}</div>
      <button class="delete-button" data-req-id="${req.id}">Удалить</button>
    `;
    requisitionList.appendChild(reqElement);
    const deleteButton = reqElement.querySelector('.delete-button');
  deleteButton.addEventListener('click', function() {
    deleteRequisition(req.id);
    setTimeout(function() {
      location.reload();
    }, 20);
  });
  });
}
  document.addEventListener('DOMContentLoaded', (event) => {
    fetchRequisitions();
  });
  async function deleteRequisition(requisitionId) {
    try {
    const url =  `http://localhost:4043/requisition-service/Requisition/${requisitionId}`;
    const response = await fetch(url, {
    method: 'DELETE',
    });
    const data = await response.json();
    console.log(data);
    } catch (error) {
    console.error('Error:', error);
    }
  }