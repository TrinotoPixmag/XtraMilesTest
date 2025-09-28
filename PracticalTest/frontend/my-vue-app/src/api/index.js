import axios from "axios";

const api = axios.create({
    baseURL: "https://weather-app.devinfosekitar.my.id/core/api",
    timeout: 5000
});

export function getCountries() {
    return api.get(`/countries`);
}

export function getCities(cityName) {
    return api.get(`/countries/${cityName}/cities`);
}

export function getWeather(cityName) {
    return api.get(`/weather/${cityName}`);
}