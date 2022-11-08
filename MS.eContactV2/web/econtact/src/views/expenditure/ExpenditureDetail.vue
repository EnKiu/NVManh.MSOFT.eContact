<template>
  <m-dialog title="Thông tin kế hoạch thu/chi" @onClose="onClose">
    <template v-slot:content>
      <form id="form-info" @submit.prevent="onSubmitForm"></form>
      <div class="m-row">
        <m-input
          label="Tên kế hoạch"
          v-model="plan.ExpenditurePlanName"
          :isFocus="true"
          required
        ></m-input>
      </div>
      <div class="m-row">
        <m-combobox
          label="Thu/chi cho"
          url="/api/v1/expenditureplans/plan-type"
          v-model="plan.ExpenditurePlanType"
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
          v-model="plan.EventId"
          :required="true"
          :isDisabled="false"
          propValue="EventId"
          propText="EventName"
        >
        </m-combobox>
      </div>
      <div v-if="isIncrement" class="m-row">
        <m-input
          label="Số tiền/người"
          :onlyNumberChar="true"
          v-model="plan.AmountUnit"
          required
        ></m-input>
      </div>
      <div class="m-row flex">
        <div class="m-col">
          <label for="">Bắt đầu thu từ: </label>
          <el-date-picker
            v-model="plan.StartDate"
            type="date"
            format="DD-MM-YYYY"
            placeholder="Ngày bắt đầu thu/chi"
          />
        </div>
        <div class="m-col" style="margin-left: 10px">
          <label for="">Ngày kết thúc đợt thu:</label>
          <el-date-picker
            v-model="plan.EndDate"
            type="date"
            format="DD-MM-YYYY"
            placeholder="Ngày kết thúc"
          />
        </div>
      </div>

      <div class="m-row">
        <m-combobox
          label="Người thực hiện"
          url="/api/v1/contacts"
          v-model="plan.ContactId"
          :required="false"
          :isDisabled="false"
          propValue="ContactId"
          propText="FullName"
        >
        </m-combobox>
      </div>
      <div class="m-row">
        <m-text-area
          label="Mô tả/ Ghi chú"
          v-model="plan.Description"
        ></m-text-area>
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
        <i class="icofont-save"></i> Lưu
      </button>
    </template>
  </m-dialog>
</template>
<script>
import Enum from "@/scripts/enum";
import router from "@/router";
export default {
  name: "ExpenditureDetail",
  emits: ["onSaveSuccess", "update:formMode"],
  props: ["planEdit", "formMode", "id"],
  computed: {
    isEventType: function () {
      if (
        this.plan.ExpenditurePlanType ==
        Enum.ExpenditurePlanType.INCREMENT_EVENT
      )
        return true;
      else return false;
    },
    isIncrement() {
      if (this.plan.ExpenditurePlanType < 200) {
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
  created() {
    console.log(this.id);
    if (this.id && this.id != "create") {
      this.api({ url: "/api/v1/expenditureplans/" + this.id }).then((res) => {
        this.plan = res;
      });
    }
    // if (this.formMode == Enum.FormMode.UPDATE && this.planEdit) {
    //   this.plan = this.planEdit;
    // }
  },
  methods: {
    onSubmitForm() {
      if (this.onValidate()) {
        var method = this.isAdd ? "POST" : "PUT";
        var url = this.isAdd
          ? "/api/v1/expenditureplans"
          : `/api/v1/expenditureplans/${this.id}`;
        this.api({
          url: url,
          data: this.plan,
          method: method,
        }).then((res) => {
          console.log(res);
          //   this.$emit("onSaveSuccess");
          //   this.$emit("update:formMode", Enum.FormMode.ADD);
          router.push("/expenditures");
        });
      }
    },
    onValidate() {
      try {
        var errors = [];
        if (!this.plan.ExpenditurePlanName) {
          errors.push("Tên kế hoạch không được phép để trống.");
        }
        if (!this.plan.ExpenditurePlanType) {
          errors.push("Loại kế hoạch không được để trống.");
        }

        if (
          this.plan.ExpenditurePlanType ==
            Enum.ExpenditurePlanType.INCREMENT_EVENT &&
          !this.plan.EventId
        ) {
          errors.push("Sự kiện cho khoản thu/chi không được phép để trống.");
        }

        if (this.isIncrement && !this.plan.AmountUnit) {
          errors.push("Số tiền thu/người không được phép để trống.");
        }
        if (errors.length > 0) {
          this.commonJs.showMessenger({
            title: "Dữ liệu không hợp lệ",
            msg: errors,
            type: Enum.MsgType.Error,
            confirm: () => {},
          });
          return false;
        }
        return true;
      } catch (error) {
        console.log(error);
      }
    },
    onClose(){
        router.push("/expenditures?isReload");
    }
  },
  data() {
    return {
      plan: { ExpenditurePlanType: Enum.ExpenditurePlanType.INCREMENT_EVENT },
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