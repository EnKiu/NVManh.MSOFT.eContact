<template>
  <div class="toast-list">
    <div class="toast">
      <div
        class="toast__icon"
        :class="{
          'toast__icon--info': isInfo,
          'toast__icon--error': isError,
          'toast__icon--success': isSuccess,
          'toast__icon--warning': isWarning,
        }"
      >
        <i v-if="isError" class="icofont-exclamation-tringle"></i>
        <i v-if="isInfo" class="icofont-info-circle"></i>
        <i v-if="isSuccess" class="icofont-verification-check"></i>
        <i v-if="isWarning" class="icofont-warning"></i>
      </div>
      <div class="toast__text">{{ msg }}</div>
      <div
        class="toast__close"
        :class="{
          'toast__icon--info': isInfo,
          'toast__icon--error': isError,
          'toast__icon--success': isSuccess,
          'toast__icon--warning': isWarning,
        }"
      >
        <i class="icofont-close"></i>
      </div>
    </div>
  </div>
</template>
<script>
import MISAEnum from "@/scripts/enum";

export default {
  name: "MToast",
  props: ["type", "msg"],
  created() {
    switch (this.type) {
      case MISAEnum.MsgType.Error:
        this.isError = true;
        this.isInfo = false;
        this.isSuccess = false;
        this.isWarning = false;
        break;
      case MISAEnum.MsgType.Warning:
        this.isError = false;
        this.isInfo = false;
        this.isSuccess = false;
        this.isWarning = true;
        break;
      case MISAEnum.MsgType.Success:
        this.isError = false;
        this.isInfo = false;
        this.isSuccess = true;
        this.isWarning = false;
        break;
      default:
        this.isError = false;
        this.isInfo = true;
        this.isSuccess = false;
        this.isWarning = false;
        break;
    }
  },

  data() {
    return {
      isWarning: true,
      isError: false,
      isInfo: false,
      isSuccess: false,
    };
  },
};
</script>
<style scoped>
.toast-list {
  position: fixed;
  bottom: 16px;
  right: 16px;
  z-index: 9999;
}

.toast {
  display: flex;
  align-items: center;
  padding: 16px;
  background-color: #fff;
  border-radius: 4px;
  box-shadow: 0px 3px 6px #ccc;
  z-index: 9999;
}

.toast__icon {
  width: 24px;
  height: 24px;
  font-size: 24px;
  margin-right: 10px;
}

.toast__icon--error {
  color: #ff0000;
}

.toast__icon--success {
  color: #067933;
}

.toast__icon--warning {
  color: #ffd000;
}

.toast__icon--info {
  color: #0062cc;
}

.toast__close {
  width: 24px;
  height: 24px;
  font-size: 24px;
  margin-left: 16px;
  font-weight: 400;
  cursor: pointer;
  opacity: 0.5;
}

.toast__close:hover {
  opacity: 1;
}

.toast + .toast {
  margin-top: 8px;
}
</style>