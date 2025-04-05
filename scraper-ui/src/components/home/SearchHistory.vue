<script setup lang="ts">
import type SearchModel from "../../models/SearchModel.ts";

const props = defineProps<{
  history: SearchModel[]
}>();

</script>

<template>
  <label class="text-lg font-bold">Search History</label>
  <div v-if="props.history.length > 0" class="rounded-xl bg-white text-black p-3">
    <table class="table-auto w-full">
      <thead>
        <tr>
          <th class="text-start">
            <label class="font-bold underline underline-offset-4">Date</label>
          </th>
          <th class="text-start">
            <label class="font-bold underline underline-offset-4">Keywords</label>
          </th>
          <th class="text-start">
            <label class="font-bold underline underline-offset-4">URL</label>
          </th>
          <th class="text-start">
            <label class="font-bold underline underline-offset-4">Result</label>
          </th>
        </tr>
      </thead>
      <tbody v-for="item in props.history">
        <tr>
          <td>
            {{new Date(item.createdDate).toLocaleString("en-GB", { timeZone: "UTC" })}}
          </td>
          <td>
            {{item.keywords}}
          </td>
          <td>
            {{item.url}}
          </td>
          <td>
            {{item.result}}
          </td>
          <td class="text-end">
            <button class="rounded-xl bg-[#023E5C] text-sm text-white px-3 py-2 my-1 cursor-pointer duration-200 hover:bg-[#012d43]"
                    @click="$emit('searchAgain', item.keywords, item.url)">
              Search Again
            </button>
          </td>
        </tr>
      </tbody>
    </table>
    <button class="rounded-xl bg-[#023E5C] text-sm text-white px-3 py-2 mt-3 cursor-pointer duration-200 hover:bg-[#012d43]"
            @click="$emit('clearHistory')">
      Clear
    </button>
  </div>
  <div v-else>
    <label class="italic">None</label>
  </div>
</template>

<style scoped>
  th, td {
    padding: 0 .5rem;
  }
</style>