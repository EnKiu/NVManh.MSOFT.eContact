<template>
  <div class="expenditure-plan-list">
    <expenditure-plan-item
      v-for="(plan, index) in plans"
      :key="index"
      :plan="plan"
      :isAdmin="isAdmin"
      @dblclick="onUpdatePlan(plan)"
      @onUpdatePlan="onUpdatePlan(plan)"
      @onDeleteSuccess="loadData()"
    ></expenditure-plan-item>
  </div>
</template>
<script>
import router from "@/router";
import ExpenditurePlanItem from "./ExpenditurePlanItem.vue";
export default {
  name: "ExpenditurePlanList",
  components: { ExpenditurePlanItem },
  props: ["isAdmin"],
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
      router.replace("/funds/plans/create");
    },
    onUpdatePlan(plan) {
      // this.showDetail = true;
      // this.planForEdit = plan;
      // this.detailFormMode = Enum.FormMode.UPDATE;
      router.replace("/funds/plans/" + plan.ExpenditurePlanId);
    },
  },
  data() {
    return {
      plans: [],
    };
  },
};
</script>
<style scoped></style>
