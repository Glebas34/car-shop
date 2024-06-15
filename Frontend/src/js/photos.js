let carPageId;
document.addEventListener('DOMContentLoaded', async function() {
    initializeCarPageId();
    if (!carPageId) {
        console.error('carPageId is null or undefined. Ensure the URL has the correct parameter.');
        alert('Не указан ID автомобиля в URL');
        return;
    }
    document.getElementById('updateMainPhotoButton').onclick = function() {
        updateMainPhoto(carPageId);
    };
    document.getElementById('updateAdditionalPhotosButton').onclick = function() {
        updateAdditionalPhotos(carPageId);
    };
    try {
        await updateCarPage(carPageId);
    } catch (error) {
        console.error('Произошла ошибка при получении данных об автомобиле:', error);
    }
});

function initializeCarPageId() {
    const params = new URLSearchParams(window.location.search);
    carPageId = params.get('carId');
    console.log(`Extracted carPageId: ${carPageId}`);
}

async function updateCarPage(carId) {
    await loadExistingPhotos(carId);
}

async function loadExistingPhotos(carId) {
    try {
        const mainPhotoResponse = await fetch(`http://localhost:4043/image-service/Image/MainImage/${carId}`);
        const mainPhotoData = await mainPhotoResponse.json();
        const mainPhotoUrl = mainPhotoData.url;
        const mainPhoto = document.getElementById('mainPhoto');
        mainPhoto.src = mainPhotoUrl;

        const additionalPhotosResponse = await fetch(`http://localhost:4043/image-service/Image/${carId}`);
        const additionalPhotoData = await additionalPhotosResponse.json();
        const additionalPhotos = document.getElementById('additionalPhotos');
        additionalPhotos.innerHTML = '';

        additionalPhotoData.forEach(photo => {
            const photoWrapper = document.createElement('div');
            photoWrapper.classList.add('photoWrapper');
            
            const img = document.createElement('img');
            img.src = photo.url;
            img.alt = 'Additional Photo';
            img.classList.add('photo');
            const deleteButton = document.createElement('button');
            deleteButton.textContent = 'Удалить';
            deleteButton.classList.add('photoButton');
            deleteButton.onclick = function() {
                additionalPhotos.removeChild(photoWrapper);
                deletephoto(photo.id);
                console.log(`Фото с ID: ${photo.id} удалено`);
            };
            photoWrapper.appendChild(img);
            photoWrapper.appendChild(deleteButton);
            additionalPhotos.appendChild(photoWrapper);
        });
    } catch (error) {
        console.error('Ошибка при загрузке фото:', error);
    }
}
async function deletephoto(photoId){
    try {
        const url =  `http://localhost:4043/image-service/Image/${photoId}`;
        const response = await fetch(url, {
        method: 'DELETE',
        });
        const data = await response.json();
        console.log(data);
        } catch (error) {
        console.error('Error:', error);
        }
}
function updateMainPhoto(carId) {
    const mainPhotoUpload = document.getElementById('mainPhotoUpload');
    const mainPhoto = document.getElementById('mainPhoto');
    if (mainPhotoUpload.files && mainPhotoUpload.files[0]) {
        const reader = new FileReader();
        reader.onload = function(e) {
            mainPhoto.src = e.target.result;
            // Add logic to upload the photo to the server using carId
            console.log(`Главное фото для автомобиля с ID: ${carId} обновлено`);
        }
        reader.readAsDataURL(mainPhotoUpload.files[0]);
    } else {
        alert('Пожалуйста, выберите файл для загрузки.');
    }
}

function updateAdditionalPhotos(carId) {
    const additionalPhotoUpload = document.getElementById('additionalPhotoUpload');
    const additionalPhotos = document.getElementById('additionalPhotos');
    additionalPhotos.innerHTML = '';
    if (additionalPhotoUpload.files) {
        Array.from(additionalPhotoUpload.files).forEach(file => {
            const reader = new FileReader();
            reader.onload = function(e) {
                const photoWrapper = document.createElement('div');
                photoWrapper.classList.add('photoWrapper');

                const img = document.createElement('img');
                img.src = e.target.result;
                img.alt = 'Additional Photo';
                img.classList.add('photo');

                const deleteButton = document.createElement('button');
                deleteButton.textContent = 'Удалить';
                deleteButton.classList.add('photoButton');
                deleteButton.onclick = function() {
                    additionalPhotos.removeChild(photoWrapper);
                    deletephoto(photo.id);
                    console.log(`Дополнительное фото удалено`);
                };
                photoWrapper.appendChild(img);
                photoWrapper.appendChild(deleteButton);
                additionalPhotos.appendChild(photoWrapper);
                // Add logic to upload the photo to the server using carId
                console.log(`Дополнительное фото для автомобиля с ID: ${carId} обновлено`);
            }
            reader.readAsDataURL(file);
        });
    } else {
        alert('Пожалуйста, выберите файлы для загрузки.');
    }
}

function goBack() {
    window.location.href = 'admin.html';
}
