<script setup lang="ts">
import type SearchModel from "../../models/SearchModel.ts";
import {search} from "../../services/SearchService.ts";

const searchEngine = defineModel<number>("searchEngine", {
  default: 1
});
const keywords = defineModel<string>("keywords");
const url = defineModel<string>("url");
const history = defineModel<SearchModel[]>("history");
const emit = defineEmits<{
  updateHistory: []
}>();

const performSearch = () => {
  if (searchEngine.value > 0 && keywords.value && keywords.value.length > 0 && url.value && url.value?.length > 0) {
    search(searchEngine.value, keywords.value, url.value)
        .then(model => {
          if (model.result.length === 0)
            return;

          history.value?.push(model)
          emit("updateHistory");

          keywords.value = "";
          url.value = "";
        });
  }
}
</script>

<template>
  <select class="rounded-xl bg-white text-black p-3"
          v-model="searchEngine">
    <option value="1">Google</option>
  </select>
  <input class="rounded-xl bg-white text-black p-3"
         v-model="keywords"
         placeholder="Search keywords..." />
  <input class="rounded-xl bg-white text-black p-3"
         v-model="url"
         placeholder="Search URL..." />
  <div>
    <input class="rounded-xl bg-white text-black text-lg w-32 px-3 py-2 cursor-pointer duration-200 hover:bg-[#012d43] hover:text-white hover:font-bold"
           type="submit"
           value="Search"
           @click="performSearch" />
  </div>
</template>