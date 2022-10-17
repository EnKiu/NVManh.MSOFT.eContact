<template>
  <div class="page-title" style="">
    Website nội bộ cựu học sinh A1 (2004-2007) - trường thpt tứ sơn
  </div>
  <nav class="navbar">
    <div class="logo"></div>
    <div class="navbar-list">
      <router-link to="/contacts" class="navbar-item">
        <span class="navbar-item__text"
          ><i class="icofont-contacts"></i> Danh bạ</span
        >
      </router-link>
      <router-link to="/events" class="navbar-item">
        <span class="navbar-item__text"
          ><i class="icofont-history"></i> Sự kiện</span
        >
      </router-link>
      <router-link to="/pictures" class="navbar-item">
        <span class="navbar-item__text"
          ><i class="icofont-image"></i> Kho ảnh</span
        >
      </router-link>
      <a
        v-if="isAuthenticated"
        class="navbar-item account"
        @click="showAccountOption = !showAccountOption"
      >
        <div
          class="navbar-item__avatar"
          :style="{
            'background-image': `url(${
              account.AvatarFullPath || 'avatar.png'
            })`,
          }"
        ></div>
        <span>{{ account.LastName }}</span>
        <div v-if="showAccountOption" class="account-option">
          <router-link to="/account" class="option-item"
            ><i class="icofont-info-circle"></i> Thông tin</router-link
          >
          <div class="option-item">
            <i class="icofont-key"></i> Đổi mật khẩu
          </div>
          <div class="option-item" @click="logOut">
            <i class="icofont-sign-out"></i> Đăng xuất
          </div>
        </div>
      </a>
      <router-link v-else to="/login" class="navbar-item">
        <span class="navbar-item__text"
          ><i class="icofont-login"></i> Đăng nhập</span
        >
      </router-link>
    </div>
  </nav>
</template>
<script>
import { AUTH_LOGOUT } from "../../store/actions/auth";
export default {
  name: "TheHeader",
  props: ["isAuthenticated"],
  created() {
    if (this.isAuthenticated) {
      this.account.AvatarFullPath = localStorage.getItem("avatar");
      this.account.LastName = localStorage.getItem("lastName");
      console.log(this.account);
    }
  },
  watch: {
    isAuthenticated: function (newValue) {
      if (newValue) {
        this.account.AvatarFullPath = localStorage.getItem("avatar");
        this.account.LastName = localStorage.getItem("lastName");
        console.log(this.account);
      }
    },
  },
  methods: {
    logOut() {
      this.$store.dispatch(AUTH_LOGOUT);
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
  height: 40px;
  display: flex;
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
  top: 40px;
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
  background-size: contain;
  background-position: center;
  border-radius: 50%;
  margin-right: 10px;
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
</style>
