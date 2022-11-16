<template>
  <div class="plan">
    <div class="plan__name">
      <div class="plan__title">
        <span class="text--underline" :class="isIncome ? '--color-green' : '--color-red'"
          >{{ planTypeName }}:</span
        >
        {{ plan.ExpenditurePlanName }}
      </div>
      <!-- <div class="plan__state">(Đang thu)</div> -->
    </div>
    <div class="flex">
      <div class="plan__amount">
        <div
          v-if="plan.TotalMoney"
          class="expenditure__fee"
          :class="isIncome ? '--color-green' : '--color-red'"
        >
          <span v-if="isIncome">+</span><span v-else>-</span
          >{{ this.commonJs.formatMoney(plan.TotalMoney) }}
        </div>
      </div>
      <div v-if="isAdmin" class="plan__option">
        <button
          class="btn-mini --color-red"
          title="Xóa kế hoạch"
          @click="onDeletePlan(plan)"
        >
          <i class="icofont-ui-delete"></i>
        </button>
        <button
          class="btn-mini --color-edit"
          title="Sửa kế hoạch"
          @click="onUpdatePlan(plan)"
        >
          <i class="icofont-ui-edit"></i>
        </button>
      </div>
    </div>
  </div>
</template>
<script>
export default {
  name: "ExpenditurePlanItem",
  emits: ["onDeleteSuccess", "onUpdatePlan"],
  props: ["plan", "isAdmin"],
  //   created() {
  //     for (let index = 0; index < 20; index++) {
  //       var expenditurePlan = {
  //         ExpenditurePlanId: index + 1,
  //         ExpenditurePlanName:
  //           "Kế hoạch thu chi tháng 10/2022 Kế hoạch thu chi tháng 10/2022",
  //         ExpenditurePlanType: Math.round(Math.random(0, 1) + 1),
  //         EndDate: "2022-12-11",
  //         Amount: Math.round(Math.random(0, 1) * 100000000),
  //       };
  //       this.plans.push(expenditurePlan);
  //     }
  //   },
  computed: {
    isIncome() {
      if (this.plan.ExpenditurePlanType < 200) {
        return true;
      }
      return false;
    },
    planTypeName() {
      if (this.plan.ExpenditurePlanType < 200) {
        return "Thu";
      }
      return "Chi";
    },
  },
  methods: {
    onDeletePlan(plan) {
      this.commonJs.showConfirm("Bạn có chắc chắn muốn xóa kế hoạch này?", () => {
        this.api({
          url: `/api/v1/expenditureplans/${plan.ExpenditurePlanId}`,
          method: "DELETE",
        }).then(() => {
          this.$emit("onDeleteSuccess");
        });
      });
    },
    onUpdatePlan(plan) {
      this.$emit("onUpdatePlan", plan);
    },
  },
  data() {
    return {};
  },
};
</script>
<style scoped>
.plan {
  padding: 10px;
  display: flex;
  align-items: center;
  justify-content: space-between;
}
.plan + .plan {
  border-top: 1px solid #dedede;
}
.plan__detail {
  display: flex;
  align-items: center;
  justify-content: space-between;
}

/* .plan__name{
  margin-right: 8px;
} */

.plan__amount {
  margin: 0 8px;
  font-weight: 700;
}
.btn-mini + .btn-mini {
  margin-left: 4px;
}

.plan__option {
  width: 45px;
}
</style>
