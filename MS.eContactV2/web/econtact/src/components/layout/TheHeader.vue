<template>
  <div class="page-title animate-charcter" style="">
    Cựu học sinh A1 (2004-2007) - trường thpt tứ sơn
  </div>
  <nav class="navbar">
    <div class="logo"></div>
    <div class="navbar-list">
      <router-link to="/" class="navbar-item">
        <span class="navbar-item__text"><i class="icofont-navigation-menu"></i></span>
        <!-- <span class="item__text-label">Tin tức</span> -->
      </router-link>
      <router-link to="/contacts" class="navbar-item">
        <span class="navbar-item__text"><i class="icofont-contacts"></i></span>
        <span class="item__text-label">Danh bạ</span>
      </router-link>
      <router-link to="/events" class="navbar-item">
        <span class="navbar-item__text"><i class="icofont-history"></i></span>
        <span class="item__text-label">Sự kiện</span>
      </router-link>
      <router-link to="/pictures" class="navbar-item">
        <span class="navbar-item__text"><i class="icofont-image"></i></span>
        <span class="item__text-label">Ảnh</span>
      </router-link>
      <a
        v-if="isAuthenticated"
        class="navbar-item account"
        style="flex-direction: row"
        @click="showAccountOption = !showAccountOption"
        v-clickoutside="hideListAccountOption"
      >
        <div
          class="navbar-item__avatar"
          :style="{
            'background-image': `url(${account.AvatarFullPath || 'avatar.png'})`,
          }"
        ></div>
        <!-- <span>{{ account.LastName }}</span> -->
        <div v-if="showAccountOption" class="account-option">
          <router-link to="/account" class="option-item"
            ><i class="icofont-info-circle"></i> Thông tin</router-link
          >
          <div class="option-item"><i class="icofont-key"></i> Đổi mật khẩu</div>
          <div class="option-item" @click="logOut">
            <i class="icofont-sign-out"></i> Đăng xuất
          </div>
        </div>
      </a>
      <router-link v-else to="/login" class="navbar-item">
        <span class="navbar-item__text"><i class="icofont-login"></i> Đăng nhập</span>
      </router-link>
    </div>
  </nav>
  <div class="class-info">
    <div class="info-item">Sĩ số: {{ classInfo.TotalMembers }}</div>
    <div class="info-item">
      <span class="flipX"
        >Quỹ lớp:
        <span class="total-money"
          >+{{ commonJs.formatMoney(classInfo.TotalMoneys) }}</span
        ></span
      >
    </div>
    <div class="expenditure" id="expenditure">
      <router-link to="/expenditures" class="navbar-item">
        <span><i class="icofont-chart-arrows-axis"></i>Báo cáo Thu/Chi</span>
      </router-link>
    </div>
  </div>
</template>
<script>
/* eslint-disable */
/**
 * Gán sự kiện nhấn click chuột ra ngoài combobox data (ẩn data list đi)
 * NVMANH (31/07/2022)
 */
const clickoutside = {
  mounted(el, binding, vnode, prevVnode) {
    el.clickOutsideEvent = (event) => {
      // Nếu element hiện tại không phải là element đang click vào
      // Hoặc element đang click vào không phải là button trong combobox hiện tại thì ẩn đi.
      if (
        !(
          (
            el === event.target || // click phạm vi của combobox__data
            el.contains(event.target) || //click vào element con của combobox__data
            el.previousElementSibling.contains(event.target)
          ) //click vào element button trước combobox data (nhấn vào button thì hiển thị)
        )
      ) {
        // Thực hiện sự kiện tùy chỉnh:
        binding.value(event, el);
        // vnode.context[binding.expression](event); // vue 2
      }
    };
    document.body.addEventListener("click", el.clickOutsideEvent);
  },
  beforeUnmount: (el) => {
    document.body.removeEventListener("click", el.clickOutsideEvent);
  },
};
import { AUTH_LOGOUT } from "../../store/actions/auth";
export default {
  name: "TheHeader",
  directives: {
    clickoutside,
  },
  props: ["isAuthenticated", "isProfileLoaded", "classInfo"],
  created() {
    if (this.isAuthenticated) {
      this.account.AvatarFullPath = localStorage.getItem("avatar");
      this.account.LastName = localStorage.getItem("lastName");
    }
  },
  watch: {
    isProfileLoaded: function (newValue) {
      if (newValue) {
        this.account.AvatarFullPath = localStorage.getItem("avatar");
        this.account.LastName = localStorage.getItem("lastName");
      }
    },
  },
  methods: {
    logOut() {
      this.hubConnection.invoke("RemoveConnection");
      this.$store.dispatch(AUTH_LOGOUT);
    },
    hideListAccountOption() {
      this.showAccountOption = false;
    },
  },
  data() {
    return {
      account: { AvatarFullPath: null },
      showAccountOption: false,
    };
  },
};
</script>

<style scoped>
.page-title {
  position: fixed;
  top: 0;
  width: 100%;
  height: 20px;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  background-color: black;
  color: #fff;
  text-align: center;
  text-transform: uppercase;
  font-weight: 700;
}
.navbar {
  position: fixed;
  top: 20px;
  width: 100%;
  display: flex;
  align-items: center;
  justify-content: center;
  height: 40px;
  background-color: #000;
  color: #fff;
  border-top: 1px solid #fff;
  z-index: 999;
}
.navbar-list {
  height: 100%;
  display: flex;
  align-items: center;
}
.navbar-item {
  height: 100%;
  padding: 0 16px;
  display: flex;
  align-items: center;
  justify-content: center;
  font-weight: 700;
}

.navbar-item i {
  font-size: 16px;
  margin-right: 5px;
}
.router-link-active {
  background-color: #656565cc;
}

.nav-account {
  height: 100%;
}
.navbar a {
  color: #fff;
  text-decoration: none;
}

.navbar-item__avatar {
  width: 24px;
  height: 24px;
  background-size: cover;
  background-position: center;
  border-radius: 50%;
  /* margin-right: 10px; */
  background-repeat: no-repeat;
}

.account {
  position: relative;
  cursor: pointer;
  background-color: #004982;
}
.account-option {
  position: absolute;
  top: calc(100% + 1px);
  z-index: 9999;
  background-color: #000000;
  box-shadow: 0 3px 6px #ccc;
}

.option-item {
  padding: 10px;
  white-space: nowrap;
  display: block;
}

.option-item:hover {
  background-color: #4a4a4a;
}

.class-info {
  height: 25px;
  width: 100%;
  top: 60px;
  position: fixed;
  background-color: #fffb02;
  display: flex;
  align-items: center;
  justify-content: center;
  font-weight: 700;
  padding: 0 24px;
}

.info-item + .info-item {
  margin-left: 10px;
}

.total-money {
  color: #0c701e;
}

.expenditure a{
  text-decoration: none;
  margin: 0 10px;
}

.expenditure .router-link-active{
  color: #fff;
  background-color: #ff0000;
}
@media screen and (max-width: 411px) {
  /* .item__text-label{
    display: none;
  } */
  .navbar-item {
    padding: 4px 10px;
    box-sizing: border-box;
  }
}
</style>
