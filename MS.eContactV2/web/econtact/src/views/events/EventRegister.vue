<template>
  <m-dialog title="Đăng ký" @onClose="onClose">
    <template v-slot:content>
      <el-autocomplete
        v-model="state1"
        :fetch-suggestions="querySearch"
        clearable
        class="inline-input w-50"
        placeholder="Nhập và chọn Họ và tên của bạn"
        @select="handleSelect"
      />
    </template>
    <template v-slot:footer> </template>
  </m-dialog>
</template>
<script>
export default {
  name: "EventRegister",
  created() {
    this.loadData();
  },
  methods: {
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
      fetch(baseUrl + "/api/v1/events/contact-no-register?eventId="+1)
        .then((res) => res.json())
        .then((res) => {
          console.log(res);
          this.contacts = res;
        })
        .catch((res) => console.log(res));
    },
  },
  data() {
    return {
      state1: [],
      contacts: [],
    };
  },
};
</script>
<style scoped>
</style>