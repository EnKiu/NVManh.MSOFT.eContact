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
        <m-input label="Số người đính kèm" v-model="contact.NumberAccompanying" type="number" :required="true"></m-input>
      </div>
      <div class="m-row">
        <m-text-area label="Ghi chú/ đóng góp ý kiến"  v-model="contact.Note"></m-text-area>
      </div>
    </template>
    <template v-slot:footer>
      <button class="btn btn--default" @click="onRegister"><i class="icofont-ui-add"></i> Đăng ký</button>
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
    onRegister(){
      // Thực hiện validate dữ liệu:
      if(!this.contact.NumberAccompanying){
        this.commonJs.showMessenger({title:"Lỗi",msg:"Vui lòng nhập số người đính kèm",type:this.Enum.MsgType.Error});
      }
      // Thực hiện thêm đăng ký mới:
      this.contact.EventId = this.eventRegister.EventId;
    },
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
      contact: {},
      url: null,
    };
  },
};
</script>
<style scoped>

  .m-row+ .m-row{
    margin-top: 10px;
  }
</style>