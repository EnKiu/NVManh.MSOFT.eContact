<template>
  <div class="register">
    <div class="register-container">
      <div class="form__title">Tạo tài khoản mới</div>
      <form class="register-form" id="FRM_REGISTER" @submit.prevent="onRegister">
        <m-combobox
          label="Chủ tài khoản"
          url="/api/v1/contacts"
          placeholder="Chọn thành viên sử dụng tài khoản này"
          v-model="user.ContactId"
          :required="true"
          :isDisabled="false"
          :isFocus="true"
          propValue="ContactId"
          propText="FullName"
        >
        </m-combobox>
        <m-input
          label="Số điện thoại"
          :onlyNumberChar="true"
          :required="true"
          type="tel"
          name="username"
          v-model="user.UserName"
          :maxLength="10"
          placeholder="Nhập số điện thoại chính của bạn"
          v-model:validated="validated"
          @onBlur="onBlurPhoneInput"
        ></m-input>
        <m-input
          label="Mật khẩu"
          placeholder="Mật khẩu"
          type="password"
          v-model="user.Password"
          autocomplete="on"
          :required="true"
          v-model:validated="validated"
        ></m-input>
        <m-input
          label="Xác nhận Mật khẩu"
          placeholder="Xác nhận mật khẩu"
          type="password"
          v-model="user.RePassword"
          autocomplete="on"
          :required="true"
          v-model:validated="validated"
        ></m-input>
        <m-input
          label="Email"
          placeholder="VD: example@domain.com"
          type="email"
          v-model="user.Email"
          autocomplete="on"
        ></m-input>
        <div class="form__button">
          <button id="btn-register" class="btn btn--default" submit="onRegister">
            <i class="icofont-login"></i> Tạo tài khoản
          </button>
        </div>
      </form>
      <div class="register-ext">
        <span
          >Nếu bạn đã có tài khoản rồi <i class="icofont-swoosh-right"></i>
          <router-link to="/login">Đăng nhập</router-link></span
        >
      </div>
    </div>
  </div>
</template>
<script>
import Enum from "@/scripts/enum";
export default {
  name: "AccountRegister",
  components: {},
  emits: [],
  props: [],
  created() {},
  methods: {
    onRegister() {
      this.api({
        url: "/api/v1/accounts/register",
        data: this.user,
        method: "POST",
      }).then(() => {
        this.commonJs.showMessenger({
          title: "Tạo tài khoản thành công",
          msg: "Tài khoản đã được tạo thành công. Nhấn [Đồng ý] để tiến hành đăng nhập",
          type: Enum.MsgType.Success,
          confirm: this.onLogin,
          showCancelButton: true,
        });
      });
    },
    onLogin() {
      this.commonJs.login(this.user.UserName, this.user.Password);
    },
    onBlurPhoneInput() {
      console.log("onBlurPhoneInput");
      // Kiểm tra thông tin số điện thoại đã được đăng ký hoặc khớp với thành viên nào trong hệ thống:
      // var userName = this.user.UserName;
      // if (userName) {
      //   this.api({
      //     url: "/api/v1/accounts/register?phoneNumber=" + this.user.UserName,
      //   }).then((res) => {
      //     if (!res) {
      //       console.log("Không có dữ liệu");
      //       return;
      //     } else {
      //       console.log(res);
      //     }
      //   });
      // }
    },
  },
  computed: {
    isLockSelectContact: function () {
      if (!this.user.UserName) {
        return true;
      } else {
        return false;
      }
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
.register {
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

.register-container {
  min-width: 200px;
  background-color: #fff;
  padding: 24px;
  border-radius: 4px;
}
.register-form > div + div {
  /* margin-top: 10px; */
}
.form__title {
  font-size: 24px;
  font-weight: 700;
  margin-bottom: 10px;
}
.form__button {
  width: 100%;
  display: flex;
  justify-content: flex-end;
}
#btn-register {
  margin-top: 10px;
}

.register-ext {
  margin-top: 10px;
  text-align: right;
}
</style>
