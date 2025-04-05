<script setup lang="ts">
import {FontAwesomeIcon} from "@fortawesome/vue-fontawesome";
</script>

<template>
  <header class="sticky top-0 w-full p-5">
    <nav class="max-w-[1000px] mx-auto flex justify-between items-center p-5 bg-white text-black rounded-2xl">
      <a class="text-red-500 font-bold italic"
         href="https://www.infotrack.co.uk"
         target="_blank">
        <img class="h-12 w-36"
             src="https://www.infotrack.co.uk/media/w3xbuylz/site-logo.svg"
             alt="InfoTrack" />
      </a>
      <label class="hidden md:block text-2xl font-bold">
        Search Engine Scraper
      </label>
      <a class="hover:underline underline-offset-4 decoration-red-500 w-36 text-end"
         href="https://github.com/francis-j/InfoTrack"
         target="_blank">
        <FontAwesomeIcon icon="fa-brands fa-github" size="2xl" />
      </a>
    </nav>
  </header>
</template>