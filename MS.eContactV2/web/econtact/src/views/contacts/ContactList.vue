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
        <!-- <m-column prop="FullName" label="#" width="45px">
          
        </m-column> -->
        <m-column prop="FullName" label="Họ và tên">
          <template #default="scope">
            <div class="flex">
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
              <div>{{ scope.row.FullName }}</div>
            </div>
          </template>
        </m-column>
        <m-column label="SĐT" width="90px">
          <template #default="scope">
            <div>{{ scope.row.PhoneNumber }}</div>
            <div>{{ scope.row.MobileNumber }}</div>
          </template>
        </m-column>
        <m-column v-if="isAdmin" fixed="right" width="55">
          <template #default="scope">
            <div class="button-column">
              <button
                class="btn-mini --color-edit"
                :title="scope.row.FullName"
                @click="onUpdate(scope.row)"
              >
                <i class="icofont-ui-edit"></i> <span style="font-size: 13px">Sửa</span>
              </button>
              <button
                class="btn-mini --color-red"
                :title="scope.row.FullName"
                @click="onDelete(scope.row)"
              >
                <i class="icofont-ui-delete"></i> <span style="font-size: 13px">Xóa</span>
              </button>
            </div>
          </template>
        </m-column>
      </m-table>
    </div>
    <div class="toobar-ext" v-if="isAdmin">
      <button class="btn btn--default" @click="onAddClick">
        <i class="icofont-ui-add"></i> Thêm thành viên
      </button>
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
import Enum from "@/scripts/enum";
import ContactDetail from "./ContactDetail.vue";
export default {
  name: "ContactList",
  components: { ContactDetail },
  props: [],
  emits: [],
  created() {
    var roleValue = localStorage.getItem("userRoleValue");
    if (roleValue == 1) {
      this.isAdmin = true;
    }
    this.loadData();
  },
  watch: {
    textSearch: function () {
      this.search();
    },
  },
  methods: {
    onAddClick() {
      this.detailFormMode = Enum.FormMode.ADD;
      this.contactSelected = {};
    },
    onDelete(contact) {
      this.commonJs.showConfirm(
        `Bạn có chắc chắn muốn xóa [${contact.FullName}]?`,
        () => {
          this.api({
            url: `/api/v1/contacts/${contact.ContactId}`,
            method: "DELETE",
          }).then(() => {
            this.loadData();
          });
        }
      );
      event.stopPropagation();
    },
    onUpdate(contact) {
      this.contactSelected = contact;
      this.detailFormMode = Enum.FormMode.UPDATE;
    },
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
      isAdmin: false,
    };
  },
};
</script>
<style scoped>
.contact-container {
  min-width: 300px;
  box-sizing: border-box;
  height: 100%;
  margin: 0 auto;
  display: flex;
  flex-direction: column;
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
  margin-top: 4px;
  /* height: calc(100% - 50px); */
  overflow-y: auto;
  border-radius: 0px;
  padding: 4px;
  background-color: #fff;
  flex: 1;
}

.avatar {
  width: 35px;
  height: 35px;
  background-repeat: no-repeat;
  background-position: center;
  background-size: cover;
  border-radius: 50%;
  flex-shrink: 0;
  flex-grow: 0;
  flex-basis: 35px;
  margin-right: 4px;
}

.toobar-ext {
  position: relative;
  margin-top: 4px;
  background-color: #fff;
  padding: 4px;
  z-index: 999;
  border-radius: 0 0 4px 4px;
}

.button-column {
  display: flex;
  flex-direction: column;
}

.btn-mini + .btn-mini {
  margin-left: 0;
}
</style>
