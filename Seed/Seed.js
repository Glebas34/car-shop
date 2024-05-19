const apiUrl = 'http://localhost:4043';
const carPages = [
    {
        manufacturer: "string",
        model: "string",
        price: 0,
        color: "string",
        warranty: "string",
        speed: "string",
        power: "string",
        acceleration: "string",
        fuelConsumption: "string",
        package: "string",
        year: 0

    },

    {
        manufacturer: "string",
        model: "string",
        price: 0,
        color: "string",
        warranty: "string",
        speed: "string",
        power: "string",
        acceleration: "string",
        fuelConsumption: "string",
        package: "string",
        year: 0

    },

    {
        manufacturer: "string",
        model: "string",
        price: 0,
        color: "string",
        warranty: "string",
        speed: "string",
        power: "string",
        acceleration: "string",
        fuelConsumption: "string",
        package: "string",
        year: 0

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
