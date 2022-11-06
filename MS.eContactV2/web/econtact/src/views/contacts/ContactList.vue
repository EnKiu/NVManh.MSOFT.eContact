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
      <m-table
        ref="tbListDocument"
        :data="contactsFilter"
        empty-text="Không có dữ liệu"
        @row-click="onSelectContact"
        width="100%"
        height="100%"
      >
        <m-column prop="FullName" label="#" width="68px">
          <template #default="scope">
            <div
              class="avatar"
              @click="onSelectContact(scope.row)"
              :style="{
                'background-image': `url(${
                  scope.row.AvatarFullPath ||
                  'https://file.nmanh.com/e-contact/Content/imgs/avatar.png'
                })`,
              }"
            ></div>
          </template>
        </m-column>
        <m-column prop="FullName" label="Họ và tên" width="160px"></m-column>
        <m-column label="Số điện thoại">
          <template #default="scope">
            <div>{{ scope.row.PhoneNumber }}</div>
            <div>{{ scope.row.MobileNumber }}</div>
          </template>
        </m-column>
      </m-table>
    </div>
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
      this.api({
        url: "/api/v1/contacts",
      })
        .then((res) => {
          this.contacts = res;
          this.contactsFilter = res;
        })
        .catch((res) => {
          console.log(res);
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
            (item.FullName || "").replace(" ", "").toLowerCase()
          ),
          mobile = item.MobileNumber || "",
          phone = item.PhoneNumber || "";
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
  height: 100%;
  max-width: 350px;
  margin: 0 auto;
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
  z-index: 900;
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

.avatar {
  width: 50px;
  height: 50px;
  background-repeat: no-repeat;
  background-position: center;
  background-size: cover;
  border-radius: 50%;
}
</style>
