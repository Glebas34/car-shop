document.addEventListener("DOMContentLoaded", function() {
    const form = document.querySelector('.main__form');
    const button = form.querySelector('.main__form-button');

    button.addEventListener('click', function(event) {
        event.preventDefault();
        // Проверка заполнения полей
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
        // Проверка формата телефона
        const phoneRegex = /^\+7 \(\d{3}\) \d{3}-\d{2}-\d{2}$/;
        if (!phoneRegex.test(phoneInput.value.trim())) {
            alert('Пожалуйста, введите телефон в формате +7 (XXX) XXX-XX-XX');
            return;
        }
        // Если все данные введены корректно
        alert('Мы вам перезвоним');
        form.reset();
    });
});
document.addEventListener("DOMContentLoaded", function() {
    const phoneInput = document.querySelector('input[name="phone"]');

    phoneInput.addEventListener('input', function(event) {
        const input = event.target.value.replace(/\D/g, '').substring(0, 11);
        const phoneNumber = input.replace(/(\d{1})(\d{3})(\d{3})(\d{2})(\d{2})/, "+7 (\$2) \$3-\$4-\$5");
        event.target.value = phoneNumber;
    });
});