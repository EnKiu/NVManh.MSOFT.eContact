<template>
  <div class="fund-container">
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
          <span v-if="FundInfo.FundLeft > 0">+</span><span v-else>-</span
          >{{ commonJs.formatMoney(FundInfo.FundLeft) }}
        </div>
      </div>
    </div>
    <div class="tab-panel">
      <el-tabs v-model="activeName" class="demo-tabs" @tab-click="handleClick">
        <el-tab-pane label="Kế hoạch Thu/Chi" name="plans">
          <expenditure-plan-list :isAdmin="isAdmin"></expenditure-plan-list>
        </el-tab-pane>
        <el-tab-pane label="Thu" name="revenues">
          <fluctuation-list :isIncome="true" :isAdmin="isAdmin"></fluctuation-list>
        </el-tab-pane>
        <el-tab-pane label="Chi" name="expenditures">
          <fluctuation-list :isIncome="false" :isAdmin="isAdmin"></fluctuation-list>
        </el-tab-pane>
      </el-tabs>
    </div>
    <div class="toolbar">
      <button class="btn btn--default" :disabled="!isAdmin" @click="onAddPlan">
        <i class="icofont-coins"></i> Thêm kế hoạch
      </button>
      <button class="btn btn--add" :disabled="!isAdmin" @click="onReceive">
        <i class="icofont-dong-plus"></i> Thu
      </button>
      <button class="btn btn--remove" :disabled="!isAdmin" @click="onSpend">
        <i class="icofont-dong-minus"></i> Chi
      </button>
    </div>
    <router-view name="ExpenditureDialog" :type="type"></router-view>
  </div>
</template>
<script>
import FluctuationList from "./fluctuations/FluctuationList.vue";
import ExpenditurePlanList from "./plans/ExpenditurePlanList.vue";
// import ExpenditurePlanDetail from "./ExpenditurePlanDetail.vue";
import Enum from "@/scripts/enum";
import router from "@/router";
export default {
  name: "ExpenditureHome",
  components: { FluctuationList, ExpenditurePlanList },
  props: ["type", "tab"],
  emits: [],
  created() {
    var roleValue = localStorage.getItem("userRoleValue");
    if (roleValue == 1) {
      this.isAdmin = true;
    }
    this.activeName = this.tab ? this.tab : "plans";
    this.loadData();
    this.hubConnection.on("UpdateFundInfo", (fundInfo) => {
      this.FundInfo = fundInfo;
    });
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
    onAddPlan() {
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
      isAdmin: false,
    };
  },
};
</script>
<style scoped>
.fund-container {
  position: relative;
  height: 100%;
  background-color: #fff !important;
  box-sizing: border-box;
  border-radius: 0 0 4px 4px;
  /* border: 1px solid #004982; */
}
.tab-panel {
  position: relative;
  background-color: #fff !important;
  height: calc(100% - 60px);
}
.expenditure-plan-list {
  max-height: calc(100vh - 270px);
  overflow-y: auto;
}
.toolbar {
  width: 100%;
  position: absolute;
  left: 0px;
  bottom: 0px;
  z-index: 1000;
  padding: 4px;
  box-shadow: 0 0 10px #ccc;
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
  border-bottom: 1px solid #ccc;
  border-radius: 4px 4px 0 0;
  padding: 8px;
  box-shadow: 0px 0px 10px #404040;
  background-color: #fff;
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
