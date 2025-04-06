<script setup lang="ts">
import {onMounted, ref} from "vue";
import SearchForm from "../components/home/SearchForm.vue";
import SearchHistory from "../components/home/SearchHistory.vue";
import type SearchModel from "../models/SearchModel.ts";
import {loadSearchHistory, saveSearchHistory} from "../store/SearchStore.ts";

const keywords = ref("");
const url = ref("");
const history = ref<SearchModel[]>([]);

onMounted(() => {
  history.value = loadSearchHistory();
})

const setForm = (k: string, u: string) => {
  keywords.value = k;
  url.value = u;
}

const clearHistory = () => {
  history.value = [];
  saveHistory();
}

const saveHistory = () => {
  saveSearchHistory(history.value);
}

</script>

<template>
  <section class="max-w-[1000px] mx-auto flex flex-col gap-3">
    <SearchForm v-model:keywords="keywords" v-model:url="url" v-model:history="history" v-on:updateHistory="saveHistory" />
    <hr class="my-5" />
    <SearchHistory v-bind:history="history" v-on:searchAgain="setForm" v-on:clearHistory="clearHistory" />
  </section>
</template>

<style scoped>

</style>