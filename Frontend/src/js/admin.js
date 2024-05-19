document.addEventListener("DOMContentLoaded", function() {
    // Получаем элементы модальных окон и кнопок
    const addOfferBtn = document.getElementById("addOfferBtn");
    const removeOfferBtn = document.getElementById("removeOfferBtn");
    const addCarBtn = document.getElementById("addCarBtn");
    const removeCarBtn = document.getElementById("removeCarBtn");

    const addOfferModal = document.getElementById("addOfferModal");
    const removeOfferModal = document.getElementById("removeOfferModal");
    const addModal = document.getElementById("addModal");
    const removeModal = document.getElementById("removeModal");

    const closeOfferBtn = document.getElementById("closeOfferBtn");
    const closeOfferBtnRemove = document.getElementById("closeOfferBtnRemove");
    const closeCarBtn = document.getElementById("closeCarBtn");
    const closeCarBtnRemove = document.getElementById("closeCarBtnRemove");

    // Показываем модальное окно для добавления предложения
    addOfferBtn.addEventListener("click", function() {
        addOfferModal.style.display = "block";
    });

    // Показываем модальное окно для удаления предложения
    removeOfferBtn.addEventListener("click", function() {
        removeOfferModal.style.display = "block";
    });

    // Показываем модальное окно для добавления автомобиля каталога
    addCarBtn.addEventListener("click", function() {
        addModal.style.display = "block";
    });

    // Показываем модальное окно для удаления автомобиля каталога
    removeCarBtn.addEventListener("click", function() {
        removeModal.style.display = "block";
    });

    // Закрываем модальные окна при нажатии на кнопку "Закрыть"
    closeOfferBtn.addEventListener("click", function() {
        addOfferModal.style.display = "none";
    });

    closeOfferBtnRemove.addEventListener("click", function() {
        removeOfferModal.style.display = "none";
    });

    closeCarBtn.addEventListener("click", function() {
        addModal.style.display = "none";
    });

    closeCarBtnRemove.addEventListener("click", function() {
        removeModal.style.display = "none";
    });
// Обработка добавления машины к актуальным предложениям
addOfferBtn.addEventListener("click", function() {
    const id = document.getElementById("idInput").value; // Получаем ID машины

    fetch(`/addOffer/${id}`, { // Отправляем запрос на сервер для добавления машины к актуальным предложениям по ID
        method: 'POST'
    })
    .then(response => {
        if (response.ok) {
            addOfferModal.style.display = "none";
            // Очищаем поле ввода ID после успешного добавления 
            document.getElementById("idInput").value = "";
        } else {
            throw new Error('Ошибка при добавлении машины к актуальным предложениям');
        }
    })
    .catch(error => {
        console.error('Ошибка:', error);
        alert('Произошла ошибка при добавлении машины к актуальным предложениям. Пожалуйста, попробуйте снова.');
    });
});
// Обработка удаления машины из актуальных предложений
removeOfferBtn.addEventListener("click", function() {
    const id = document.getElementById("idInput").value; // Получаем ID машины

    fetch(`/removeOffer/${id}`, { // Отправляем запрос на сервер для удаления машины из актуальных предложений по ID
        method: 'DELETE'
    })
    .then(response => {
        if (response.ok) {
            removeOfferModal.style.display = "none";
            // Очищаем поле ввода ID после успешного удаления
            document.getElementById("idInput").value = "";
        } else {
            throw new Error('Ошибка при удалении машины из актуальных предложений');
        }
    })
    .catch(error => {
        console.error('Ошибка:', error);
        alert('Произошла ошибка при удалении машины из актуальных предложений. Пожалуйста, попробуйте снова.');
    });
});
    // Обработка отправки формы для добавления машины в каталог
    const submitCarBtn = document.getElementById("submitCarBtn");
    submitCarBtn.addEventListener("click", function() {
        const manufacturer = document.getElementById("manufacturerInput").value;
        const model = document.getElementById("modelInput").value;
        const price = document.getElementById("priceInput").value;
        const color = document.getElementById("colorInput").value;
        const warranty = document.getElementById("warrantyInput").value;
        const speed = document.getElementById("speedInput").value;
        const power = document.getElementById("powerInput").value;
        const acceleration = document.getElementById("accelerationInput").value;
        const fuelConsumption = document.getElementById("fuelConsumptionInput").value;
        const package = document.getElementById("packageInput").value;
        const year = document.getElementById("yearInput").value;
        
        const formData = new FormData();
        formData.append('manufacturer', manufacturer);
        formData.append('model', model);
        formData.append('price', price);
        formData.append('color', color);
        formData.append('warranty', warranty);
        formData.append('speed', speed);
        formData.append('power', power);
        formData.append('acceleration', acceleration);
        formData.append('fuelConsumption', fuelConsumption);
        formData.append('package', package);
        formData.append('year', year);
        
        fetch('/addCar', {
            method: 'POST',
            body: formData
        })
        .then(response => {
            if (response.ok) {
                addModal.style.display = "none";
                // Очищаем поля ввода после успешной отправки данных
                document.getElementById("manufacturerInput").value = "";
                document.getElementById("modelInput").value = "";
                document.getElementById("priceInput").value = "";
                document.getElementById("colorInput").value = "";
                document.getElementById("warrantyInput").value = "";
                document.getElementById("speedInput").value = "";
                document.getElementById("powerInput").value = "";
                document.getElementById("accelerationInput").value = "";
                document.getElementById("fuelConsumptionInput").value = "";
                document.getElementById("packageInput").value = "";
                document.getElementById("yearInput").value = "";
            } else {
                throw new Error('Ошибка при добавлении машины');
            }
        })
        .catch(error => {
            console.error('Ошибка:', error);
            alert('Произошла ошибка при добавлении машины. Пожалуйста, попробуйте снова.');
        });

    // Обработка отправки формы для удаления машины
    const submitCarBtnRemove = document.getElementById("submitCarBtnRemove");
    submitCarBtnRemove.addEventListener("click", function() {
        const carIdToRemove = document.getElementById("carIdInputRemove").value.trim();
        if (carIdToRemove !== "") {
            fetch('/removeCar', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ carId: carIdToRemove })
            })
            .then(response => {
                if (response.ok) {
                    removeModal.style.display = "none";
                    document.getElementById("carIdInputRemove").value = "";
                } else {
                    throw new Error('Ошибка при удалении машины');
                }
            })
            .catch(error => {
                console.error('Ошибка:', error);
                alert('Произошла ошибка при удалении машины. Пожалуйста, попробуйте снова.');
            });
        } else {
            alert("Пожалуйста, введите ID автомобиля для удаления.");
        }
    });
});
});