<template>
  <div class="contact-container">
    <div class="contact__search">
      <form id="frmSearch" @submit.prevent="search">
        <span
          v-if="textSearch != null && textSearch != ''"
          @click="textSearch = ''"
          class="clear-input"
          ><i class="icofont-close-line"></i
        ></span>
        <input
          class="input--search"
          v-model="textSearch"
          placeholder="Nhập họ tên hoặc số điện thoại"
          type="text"
        />
        <button type="submit" form="frmSearch" class="btn">Tìm kiếm</button>
      </form>
    </div>
    <div class="contact__list">
      <table class="table" border="0" width="100%">
        <thead>
          <tr>
            <th>#</th>
            <th>Họ và tên</th>
            <th>Số điện thoại</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(contact, index) in contactsFilter" :key="index" @dblclick="onSelectContact(contact)">
            <td class="th-avatar">
              <img
                :src="contact.AvatarFullPath || 'avatar.png'"
                alt=""
                @click="onSelectContact(contact)"
              />
            </td>
            <td>
              <a @click="onSelectContact(contact)">{{ contact.FirstName }} <b>{{ contact.LastName }}</b></a>
            </td>
            <td>{{ contact.PhoneNumber }}<br />{{ contact.MobileNumber }}</td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
  <div v-if="loading" class="loading">
    <div class="loading__icon"></div>
  </div>
  <contact-detail
    v-if="detailFormMode != null"
    v-model:formMode="detailFormMode"
    :contactInput="contactSelected"
    @afterSave="afterSave"
  ></contact-detail>
</template>
<script>
import ContactDetail from "./ContactDetail.vue";
export default {
  name: "ContactList",
  components: { ContactDetail },
  props: [],
  emits: [],
  created() {
    this.loadData();
  },
  watch: {
    textSearch: function () {
      this.search();
    },
  },
  methods: {
    loadData() {
      // Lấy danh sách liên hệ
      this.loading = true;
      var baseUrl = process.env.VUE_APP_BASE_URL;
      fetch(baseUrl + "/api/v1/contacts")
        .then((res) => res.json())
        .then((res) => {
          this.contacts = res;
          this.contactsFilter = res;
          this.loading = false;
        })
        .catch((res) => {
          console.log(res);
          this.loading = false;
        });
    },
    /**
     * Thực hiện tìm kiếm danh bạ
     * Author: NVMANH (01/10/2022)
     */
    search() {
      var me = this;
      var key = this.commonJs.change_alias(
        this.textSearch.replace(" ", "").toLowerCase()
      );
      this.contactsFilter = this.contacts.filter((item) => {
        var fullName = me.commonJs.change_alias(
            (item.FirstName + item.LastName).replace(" ", "").toLowerCase()
          ),
          mobile = item.MobileNumber,
          phone = item.PhoneNumber;
        return fullName.match(key) || mobile.match(key) || phone.match(key);
      });
    },
    onSelectContact(contact) {
      console.log(contact);
      this.contactSelected = contact;
      this.detailFormMode = this.Enum.FormMode.VIEW;
    },
    afterSave() {
      this.loadData();
      this.detailFormMode = this.Enum.FormMode.VIEW;
    },
  },
  data() {
    return {
      contactsFilter: [],
      contacts: [],
      textSearch: "",
      loading: false,
      detailFormMode: null,
      contactSelected: {},
    };
  },
};
</script>
<style scoped>
.contact-container {
  min-width: 300px;
  box-sizing: border-box;
}
.contact__search {
  width: 100%;
  box-sizing: border-box;
}
.contact__search form {
  position: relative;
  position: sticky;
  display: flex;
  top: 0;
  background-color: #fff;
  box-sizing: border-box;
}

.contact__search input {
  flex: 1;
}
.contact__search button {
  margin-left: 8px;
  white-space: nowrap;
  border: none;
  border-radius: 4px;
  outline: none;
  background-color: rgb(0, 93, 186);
  padding: 0 16px;
  color: #fff;
  cursor: pointer;
  box-sizing: border-box;
}

.contact__search button:hover {
  background-color: rgb(0, 128, 255);
}

.clear-input {
  width: 20px;
  height: 20px;
  font-size: 16px;
  position: absolute;
  z-index: 999;
  background-color: #dedede;
  color: #fff;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  right: 106px;
  top: 50%;
  transform: translate(0, -50%);
  cursor: pointer;
}
.contact__list {
  margin-top: 10px;
  height: calc(100% - 40px);
  overflow-y: auto;
}
thead {
}
thead th {
  position: sticky;
  top: -1px;
  background-color: #fff;
}
.th-avatar {
  margin-left: auto !important;
  margin-right: auto !important;
  width: 100% !important;
}

td img {
  width: 150px;
}

table {
  border-collapse: collapse;
  width: 100%;
  border: unset;
}

table td,
th {
  white-space: nowrap;
  padding: 0 16px;
  border-bottom: 1px solid #ccc;
  text-align: left;
}
table tr {
  height: 48px;
  cursor: pointer;
}

table tr:hover {
  background-color: #dedede;
}
</style>
