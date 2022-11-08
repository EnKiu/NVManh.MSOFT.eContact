<template>
  <div>
    <div class="toolbar">
      <button class="btn btn--default" @click="onAddPlan">
        <i class="icofont-coins"></i> Thêm kế hoạch
      </button>
      <button class="btn btn--add" @click="onReceive">
        <i class="icofont-dong-plus"></i> Thu tiền
      </button>
      <button class="btn btn--remove" @click="onSpend">
        <i class="icofont-dong-minus"></i> Chi tiền
      </button>
    </div>
    <div class="expenditure-statistic">
      <div class="increment-statistic">
        <div class="statistic__title">Tổng thu:</div>
        <div class="increment-total">+13.500.000</div>
      </div>
      <div class="reduce-statistic">
        <div class="statistic__title">Tổng chi:</div>
        <div class="reduce-total">-1.000.000</div>
      </div>
      <div class="money-left-statistic">
        <div class="statistic__title">Quỹ còn lại:</div>
        <div class="money-total">+12.500.000</div>
      </div>
    </div>
    <el-tabs v-model="activeName" class="demo-tabs" @tab-click="handleClick">
      <el-tab-pane label="Kế hoạch Thu/Chi" name="first">
        <div class="expenditure-plan-list">
          <expenditure-plan-item
            v-for="(plan, index) in plans"
            :key="index"
            :plan="plan"
            @dblclick="onUpdatePlan(plan)"
            @onUpdatePlan="onUpdatePlan(plan)"
            @onDeleteSuccess="loadData()"
          ></expenditure-plan-item>
        </div>
      </el-tab-pane>
      <el-tab-pane label="Thu" name="second">Config</el-tab-pane>
      <el-tab-pane label="Chi" name="third">Role</el-tab-pane>
    </el-tabs>
  </div>
  <expenditure-detail
    v-if="showDetail"
    :planEdit="planForEdit"
    :formMode="detailFormMode"
    @onClose="showDetail = false"
    @onSaveSuccess="onSavePlanSuccess"
  ></expenditure-detail>
</template>
<script>
import ExpenditurePlanItem from "./ExpenditurePlanItem.vue";
import ExpenditureDetail from "./ExpenditureDetail.vue";
import Enum from '@/scripts/enum';
import router from '@/router';
export default {
  name: "ExpenditureHome",
  components: { ExpenditurePlanItem, ExpenditureDetail },
  props: [],
  emits: [],
  created() {
    this.loadData();
  },
  methods: {
    loadData() {
      this.api({ url: "/api/v1/expenditureplans" }).then((res) => {
        this.plans = res;
      });
    },
    onAddPlan() {
      // this.showDetail = true;
      // this.detailFormMode = Enum.FormMode.ADD;
      router.push("/expenditures/create")
    },
    onUpdatePlan(plan){
      // this.showDetail = true;
      // this.planForEdit = plan;
      // this.detailFormMode = Enum.FormMode.UPDATE;
      router.push("/expenditures/"+plan.ExpenditurePlanId)
    },
    onReceive() {},
    onSpend() {},
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
      activeName: "first",
      showDetail: false,
      plans: [],
      planForEdit:null,
      detailFormMode: Enum.FormMode.ADD
    };
  },
};
</script>
<style scoped>
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
  color: #004982;
}
</style>