<template>
  <the-header :isAuthenticated="isAuthenticated"></the-header>
  <the-main></the-main>
  <router-view name="LoginPage"></router-view>
  <router-view class="register" name="Register"></router-view>
  <MLoading v-if="isShowLoading" />
  <router-view name="HomePage"></router-view>
  <!-- <home-page v-if="!isAuthenticated && showHomePage" v-model:showHomePage="showHomePage"></home-page> -->
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
// import HomePage from './views/Index.vue'
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
  data() {
    return {
      showHomePage: false
    }
  },
}
</script>

<style>
@import url(./styles/main.css);
</style>
