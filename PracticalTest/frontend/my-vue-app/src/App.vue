<template>
  <div class="min-h-screen bg-gradient-to-br from-blue-100 to-blue-300 flex items-center justify-center p-6">
    <div class="bg-white shadow-lg rounded-2xl p-6 w-full max-w-md">
      <h1 class="text-3xl font-bold text-center text-blue-700 mb-6">🌤 Weather App</h1>

      <label class="block mb-2 font-semibold">Select Country:</label>
      <select
        v-model="selectedCountry"
        @change="loadCities"
        class="border border-gray-300 rounded-lg p-2 w-full mb-4 focus:ring-2 focus:ring-blue-400"
      >
        <option value="">-- Choose Country --</option>
        <option v-for="c in countries" :key="c" :value="c.code">{{ c.name }}</option>
      </select>

      <label class="block mb-2 font-semibold">Select City:</label>
      <select
        v-model="selectedCity"
        @change="getWeatherMethod"
        class="border border-gray-300 rounded-lg p-2 w-full mb-4 focus:ring-2 focus:ring-blue-400 disabled:bg-gray-100"
        :disabled="!selectedCountry"
      >
        <option value="">-- Choose City --</option>
        <option v-for="city in cities" :key="city" :value="city">{{ city }}</option>
      </select>

      <div v-if="loading" class="flex justify-center py-6">
        <div class="w-8 h-8 border-4 border-blue-400 border-t-transparent rounded-full animate-spin"></div>
      </div>

      <div v-if="weather && !loading" class="mt-6 p-4 bg-blue-50 border border-blue-200 rounded-xl shadow">
        <div class="bg-gradient-to-br from-blue-200 to-blue-400 p-6 rounded-2xl shadow-lg text-gray-800 w-full max-w-md">
          <div class="items-center justify-between">
            <h2 class="text-2xl font-bold">
              {{ weather.city }}, {{ weather.country }}
            </h2>
            <span class="text-sm block">{{ new Date(weather.utcTime).toUTCString() }}</span>
          </div>

          <div class="items-center justify-between mt-4">
            <div>
              <p class="font-extrabold">{{ weather.temperatureC }}°C</p>
              <p class="text-sm text-gray-700">({{ weather.temperatureF }}°F)</p>
            </div>
            <div class="flex flex-col items-center">
              <CloudIcon v-if="weather.skyCondition.includes('cloud')" class="w-5 h-5 text-gray-700" />
              <SunIcon v-else-if="weather.skyCondition.includes('clear')" class="w-5 h-5 text-yellow-500" />
              <CloudRainIcon v-else class="w-5 h-5 text-blue-600" />
              <p class="capitalize">{{ weather.skyCondition }}</p>
            </div>
          </div>

          <div class="grid grid-cols-2 gap-4 mt-6 text-sm">
            <div class="flex items-center gap-2">
              <WindIcon class="w-5 h-5 text-blue-800" />
              <span>{{ weather.windSpeed }} m/s ({{ weather.windDirection }}°)</span>
            </div>
            <div class="flex items-center gap-2">
              <DropletIcon class="w-5 h-5 text-blue-600" />
              <span>{{ weather.humidity }}% Humidity</span>
            </div>
            <div class="flex items-center gap-2">
              <EyeIcon class="w-5 h-5 text-gray-600" />
              <span>{{ weather.visibility / 1000 }} km Visibility</span>
            </div>
            <div class="flex items-center gap-2">
              <GaugeIcon class="w-5 h-5 text-gray-600" />
              <span>{{ weather.pressure }} hPa</span>
            </div>
            <div class="flex items-center gap-2">
              <ThermometerIcon class="w-5 h-5 text-red-500" />
              <span>Dew Point: {{ weather.dewPoint }}°C</span>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { SunIcon, CloudIcon, CloudRainIcon, WindIcon, DropletIcon, EyeIcon, GaugeIcon, ThermometerIcon } from "lucide-vue-next";
import { ref, onMounted } from "vue";
import axios from "axios";
import { getCountries, getCities, getWeather } from "./api";

const countries = ref([]);
const selectedCountry = ref("");
const selectedCity = ref("");
const cities = ref([]);
const weather = ref(null);
const loading = ref(false);

const fetchCountries = async () => {
  try {
    const res = await getCountries();
    countries.value = res.data;
  } catch (err) {
    console.error("Failed to load countries", err);
  }
};

const fetchCities = async () => {
  try {
    const res = await getCities(selectedCountry.value);
    cities.value = res.data;
  } catch (err) {
    console.error("Failed to load city", err);
  }
};

function loadCities(countryId) {
  fetchCities(countryId);
}

async function getWeatherMethod() {
  if (!selectedCity.value) return;
  loading.value = true;
  try {
    const res = await getWeather(selectedCity.value);
    weather.value = res.data;
  } catch (error) {
    alert("Failed to fetch weather data");
    console.error(error);
  } finally {
    loading.value = false;
  }
}

onMounted(fetchCountries);
</script>
