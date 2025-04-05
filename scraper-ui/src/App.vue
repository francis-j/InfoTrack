<script setup lang="ts">
import { RouterView } from 'vue-router'
import Header from "./components/layout/Header.vue";
</script>

<template>
  <Header />
  <main class="p-5">
    <RouterView />
  </main>
</template>