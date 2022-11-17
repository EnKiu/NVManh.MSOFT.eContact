<template>
  <div class="login">
    <div class="login-container">
      <div class="note">
        <ul>
          <li>
            Nếu bạn chưa có tài khoản vui lòng
            <router-link to="/register">Tạo tài khoản</router-link> trước.
          </li>
          <li>
            Bạn không thể Tạo tài khoản nếu bạn chưa có trong danh sách lớp. Hãy liên hệ
            Quản trị viên và yêu cầu bổ sung bạn vào danh sách lớp.
          </li>
        </ul>
      </div>
      <form class="login-form" id="FRM_LOGIN" @submit.prevent="onLogin">
        <m-input
          label="Số điện thoại"
          :required="true"
          :isFocus="true"
          v-model="user.UserName"
          placeholder="Nhập số điện thoại chính của bạn"
          v-model:validated="validated"
        ></m-input>
        <m-input
          label="Mật khẩu"
          placeholder="Mật khẩu"
          type="password"
          v-model="user.Password"
          autocomplete="password"
          :required="true"
          :validated="validated"
        ></m-input>
        <div class="form__button">
          <button id="btn-login" class="btn btn--default" submit="onLogin">
            <i class="icofont-login"></i> Đăng nhập
          </button>
        </div>
      </form>
      <div class="login-ext">
        <span
          >Bạn chưa có tài khoản? <i class="icofont-swoosh-right"></i>
          <router-link to="/register">Tạo tài khoản</router-link></span
        >
      </div>
      <div v-if="showSuggest" class="auth-info">
        <!-- <button class="close-comment" @click="onClose">
        <i class="icofont-close"></i>
      </button> -->
        Website được thiết kế bởi Mr Mạnh - A1 THPT Tứ Sơn (2004-2007). Nếu ứng dụng có
        lỗi hoặc bạn cần trợ giúp vui lòng liên hệ:
        <li>Mobile: <a href="tel:0961179969">0961179969</a></li>
        <li>Email: <a href="email:manhnv229@gmail.com">manhnv229@gmail.com</a></li>
        <li>Facebook: <a href="https://fb.com/manhnv229">Mạnh Nguyễn</a></li>
        <li>Website: <a href="https://manhnv.net">https://manhnv.net</a></li>
        Cám ơn bạn đã sử dụng ứng dụng!
      </div>
    </div>
  </div>
</template>
<script>
export default {
  name: "AccountLogin",
  components: {},
  emits: [],
  props: [],
  created() {},
  mounted() {
    // var me = this;
    // this.$nextTick(function () {
    //   setTimeout(function () {
    //     me.showSuggest = false;
    //   }, 20000);
    // });
  },
  methods: {
    onLogin() {
      this.commonJs.login(this.user.UserName, this.user.Password);
    },
    onClose() {
      this.showSuggest = false;
    },
  },
  data() {
    return {
      validated: false,
      user: {},
      showSuggest: true,
    };
  },
};
</script>
<style scoped>
.login {
  position: absolute;
  top: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.479);
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  box-sizing: border-box;
}

.login-container {
  min-width: 200px;
  max-width: 400px;
  background-color: #fff;
  padding: 16px;
  border-radius: 4px;
  margin: 10px;
  box-sizing: border-box;
}
.form__button {
  width: 100%;
  display: flex;
  justify-content: flex-end;
}
#btn-login {
  margin-top: 10px;
}

.login-ext {
  margin-top: 10px;
  text-align: right;
}

.note {
  max-width: 270px;
}

.auth-info a {
  color: rgb(255, 255, 255);
}

/* .auth-notice {
  position: fixed;
  top: 0;
  left: 0;
  bottom: 0;
  right: 0;
  background-color: #00000064;
  z-index: 1020;
} */
.auth-info {
  /* position: fixed;
  bottom: 10px;
  z-index: 1020;
  margin: 0 10px; */
  /* width: calc(100% - 80px); */
  /* max-width: 500px; */
  background-color: rgb(0, 74, 117);
  padding: 16px;
  color: #fff;
  border-radius: 4px;
  margin-top: 10px;
}
.close-comment {
  position: absolute;
  top: -10px;
  right: -10px;
  border-radius: 50%;
  width: 30px;
  height: 30px;
  font-size: 24px;
  display: flex;
  align-items: center;
  justify-content: center;
  cursor: pointer;
  color: #ff0000;
  border-color: #ff0000;
  border-style: solid;
}
</style>
