<template>
  <div class="fluctuation-list">
    <fluctuation-item
      v-for="(item, index) in fluctuations"
      :key="index"
      :item="item"
      :isIncome="isIncome"
      @onAfterDelete="onAfterDeleteItem"
    ></fluctuation-item>
  </div>
</template>
<script>
import FluctuationItem from "./FluctuationItem.vue";
export default {
  name: "FluctuationList",
  components: { FluctuationItem },
  emits: [],
  props: {
    isIncome: Boolean,
  },
  created() {
    this.loadData();
  },
  methods: {
    onAfterDeleteItem() {
      this.loadData();
    },
    loadData() {
      this.api({
        url: "api/v1/expenditures/" + (this.isIncome ? "revenues" : "spends"),
      }).then((res) => {
        this.fluctuations = res;
      });
    },
  },
  data() {
    return {
      fluctuations: [],
    };
  },
};
</script>
<style scoped>
.fluctuation-list {
  max-height: calc(100vh - 270px);
  overflow-y: auto;
}
</style>