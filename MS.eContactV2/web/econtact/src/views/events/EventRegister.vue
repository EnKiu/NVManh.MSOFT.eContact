<template>
  <m-dialog title="Thông tin đăng ký">
    <template v-slot:content>
      <!-- <el-autocomplete
        v-model="state1"
        :fetch-suggestions="querySearch"
        clearable
        class="inline-input w-100"
        placeholder="Nhập và chọn Họ và tên của bạn"
        @select="handleSelect"
      /> -->
      <div class="m-row">
        <m-combobox
          label="Chọn thành viên đăng ký"
          :url="url"
          v-model="contact.ContactId"
          :required="true"
          :isDisabled="!isAdmin || disabledSelectContact"
          propValue="ContactId"
          propText="FullName"
        >
        </m-combobox>
      </div>
      <div class="m-row">
        <label for=""
          >Số người đi kèm (<span class="--color-red">*</span>) - vợ/chồng/con...</label
        >
        <div>
          <el-input-number
            label="Đính kèm"
            v-model="contact.NumberAccompanying"
            :min="0"
            :max="10"
            @change="handleChangeNumberAccompanying"
          />
        </div>
      </div>
      <div class="m-row">
        <m-input
          label="Số tiền cần nộp"
          :isFocus="true"
          :required="true"
          class="mg-bottom-0"
          type="number"
          v-model="contact.SpendsTotal"
        ></m-input>
        <div style="text-align: right">
          <b>{{ moneyFormat }}</b>
        </div>
      </div>
      <div class="m-row" style="margin-top: 10px">
        <m-text-area
          label="Ghi chú/ đóng góp ý kiến"
          textPlaceholder="Bạn có thể ghi rõ thông tin người đi kèm, hoặc các ý kiến đóng góp khác cho BTC."
          v-model="contact.Note"
        ></m-text-area>
      </div>
    </template>
    <template v-slot:footer>
      <button
        v-if="formMode == Enum.FormMode.ADD"
        class="btn btn--default"
        @click="onRegister"
      >
        <i class="icofont-ui-add"></i> Đăng ký
      </button>
      <button
        v-if="formMode == Enum.FormMode.UPDATE"
        class="btn btn--default"
        @click="onUpdateRegister"
      >
        <i class="icofont-ui-check"></i> Lưu
      </button>
    </template>
  </m-dialog>
</template>
<script>
import Enum from "@/scripts/enum";
export default {
  name: "EventRegister",
  props: ["eventRegister", "register", "formMode", "isAdmin"],
  emits: ["onRegisterSuccess", "update:TotalAccompanying", "update:TotalMember"],
  created() {
    if (this.formMode == Enum.FormMode.ADD) {
      this.contact.ContactId = localStorage.getItem("contactId");
      this.url =
        "/api/v1/events/contact-no-register?eventId=" + this.eventRegister.EventId;
    } else if (this.formMode == Enum.FormMode.UPDATE) {
      this.contact = this.register;
      this.url = "/api/v1/contacts";
      this.disabledSelectContact = true;
    }

    var fee = this.eventRegister.Spends;
    this.contact.SpendsTotal = fee + fee * this.contact.NumberAccompanying;
    // this.loadData();
  },
  computed: {
    moneyFormat: function () {
      return this.commonJs.formatMoney(this.contact.SpendsTotal);
    },
  },
  watch: {
    contact: {
      handler: function () {
        var fee = this.eventRegister.Spends;
        this.contact.SpendsTotal = fee + fee * this.contact.NumberAccompanying;
      },
      deep: true,
    },
  },
  methods: {
    onValidate() {
      // Thực hiện validate dữ liệu:
      var isValid = true;
      var msgErrors = [];
      this.contact.EventId = this.eventRegister.EventId;
      if (!this.contact.ContactId) {
        msgErrors.push("Thông tin người đăng ký không được phép trống.");
      }
      if (this.contact.NumberAccompanying == null) {
        msgErrors.push("Số người đính kèm không được để trống.");
      }
      if (msgErrors.length > 0) {
        isValid = false;
        this.commonJs.showMessenger({
          title: "Thiếu dữ liệu",
          msg: msgErrors,
          type: this.Enum.MsgType.Error,
        });
      }
      return isValid;
    },
    /**
     * Thực hiện đăng ký sự kiện
     * Author: NVMANH (04/10/2022)
     */
    onRegister() {
      if (this.onValidate()) {
        this.api({
          url: "/api/v1/EventDetails",
          data: this.contact,
          method: "POST",
          showToast: false,
        }).then(() => {
          this.commonJs.showMessenger({
            title: "Thành công",
            msg: "Chúc mừng! bạn đã đăng ký tham gia sự kiện thành công!",
            type: this.Enum.MsgType.Success,
          });
          // Cập nhật lại thông tin sự kiện (số người đăng ký và đi kèm)
          var totalAccomanying =
            this.eventRegister.TotalAccompanying + this.contact.NumberAccompanying;
          var totalMember = this.eventRegister.TotalMember + 1;
          this.$emit("update:TotalAccompanying", totalAccomanying);
          this.$emit("update:TotalMember", totalMember);
          this.$emit("onRegisterSuccess", this.contact, this.eventRegister);
        });
      }
      // Thực hiện thêm đăng ký mới:
      this.contact.EventId = this.eventRegister.EventId;
    },
    onUpdateRegister() {
      if (this.onValidate()) {
        this.api({
          url: "/api/v1/EventDetails/" + this.contact.EventDetailId,
          data: this.contact,
          method: "PUT",
          showToast: true,
        }).then(() => {
          // Cập nhật lại thông tin sự kiện (số người đăng ký và đi kèm)
          var totalAccomanying =
            this.eventRegister.TotalAccompanying + this.contact.NumberAccompanying;
          var totalMember = this.eventRegister.TotalMember + 1;
          this.$emit("update:TotalAccompanying", totalAccomanying);
          this.$emit("update:TotalMember", totalMember);
          this.$emit("onRegisterSuccess", this.contact, this.eventRegister);
        });
      }
    },
    handleChangeNumberAccompanying() {},
    querySearch: function (queryString, cb) {
      var me = this;
      var contacts = this.contacts;
      const results = queryString
        ? contacts.filter(me.createFilter(queryString))
        : contacts;
      // call callback function to return suggestions
      cb(results);
    },
    createFilter(queryString) {
      return (contact) => {
        return contact.FirstName.toLowerCase().indexOf(queryString.toLowerCase()) === 0;
      };
    },
    handleSelect: (item) => {
      console.log(item);
    },
    loadData() {
      var baseUrl = process.env.VUE_APP_BASE_URL;
      fetch(baseUrl + "/api/v1/events/contact-no-register?eventId=" + 1)
        .then((res) => res.json())
        .then((res) => {
          this.contacts = res;
        })
        .catch((res) => console.log(res));
    },
  },
  data() {
    return {
      state1: [],
      contacts: [],
      contact: { NumberAccompanying: 0 },
      url: null,
      disabledSelectContact: false,
    };
  },
};
</script>
<style scoped>
.m-row + .m-row {
  margin-top: 10px;
}
.mg-bottom-0 {
  margin-bottom: 4px !important;
}
</style>
