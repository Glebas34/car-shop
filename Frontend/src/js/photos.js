let carPageId;

document.addEventListener('DOMContentLoaded', async function() {
    initializeCarPageId();
    if (!carPageId) {
        console.error('carPageId is null or undefined. Ensure the URL has the correct parameter.');
        alert('Не указан ID автомобиля в URL');
        return;
    }
    document.getElementById('updateMainPhotoButton').onclick = function() {
        updateMainPhoto();
    };
    document.getElementById('updateAdditionalPhotosButton').onclick = function() {
        updateAdditionalPhotos();
    };
    try {
        await loadExistingPhotos();
    } catch (error) {
        console.error('Произошла ошибка при получении данных об автомобиле:', error);
    }
});

function initializeCarPageId() {
    const params = new URLSearchParams(window.location.search);
    carPageId = params.get('carId');
    console.log(`Extracted carPageId: ${carPageId}`);
}

async function loadExistingPhotos() {
    try {
        const mainPhotoResponse = await fetch(`http://localhost:4043/image-service/Image/MainImage/${carPageId}`);
        const mainPhotoData = await mainPhotoResponse.json();
        const mainPhotoUrl = mainPhotoData.url;
        const mainPhoto = document.getElementById('mainPhoto');
        mainPhoto.src = mainPhotoUrl;

        const additionalPhotosResponse = await fetch(`http://localhost:4043/image-service/Image/${carPageId}`);
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

function updateMainPhoto() {
    const mainPhotoUpload = document.getElementById('mainPhotoUpload');
    const mainPhoto = document.getElementById('mainPhoto');
    if (mainPhotoUpload.files && mainPhotoUpload.files[0]) {
        const reader = new FileReader();
        reader.onload = async function(e) {
            mainPhoto.src = e.target.result;
            // Add logic to upload the photo to the server using carId
            const formData = new FormData();
            formData.append('CarPageId', carPageId);
            formData.append('Image', mainPhotoUpload.files[0], mainPhotoUpload.files[0].name);
            await sendPhoto(formData, `http://localhost:4043/image-service/Image/MainImage`);
            console.log(`Главное фото для автомобиля с ID: ${carPageId} обновлено`);
        }
        reader.readAsDataURL(mainPhotoUpload.files[0]);
    } else {
        alert('Пожалуйста, выберите файл для загрузки.');
    }
}

function updateAdditionalPhotos() {
    const additionalPhotoUpload = document.getElementById('additionalPhotoUpload');
    const additionalPhotos = document.getElementById('additionalPhotos');
    additionalPhotos.innerHTML = '';
    if (additionalPhotoUpload.files) {
        Array.from(additionalPhotoUpload.files).forEach(file => {
            const reader = new FileReader();
            reader.onload = async function(e) {
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
                const formData = new FormData();
                formData.append('CarPageId', carPageId);
                formData.append('Image', file, file.name);
                await sendPhoto(formData, `http://localhost:4043/image-service/Image`);

                console.log(`Дополнительное фото для автомобиля с ID: ${carPageId} обновлено`);
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

async function sendPhoto(formData, url)
{
    try {
        const response = await fetch(url, {
            method: 'POST',
            headers: 
            {
                //'Content-Type': 'multipart/form-data'
            },
            body: formData
        });
        const data = await response.json();
        console.log(data);
    } 
    catch (error) {
        console.error('Error:', error);
    }
}
