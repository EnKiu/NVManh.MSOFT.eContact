<template>
  <div>
    <div class="expenditure-statistic">
      <div class="increment-statistic">
        <div class="statistic__title">Tổng thu: </div>
        <div class="increment-total">+13.500.000</div>
      </div>
      <div class="reduce-statistic">
        <div class="statistic__title">Tổng chi: </div>
        <div class="reduce-total">-1.000.000</div>
      </div>
      <div class="money-left-statistic">
        <div class="statistic__title">Quỹ còn lại: </div>
        <div class="money-total">+12.500.000</div>
      </div>
    </div>
    <el-tabs v-model="activeName" class="demo-tabs" @tab-click="handleClick">
      <el-tab-pane label="Kế hoạch Thu/Chi" name="first">
        <expenditure-plan-list></expenditure-plan-list>
      </el-tab-pane>
      <el-tab-pane label="Thu" name="second">Config</el-tab-pane>
      <el-tab-pane label="Chi" name="third">Role</el-tab-pane>
    </el-tabs>
  </div>
</template>
<script>
import ExpenditurePlanList from "./ExpenditurePlanList.vue";
export default {
  name: "ExpenditureHome",
  components: { ExpenditurePlanList },
  props: [],
  emits: [],
  created() {},
  methods: {
    handleClick(tab, event) {
      console.log(tab, event);
    },
  },
  data() {
    return {
      activeName: "first",
    };
  },
};
</script>
<style scoped>
.expenditure-statistic{
    display: flex;
    align-items: center;
    justify-content: space-between;
    border: 1px solid #004982;
    border-radius: 4px;
    padding: 8px;
    box-shadow: 0px 0px 10px #404040;
}
.statistic__title{
    font-weight: 700;
    text-decoration: underline;
}
.increment-total{
    color: #0c701e;
}

.reduce-total{
    color: #ff0000;
}

.money-total{
    color: #004982;
}
</style>