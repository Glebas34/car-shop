async function fetchRequisitions() {
    const response = await fetch('http://localhost:4044/requisition-service/Requisition');
    const requisitions = await response.json();
    const requisitionList = document.getElementById('requisition-list');
    requisitions.forEach(req => {
      const reqElement = document.createElement('div');
      reqElement.classList.add('requisition');
      reqElement.innerHTML = `
        <div>${req.phoneNumber}</div>
        <div>${req.fullName}</div>
        <div>${req.manufacturer}</div>
        <div>${req.model}</div>
        <div>${req.year}</div>
        <div>${req.price.toLocaleString('ru-RU')} ₽</div>
        <div>${req.color}</div>
        <div>${req.package}</div>
      `;
      requisitionList.appendChild(reqElement);
    });
  }
  document.addEventListener('DOMContentLoaded', (event) => {
    fetchRequisitions();
  });