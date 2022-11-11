<template>
  <div>
    <div class="toolbar">
      <button class="btn btn--default" @click="onAddPlan">
        <i class="icofont-coins"></i> Thêm kế hoạch
      </button>
      <button class="btn btn--add" @click="onReceive">
        <i class="icofont-dong-plus"></i> Thu
      </button>
      <button class="btn btn--remove" @click="onSpend">
        <i class="icofont-dong-minus"></i> Chi
      </button>
    </div>
    <div class="expenditure-statistic">
      <div class="increment-statistic">
        <div class="statistic__title">Tổng thu:</div>
        <div class="increment-total">
          +{{ commonJs.formatMoney(FundInfo.RevenueTotal) }}
        </div>
      </div>
      <div class="reduce-statistic">
        <div class="statistic__title">Tổng chi:</div>
        <div class="reduce-total">
          -{{ commonJs.formatMoney(FundInfo.ExpenditureTotal) }}
        </div>
      </div>
      <div class="money-left-statistic">
        <div class="statistic__title">Quỹ còn lại:</div>
        <div
          class="money-total"
          :class="FundInfo.FundLeft < 0 ? '--color-red' : '--color-green'"
        >
          {{ commonJs.formatMoney(FundInfo.FundLeft) }}
        </div>
      </div>
    </div>
    <el-tabs v-model="activeName" class="demo-tabs" @tab-click="handleClick">
      <el-tab-pane label="Kế hoạch Thu/Chi" name="plans">
       <expenditure-plan-list></expenditure-plan-list>
      </el-tab-pane>
      <el-tab-pane label="Thu" name="revenues">
        <fluctuation-list :isIncome="true"></fluctuation-list>
      </el-tab-pane>
      <el-tab-pane label="Chi" name="expenditures">
        <fluctuation-list :isIncome="false"></fluctuation-list>
      </el-tab-pane>
    </el-tabs>
    <router-view name="ExpenditureDialog" :type="type"></router-view>
  </div>
</template>
<script>
import FluctuationList from "./fluctuations/FluctuationList.vue";
import ExpenditurePlanList from './plans/ExpenditurePlanList.vue'
// import ExpenditurePlanDetail from "./ExpenditurePlanDetail.vue";
import Enum from "@/scripts/enum";
import router from "@/router";
export default {
  name: "ExpenditureHome",
  components: {FluctuationList,ExpenditurePlanList },
  props: ["type", "tab"],
  emits: [],
  created() {
    console.log("activeName: ",this.tab);
    this.activeName = this.tab ? this.tab : "plans";
    this.loadData();
  },
  // beforeRouteEnter(to, from, next) {
  //   console.log(`to`, to);
  //   console.log(`from`, from);
  //   console.log(`next`, next);
  //   next();
  // },
  methods: {
    loadData() {
      this.api({ url: "api/v1/expenditures/general-info" }).then((r) => {
        this.FundInfo = r;
      });
      this.api({ url: "/api/v1/expenditureplans" }).then((res) => {
        this.plans = res;
      });
    },
    onAddPlan(){
      router.replace("/funds/plans/create");
    },
    onReceive() {
      router.replace("/funds/create?type=1");
    },
    onSpend() {
      router.replace("/funds/create?type=2");
    },
    onSavePlanSuccess() {
      this.showDetail = false;
      this.loadData();
    },
    handleClick(tab, event) {
      console.log(tab, event);
    },
  },
  data() {
    return {
      activeName: "plans",
      showDetail: false,
      plans: [],
      planForEdit: null,
      detailFormMode: Enum.FormMode.ADD,
      FundInfo: {},
    };
  },
};
</script>
<style scoped>
.expenditure-plan-list {
  max-height: calc(100vh - 270px);
  overflow-y: auto;
}
.toolbar {
  position: absolute;
  left: -2px;
  bottom: -10px;
  margin-bottom: 8px;
  z-index: 1000;
}

.toolbar button + button {
  margin-left: 8px;
}

button i {
  font-size: 16px;
}
.expenditure-statistic {
  display: flex;
  align-items: center;
  justify-content: space-between;
  border: 1px solid #004982;
  border-radius: 4px;
  padding: 8px;
  box-shadow: 0px 0px 10px #404040;
}
.statistic__title {
  font-weight: 700;
  text-decoration: underline;
}
.increment-total {
  color: #0c701e;
}

.reduce-total {
  color: #ff0000;
}

.money-total {
  font-weight: 700;
}
</style>
