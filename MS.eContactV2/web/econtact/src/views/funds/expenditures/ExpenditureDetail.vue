<template>
  <m-dialog :title="formTitle" @onClose="onClose">
    <template v-slot:content>
      <div>
        <div class="m-row">
          <el-radio-group v-model="optionType" @change="onChangeOptionType">
            <el-radio :label="1" size="large">{{
              isIncrement == true ? "Thu theo đợt/kế hoạch" : "Chi theo đợt/kế hoạch"
            }}</el-radio>
            <el-radio :label="2" size="large">Khác</el-radio>
          </el-radio-group>
        </div>
        <div class="m-row">
          <m-combobox
            :label="isIncrement == true ? 'Mục đích thu' : 'Mục đích chi'"
            :url="'/api/v1/dictionarys/expenditure-type?type=' + type"
            v-model="expenditure.ExpenditureType"
            :required="true"
            :isDisabled="optionType == 1"
            propValue="Value"
            propText="Text"
          >
          </m-combobox>
        </div>
        <div v-if="isByPlan == true" class="m-row">
          <m-combobox
            :label="
              isIncrement == true ? 'Kế hoạch thu - Đợt thu' : 'Kế hoạch chi - Đợt chi'
            "
            :url="apiPlanUrl"
            v-model="expenditure.ExpenditurePlanId"
            :required="true"
            :isDisabled="false"
            propValue="ExpenditurePlanId"
            propText="ExpenditurePlanName"
            @onSelected="onSelectExpenditurePlan"
          >
          </m-combobox>
        </div>

        <!-- <div v-if="isEventType" class="m-row">
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
          </div> -->
        <div class="m-row">
          <m-combobox
            :label="isIncrement == true ? 'Người nộp tiền' : 'Người chi tiền'"
            url="/api/v1/contacts"
            v-model="expenditure.ContactId"
            :required="true"
            :isDisabled="false"
            propValue="ContactId"
            propText="FullName"
            @onSelected="onChangeMember"
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
          <div class="money">
            {{ commonJs.formatMoney(expenditure.Amount) }}
          </div>
        </div>
        <div class="m-row">
          <label for="">{{ isIncrement == true ? "Ngày thu" : "Ngày chi" }} </label>
          <el-date-picker
            v-model="expenditure.ExpenditureDate"
            type="date"
            format="DD-MM-YYYY"
            :placeholder="isIncrement == true ? 'Ngày thu' : 'Ngày chi'"
          />
        </div>
        <div class="m-row">
          <m-text-area
            label="Mô tả/ Ghi chú"
            v-model="expenditure.Description"
          ></m-text-area>
        </div>
      </div>
    </template>
    <template v-slot:footer>
      <button class="btn btn--cancel" @click="onClose">
        <i class="icofont-ui-close"></i> Hủy
      </button>
      <button
        @click="onSave"
        form="frm-detail"
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
    if (this.id) {
      this.api({ url: "api/v1/expenditures/" + this.id }).then((res) => {
        this.expenditure = res;
        if (
          this.expenditure.ExpenditureType == Enum.ExpenditureType.INCREMENT_PLAN ||
          this.expenditure.ExpenditureType == Enum.ExpenditureType.REDURE_PLAN
        ) {
          this.optionType = Enum.OptionExpenditurePlanType.ForPlan;
        } else {
          this.optionType = Enum.OptionExpenditurePlanType.ForOther;
        }
      });
    } else {
      this.optionType = Enum.OptionExpenditurePlanType.ForPlan;
    }
    if (this.type == Enum.ReceiptType.Income) {
      this.formTitle = "Chi tiết phiếu thu";
      this.expenditure.ExpenditureType = Enum.ExpenditureType.INCREMENT_PLAN;
    } else {
      this.formTitle = "Chi tiết phiếu chi";
      this.expenditure.ExpenditureType = Enum.ExpenditureType.REDURE_PLAN;
    }
  },
  computed: {
    expenditureNameComputed: function () {
      switch (this.expenditure.ExpenditureType) {
        case Enum.ExpenditureType.INCREMENT_PLAN: // Thu theo kế hoạch
          return `[${this.contactName}] nộp tiền theo kế hoạch [${this.planName}] `;
        case Enum.ExpenditureType.INCREMENT_SUPER_RICH: // thu từ mạnh thường quân
          return `[${this.contactName}] TÀI TRỢ [${this.commonJs.formatMoney(
            this.expenditure.Amount
          )}]`;
        case Enum.ExpenditureType.INCREMENT_OTHER: // Thu từ nguồn khác
          return `[${this.contactName}] nộp quỹ [${this.commonJs.formatMoney(
            this.expenditure.Amount
          )}]`;
        case Enum.ExpenditureType.REDURE_PLAN: // Chi theo kế hoạch
          return `[${this.contactName}] chi tiền theo kế hoạch [${this.planName}]`;
        case Enum.ExpenditureType.REDURE_WEDDING: // Chi cho đám cưới thành viên
          return `[${this.contactName}] chi tiền [đám cưới thành viên]`;
        case Enum.ExpenditureType.REDURE_FUNERAL: // Chi ma chay, cưới hỏi
          return `[${this.contactName}] chi tiền [đám ma/đám hiếu]`;
        case Enum.ExpenditureType.REDUCE_MEDICAL: // Chi ốm đau
          return `[${this.contactName}] chi tiền [thăm hỏi ốm đau]`;
        case Enum.ExpenditureType.REDUCE_OTHER: // Chi khác
          return `[${this.contactName}] chi [${this.commonJs.formatMoney(
            this.expenditure.Amount
          )}]`;
        default:
          return "test";
      }
    },
    isByPlan: function () {
      if (
        this.optionType == Enum.OptionExpenditurePlanType.ForPlan ||
        this.expenditure.ExpenditureType == Enum.ExpenditureType.INCREMENT_PLAN ||
        this.expenditure.ExpenditureType == Enum.ExpenditureType.REDURE_PLAN
      ) {
        return true;
      } else {
        return false;
      }
    },
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
    tabName() {
      return this.type == 1 ? "revenues" : "expenditures";
    },
  },
  methods: {
    onClose() {
      router.push("/funds?tab=" + this.tabName);
    },
    onChangeOptionType(value) {
      if (
        value == Enum.OptionExpenditurePlanType.ForPlan &&
        this.type == Enum.ReceiptType.Income
      ) {
        this.expenditure.ExpenditureType = Enum.ExpenditureType.INCREMENT_PLAN;
      } else if (
        value == Enum.OptionExpenditurePlanType.ForPlan &&
        this.type == Enum.ReceiptType.Outcome
      ) {
        this.expenditure.ExpenditureType = Enum.ExpenditureType.REDURE_PLAN;
      } else {
        this.expenditure.ExpenditureType = null;
      }
    },

    /* eslint-disable */
    onSelectExpenditurePlan(value, text, item) {
      this.expenditure.ExpenditurePlanId = value;
      this.expenditure.ExpenditurePlan = item;
      this.planName = item.EventName;
      // Nếu chọn là theo kế hoạch, mặc định là thu/chi theo kế hoạch
      if (this.optionType == Enum.OptionExpenditurePlanType.ForPlan) {
        if (this.type == Enum.ReceiptType.Income) {
          // --> LÀ thu thì là thu theo kế hoạch:
          this.expenditure.Amount = item.AmountUnit;
          this.expenditure.ExpenditureType = Enum.ExpenditureType.INCREMENT_PLAN;
        } else {
          // --> LÀ chi thì là chi theo kế hoạch:
          this.expenditure.ExpenditureType = Enum.ExpenditureType.REDURE_PLAN;
        }
      }
    },
    onChangeMember(value, text, item) {
      this.contactName = item.LastName;
    },
    validate() {
      return true;
      // var errors = [];
      // var isValid = true;
      // if (!this.expenditure.ExpenditureType) {
      //   errors.push("[Mục đích thu/chi] không được để trống.");
      //   isValid = false;
      // }

      // if (
      //   (this.expenditure.ExpensitureType == Enum.ExpenditureType.INCREMENT_PLAN ||
      //     this.expenditure.ExpensitureType == Enum.ExpenditureType.REDURE_PLAN) &&
      //   !this.expenditure.ExpenditurePlanId
      // ) {
      //   errors.push("[Kế hoạch thu/chi - Đợt thu/chi] không được phép để trống.");
      //   isValid = false;
      // }
      // if (!this.expenditure.ContactId) {
      //   errors.push("[Người nộp tiền/chi tiền] không được phép để trống.");
      //   isValid = false;
      // }

      // if (!this.expenditure.Amount) {
      //   errors.push("[Số tiền] không được phép để trống.");
      //   isValid = false;
      // }
      // this.commonJs.showMessenger({
      //   title: "Dữ liệu không hợp lệ",
      //   msg: errors,
      //   type: Enum.MsgType.Error,
      //   confirm: () => {
      //     console.log("Validate không hợp lệ");
      //   },
      // });
      // return isValid;
    },
    onSave() {
      this.expenditure.ExpenditureName = this.expenditureNameComputed;
      if (this.validate()) {
        var url = "api/v1/expenditures";
        var method = "POST";
        if (this.expenditure.ExpenditureId) {
          url = "api/v1/expenditures/" + this.expenditure.ExpenditureId;
          method = "PUT";
        }
        this.api({ url: url, data: this.expenditure, method: method })
          .then(() => {
            router.push("/funds?tab=" + this.tabName);
          })
          .catch((res) => {
            if (res.status == 409) {
              this.commonJs.showConfirm(
                `Thành viên này đã nộp tiền cho khoản này rồi, Bạn có muốn cập nhật lại số tiền [${this.commonJs.formatMoney(
                  this.expenditure.Amount
                )}] cho thành viên này?. Nếu không vui lòng đóng thông báo này và cập nhật lại tổng số tiền của thành viên đã đóng.`,
                () => {
                  // Lấy thông tin khoản thu đã có:
                  this.api({
                    url: `api/v1/expenditures/find-option?planId=${this.expenditure.ExpenditurePlanId}&contactId=${this.expenditure.ContactId}`,
                  }).then((res) => {
                    this.expenditure.ExpenditureId = res.ExpenditureId;
                    this.api({
                      url: url + "/" + res.ExpenditureId,
                      data: this.expenditure,
                      method: "PUT",
                    }).then(() => {
                      router.push("/funds?tab=" + this.tabName);
                    });
                  });
                }
              );
            }
          });
      }
    },
  },
  data() {
    return {
      expenditure: {
        ExpenditureType: Enum.ExpenditureType.INCREMENT_PLAN,
        ExpenditureDate: new Date(),
      },
      formTitle: null,
      optionType: null,
      plansFilter: [],
      expenditureTypesFilter: [],
      contactName: "",
      planName: "",
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
