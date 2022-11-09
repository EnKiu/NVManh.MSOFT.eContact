<template>
  <m-dialog :title="formTitle">
    <template v-slot:content>
      <div>
        <form action="">
          <div class="m-row">
            <el-radio-group v-model="optionType">
              <el-radio :label="1" size="large"
                >Thu theo đợt/ kế hoạch</el-radio
              >
              <el-radio :label="2" size="large">Khác</el-radio>
            </el-radio-group>
          </div>
          <div v-if="optionType == 1" class="m-row">
            <m-combobox
              label="Kế hoạch - Đợt thu/chi"
              :url="apiPlanUrl"
              v-model="expenditure.ExpenditurePlanId"
              :required="true"
              :isDisabled="false"
              propValue="ExpenditurePlanId"
              propText="ExpenditurePlanName"
            >
            </m-combobox>
          </div>
          <div class="m-row">
            <m-combobox
              label="Loại khoản"
              url="/api/v1/expenditureplans/plan-type"
              v-model="expenditure.ExpenditureType"
              :required="true"
              :isDisabled="false"
              propValue="Value"
              propText="Text"
            >
            </m-combobox>
          </div>
          <div v-if="isEventType" class="m-row">
            <m-combobox
              label="Sự kiện"
              url="/api/v1/Events"
              v-model="expenditure.EventId"
              :required="true"
              :isDisabled="false"
              propValue="EventId"
              propText="EventName"
            >
            </m-combobox>
          </div>
          <div v-if="isIncrement" class="m-row">
            <m-combobox
              label="Người nộp tiền"
              url="/api/v1/contacts"
              v-model="expenditure.ContactId"
              :required="true"
              :isDisabled="false"
              propValue="ContactId"
              propText="FullName"
            >
            </m-combobox>
          </div>
          <div class="m-row">
            <m-input
              label="Số tiền"
              :onlyNumberChar="true"
              v-model="expenditure.Amount"
              required
            ></m-input>
          </div>
          <div class="m-row">
            <label for="">Ngày thu </label>
            <el-date-picker
              v-model="expenditure.ExpenditureDate"
              type="date"
              format="DD-MM-YYYY"
              placeholder="Ngày thu"
            />
          </div>
          <div class="m-row">
            <m-text-area
              label="Mô tả/ Ghi chú"
              v-model="expenditure.Description"
            ></m-text-area>
          </div>
        </form>
      </div>
    </template>
    <template v-slot:footer>
      <button class="btn btn--cancel">
        <i class="icofont-ui-close"></i> Hủy
      </button>
      <button
        type="submit"
        form="form-info"
        class="btn btn--default"
        style="margin-left: 10px"
      >
        <i class="icofont-save"></i> Hoàn tất
      </button>
    </template>
  </m-dialog>
</template>
<script>
import Enum from "@/scripts/enum";
export default {
  name: "ExpenditureDetail",
  emits: [],
  props: ["id", "type"],
  created() {
    console.log(this.type);
    if (this.isIncrement) {
      this.formTitle = "Chi tiết phiếu thu";
    } else {
      this.formTitle = "Chi tiết phiếu chi";
    }
  },
  computed: {
    isEventType: function () {
      if (
        this.expenditure.ExpenditureType ==
          Enum.ExpenditurePlanType.INCREMENT_EVENT ||
        this.expenditure.ExpenditureType ==
          Enum.ExpenditurePlanType.REDURE_EVENT
      )
        return true;
      else return false;
    },
    apiPlanUrl(){
        if (
        this.type == Enum.ExpenditureType.INCREMENT_OTHER ||
        this.type == Enum.ExpenditureType.INCREMENT_PLAN ||
        this.type == Enum.ExpenditureType.INCREMENT_SUPER_RICH
      ) {
        return "/api/v1/expenditureplans/filter?type=1";
      } else {
        return "/api/v1/expenditureplans/filter?type=2";
      }
    },
    isIncrement() {
      if (
        this.type == Enum.ExpenditureType.INCREMENT_OTHER ||
        this.type == Enum.ExpenditureType.INCREMENT_PLAN ||
        this.type == Enum.ExpenditureType.INCREMENT_SUPER_RICH
      ) {
        return true;
      } else {
        return false;
      }
    },
    isAdd() {
      if (this.id && this.id != "create") {
        return false;
      }
      return true;
    },
  },
  data() {
    return {
      expenditure: {},
      formTitle: null,
      optionType: 1,
      plansFilter:[],
      expenditureTypesFilter:[]
    };
  },
};
</script>
<style scoped>
.m-row {
  margin-top: 16px;
  width: 100%;
}
</style>