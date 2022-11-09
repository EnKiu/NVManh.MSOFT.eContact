<template>
  <the-header
    v-if="isAuthenticated"
    :isAuthenticated="isAuthenticated"
    :isProfileLoaded="isProfileLoaded"
    :classInfo="classInfo"
  ></the-header>
  <the-main></the-main>
  <router-view name="LoginPage"></router-view>
  <router-view class="register" name="Register"></router-view>
  <router-view name="HomePage"></router-view>
  <router-view name="Other"></router-view>

  <MLoading v-if="isShowLoading" />
  <MLoading v-if="connectingHub" />
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

  <news-list v-if="isAuthenticated && showNew" @onClose="showNew = false"></news-list>
  <progress-bar v-if="isShowProgressBar" :processList="processList"></progress-bar>
</template>

<script>
// import HomePage from './views/Index.vue'
import TheHeader from "./components/layout/TheHeader.vue";
import TheMain from "./components/layout/TheMain.vue";
import NewsList from "./News.vue";
import { USER_REQUEST } from "./store/actions/user";
import { mapGetters, mapState } from "vuex";
import MDialogNotification from "./components/base/MDialogNotification.vue";
import MToast from "./components/base/MToast.vue";
import ProgressBar from "./components/base/ProgressBar.vue";
// import notification from "./http/WebSocket";
export default {
  name: "App",
  components: {
    TheHeader,
    TheMain,
    MToast,
    MDialogNotification,
    NewsList,
    ProgressBar,
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
      "connectingHub",
      "msgType",
      "confirmFunction",
      "role",
      "isShowToast",
      "msgToast",
      "msgToastType",
      "isShowProgressBar",
      "processList",
      "classInfo",
    ]),
    ...mapState({
      authLoading: (state) => state.auth.status === "loading",
      name: (state) => `${state.user.profile.title} ${state.user.profile.name}`,
    }),
  },
  async created() {
    if (this.$store.getters.isAuthenticated) {
      await this.$store.dispatch(USER_REQUEST);
      // this.hubConnection = notification.createHub();
      // // this.hubConnection = notification.CreateHubProxy();
      // this.hubConnection
      //   .start()
      //   .then(() => {
      //     console.log("Đã kết nối tới Hub...");
      //   })
      //   .catch((err) => console.error(err));
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
      showHomePage: false,
      showNew: false,
      progressing: false,
    };
  },
};
</script>

<style>
@import url(./styles/main.css);
</style>
