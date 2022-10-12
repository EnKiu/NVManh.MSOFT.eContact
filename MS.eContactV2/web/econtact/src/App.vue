<template>
  <the-header></the-header>
  <the-main></the-main>
  <!-- <m-dialog>
    <template v-slot:content>
      <div class="notice">
        <div class="notice__icon --error">
          <i class="icofont-error"></i>
        </div>
        <div class="notice__text">
          Có lỗi xảy ra vui lòng liên hệ MISA để được trợ giúp!
        </div>
      </div>
    </template>
    <template v-slot:footer>
      <button class="btn btn--default"><i class="icofont-ui-add"></i> Đồng ý</button>
    </template>
  </m-dialog> -->
  <MLoading v-if="isShowLoading" />
<!-- <MLoading /> -->
  <MDialogNotification
    v-if="isShowError"
    :showCancelButton="showCancelButton"
    :title="titleNotification"
    :arrayMsgs="errorsNotification"
    :type="msgType"
    @btnCancelClick="hideNotice"
    @confirmFunction="confirmFunction"
  />
  <m-toast v-if="isShowToast" :msg="msgToast" :type="msgToastType"></m-toast>
</template>

<script>
import TheHeader from './components/layout/TheHeader.vue'
import TheMain from './components/layout/TheMain.vue'
import { USER_REQUEST } from "./store/actions/user";
import { mapGetters, mapState } from "vuex";
import MDialogNotification from "./components/base/MDialogNotification.vue";
import MToast from "./components/base/MToast.vue";
export default {
  name: 'App',
  components: {
    TheHeader,TheMain,MToast,MDialogNotification
  },
  computed: {
    ...mapGetters([
      "getProfile",
      "isAuthenticated",
      "isProfileLoaded",
      "isShowError",
      "errorsNotification",
      "titleNotification",
      "showCancelButton",
      "isShowLoading",
      "msgType",
      "confirmFunction",
      "role",
      "isShowToast",
      "msgToast",
      "msgToastType",
    ]),
    ...mapState({
      authLoading: (state) => state.auth.status === "loading",
      name: (state) => `${state.user.profile.title} ${state.user.profile.name}`,
    }),
  },
  async created() {
    if (this.$store.getters.isAuthenticated) {
      await this.$store.dispatch(USER_REQUEST);
    }
  },
  methods: {
    hideNotice() {
      this.commonJs.hideConfirm();
    },
    closeMessageBox() {
      this.showMessageBox = false;
    },
  },
}
</script>

<style>
@import url(./styles/main.css);
</style>
