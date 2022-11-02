<template>
  <div class="login">
    <div class="login-container">
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
  methods: {
    onLogin() {
      this.commonJs.login(this.user.UserName, this.user.Password);
    },
  },
  data() {
    return {
      validated: false,
      user: {},
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
}

.login-container {
  min-width: 200px;
  background-color: #fff;
  padding: 24px;
  border-radius: 4px;
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
</style>
