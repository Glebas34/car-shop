const apiUrl = 'http://localhost:4043';
const carPages = [
    {
        manufacturer: "BMW",
        model: "X6 xDrive30d",
        price: 8400000,
        color: "Искрящийся Коричневый металлик",
        warranty: "5 лет",
        speed: "250 км/ч",
        power: "290 л.с.",
        acceleration: "5,5 с",
        fuelConsumption: "6,9л/100км",
        package: "Base",
        year: 2023
    },
    {
        manufacturer: "BMW",
        model: "M340i xDrive",
        price: 6750000,
        color: "Морозный Черный металлик",
        warranty: "5 лет",
        speed: "300 км/ч",
        power: "340 л.с.",
        acceleration: "5,5 с",
        fuelConsumption: "7,7л/100км",
        package: "BMW M 50 Years Special Edition",
        year: 2023
    },
    {
        manufacturer: "BMW",
        model: "420d xDrive Gran Coupe",
        price: 6896800,
        color: "Синий Танзанит металлик",
        warranty: "5 лет",
        speed: "300 км/ч",
        power: "384 л.с.",
        acceleration: "4,7 с",
        fuelConsumption: "8,2л/100км",
        package: "M Sport Pure",
        year: 2023
    },
    {
        manufacturer: "Porsche",
        model: "718 Cayman GT4",
        price: 15990000,
        color: "Желтый",
        warranty: "5 лет",
        speed: "300 км/ч",
        power: "420 л.с.",
        acceleration: "4,4 с",
        fuelConsumption: "10,4л/100км",
        package: "Sport",
        year: 2023
    },
    {
        manufacturer: "Porsche",
        model: "Panamera Turbo S Executive",
        price: 17990000,
        color: "Синий",
        warranty: "5 лет",
        speed: "315 км/ч",
        power: "630 л.с.",
        acceleration: "3,2 с",
        fuelConsumption: "11,8л/100км",
        package: "Sport",
        year: 2023
    },
    {
        manufacturer: "Porsche",
        model: "Cayenne Turbo Coupe",
        price: 17990000,
        color: "Серый",
        warranty: "5 лет",
        speed: "286 км/ч",
        power: "550 л.с.",
        acceleration: "3,9 с",
        fuelConsumption: "12,3л/100км",
        package: "Sport",
        year: 2023
    },
];

carPages.map((carPage) => {sendData(carPage,'/car-page-service/CarPage');});

carPages = await getData();

let mainImages;
let images;


async function sendData(obj,request) {
    try {
    const url =  apiUrl + request;
    const response = await fetch(url, {
    method: 'POST',
    headers: {
    'Content-Type': 'application/json'
    },
    body: JSON.stringify(obj) 
    });
    const data = await response.json();
    console.log(data); // Response from the server
    } catch (error) {
    console.error('Error:', error);
    }
}

async function getData(request)
{
    try {
        const url = apiUrl + request;
        const response = await fetch(url);
        const data = await response.json();
        console.log('Данные получены:', data);
        return data;
      } catch (error) {
        console.error('Ошибка при получении данных:', error);
        throw error;
      }
}
