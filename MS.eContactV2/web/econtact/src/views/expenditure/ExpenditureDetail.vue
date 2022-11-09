<template>
  <m-dialog :title="formTitle" @onClose="onClose">
    <template v-slot:content>
      <div>
        <form action="">
          <div class="m-row">
            <el-radio-group v-model="optionType">
              <el-radio :label="1" size="large">Thu theo đợt/ kế hoạch</el-radio>
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
            <div class="money">{{ commonJs.formatMoney(expenditure.Amount) }}</div>
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
      <button class="btn btn--cancel"><i class="icofont-ui-close"></i> Hủy</button>
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
import router from "@/router";
export default {
  name: "ExpenditureDetail",
  emits: [],
  props: {
    id: {
      type: String,
      default: null,
      required: false,
    },
    type: {
      type: String,
      default: null,
      required: false,
    },
  },
  created() {
    if (this.type == 1) {
      this.formTitle = "Chi tiết phiếu thu";
    } else {
      this.formTitle = "Chi tiết phiếu chi";
    }
  },
  computed: {
    isEventType: function () {
      if (
        this.expenditure.ExpenditureType == Enum.ExpenditurePlanType.INCREMENT_EVENT ||
        this.expenditure.ExpenditureType == Enum.ExpenditurePlanType.REDURE_EVENT
      )
        return true;
      else return false;
    },
    apiPlanUrl() {
      return `/api/v1/expenditureplans/filter?type=${this.type}`;
    },
    isIncrement() {
      if (this.type == 1) {
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
  methods: {
    onClose() {
      router.push("/expenditures");
    },
  },
  data() {
    return {
      expenditure: {},
      formTitle: null,
      optionType: 1,
      plansFilter: [],
      expenditureTypesFilter: [],
    };
  },
};
</script>
<style scoped>
.m-row:first-child {
  margin-top: 0;
}
.m-row {
  position: relative;
  margin-top: 16px;
  width: 100%;
}
.money {
  position: absolute;
  text-align: right;
  top: 65px;
  right: 0;
  font-weight: 700;
}
</style>
