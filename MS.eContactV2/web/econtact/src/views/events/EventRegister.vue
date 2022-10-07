<template>
  <m-dialog title="Đăng ký" @onClose="onClose">
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
          propValue="ContactId"
          propText="FullName"
        >
        </m-combobox>
      </div>
      <div class="m-row">
        <label for=""
          >Số người đi kèm (<span class="--color-red">*</span>)</label
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
        <m-text-area
          label="Ghi chú/ đóng góp ý kiến"
          v-model="contact.Note"
        ></m-text-area>
      </div>
    </template>
    <template v-slot:footer>
      <button class="btn btn--default" @click="onRegister">
        <i class="icofont-ui-add"></i> Đăng ký
      </button>
    </template>
  </m-dialog>
</template>
<script>
export default {
  name: "EventRegister",
  props: ["eventRegister"],
  created() {
    this.url =
      "/api/v1/events/contact-no-register?eventId=" +
      this.eventRegister.EventId;
    // this.loadData();
  },
  methods: {
    /**
     * Thực hiện đăng ký sự kiện
     * Author: NVMANH (04/10/2022)
     */
    onRegister() {
      // Thực hiện validate dữ liệu:
      var msgErrors = [];
      this.contact.EventId = this.eventRegister.EventId;
      if (!this.contact.ContactId) {
        msgErrors.push("Thông tin người đăng ký không được phép trống.");
      }
      if (this.contact.NumberAccompanying == null) {
        msgErrors.push("Số người đính kèm không được để trống.");
      }
      if (msgErrors.length > 0) {
        this.commonJs.showMessenger({
          title: "Thiếu dữ liệu",
          msg: msgErrors,
          type: this.Enum.MsgType.Error,
        });
      } else {
        this.api({
          url: "/api/v1/EventDetails",
          data: this.contact,
          method: "POST",
        })
          .then(() => {
            this.commonJs.showMessenger({
              title: "Thành công",
              msg: "Chúc mừng! bạn đã đăng ký tham gia sự kiện thành công!",
              type: this.Enum.MsgType.Success,
            });
            this.$emit("onClose", this.contact,this.eventRegister);
          })
      }
      // Thực hiện thêm đăng ký mới:
      this.contact.EventId = this.eventRegister.EventId;
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
        return (
          contact.FirstName.toLowerCase().indexOf(queryString.toLowerCase()) ===
          0
        );
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
          console.log(res);
          this.contacts = res;
        })
        .catch((res) => console.log(res));
    },
    onClose() {
      this.$emit("onClose");
    },
  },
  data() {
    return {
      state1: [],
      contacts: [],
      contact: { NumberAccompanying: 0 },
      url: null,
    };
  },
};
</script>
<style scoped>
.m-row + .m-row {
  margin-top: 10px;
}
</style>