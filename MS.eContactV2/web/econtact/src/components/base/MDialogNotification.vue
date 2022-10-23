<template>
  <div class="dialog dialog-notification">
    <div class="dialog-wapper">
      <button class="dialog__button--close" @click="btnCloseDialogClick">
        <i class="icofont-close"></i>
      </button>
      <div class="dialog__header">{{ title || "Thông báo" }}</div>
      <div class="dialog__body">
        <div class="notified-item">
          <div
            class="dialog__icon"
            :class="{
              'color--info': isInfo,
              'color--error': isError || isQuestion,
              'color--success': isSuccess,
              'color--warning': isWarning,
            }"
          >
            <i v-if="isError" class="icofont-error"></i>
            <i v-if="isInfo" class="icofont-info-circle"></i>
            <i v-if="isSuccess" class="icofont-verification-check"></i>
            <i v-if="isWarning" class="icofont-warning"></i>
            <i v-if="isQuestion" class="icofont-question-circle"></i>
          </div>
          <div class="dialog__text">
            <ul :class="{ 'dialog__text--single': arrayMsgs.length == 1 }">
              <li v-for="(item, index) in arrayMsgs" :key="index" class="text__item">
                <span>
                  {{ item }}
                </span>
              </li>
            </ul>
          </div>
        </div>
      </div>
      <div class="dialog__footer">
        <m-button
          :focus="true"
          class="dialog__button--confirm"
          text="Đồng ý"
          ref="btnconfirm"
          @click="btnConfirmClick"
        >
        </m-button>
        <m-button
          v-if="showCancelButton"
          class="dialog__button--cancel"
          text="Đóng"
          @click="btnCancelClick"
        >
        </m-button>
      </div>
    </div>
  </div>
</template>
<script>
import MISAEnum from "../../scripts/enum";
export default {
  name: "DialogDefault",
  props: {
    title: {
      type: String,
      default: "Thông báo",
      required: false,
    },
    showCancelButton: {
      type: Boolean,
      default: true,
      required: false,
    },
    text: {
      type: String,
      default: "",
      required: false,
    },
    arrayMsgs: { type: Array, required: true },
    visible: {
      type: Boolean,
      default: false,
    },
    type: Number,
  },
  created() {
    switch (this.type) {
      case MISAEnum.MsgType.Error:
        this.isError = true;
        this.isInfo = false;
        this.isSuccess = false;
        this.isWarning = false;
        this.isQuestion = false;
        break;
      case MISAEnum.MsgType.Warning:
        this.isError = false;
        this.isInfo = false;
        this.isSuccess = false;
        this.isWarning = true;
        this.isQuestion = false;
        break;
      case MISAEnum.MsgType.Success:
        this.isError = false;
        this.isInfo = false;
        this.isSuccess = true;
        this.isWarning = false;
        this.isQuestion = false;
        break;
      case MISAEnum.MsgType.Question:
        this.isError = false;
        this.isInfo = false;
        this.isSuccess = false;
        this.isWarning = false;
        this.isQuestion = true;
        break;
      default:
        this.isError = false;
        this.isInfo = true;
        this.isSuccess = false;
        this.isWarning = false;
        this.isQuestion = false;
        break;
    }
  },
  mounted() {
    // this.$refs["btnconfirm2"].focus()
  },
  data() {
    return {
      isShow: true,
      isError: false,
      isInfo: false,
      isSuccess: false,
      isWarning: true,
      isQuestion: false,
    };
  },
  methods: {
    btnConfirmClick() {
      this.$emit("confirmFunction");
      this.commonJs.hideErrorMessenger();
    },

    btnCloseDialogClick() {
      this.$emit("closeDialog");
      this.commonJs.hideErrorMessenger();
    },

    btnCancelClick() {
      this.isShow = false;
      this.$emit("btnCancelClick");
    },
    showDialog() {
      this.isShow = true;
    },
    hideDialog() {
      this.isShow = false;
    },
  },
};
</script>
<style scoped>
.dialog {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-color: rgba(0, 0, 0, 0.65);
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 24px;
}

.dialog-wapper {
  position: relative;
  min-width: 250px;
  padding: 24px;
  background-color: #ffffff;
  border-radius: 4px;
}

.dialog__header {
  font-size: 20px;
  font-weight: 700;
}

.dialog__body {
  padding: 24px 0;
  display: flex;
  align-items: flex-start;
  justify-content: flex-start;
}

.dialog__footer {
  display: flex;
  justify-content: flex-end;
}

.dialog__footer button ~ button {
  margin-right: 8px;
}

.dialog__icon {
  width: 24px;
  height: 24px;
  font-size: 24px;
  line-height: 24px;
  color: #ff0000;
  float: left;
  margin-right: 12px;
}

/* .dialog__button--close {
    border: none;
    background-color: #fff;
    outline: none;
    display: flex;
    align-items: center;
    justify-content: center;
    font-size: 24px;
    line-height: 24px;
    text-align: center;
    position: absolute;
    right: 24px;
    top: 24px;
    cursor: pointer;
} */

.dialog__button--close {
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
  outline: none;
  border-style: solid;
}

.dialog__button--close:hover {
  color: #ff0000;
}

.dialog__button--confirm {
  order: 2;
}

.dialog__button--cancel {
  order: 1;
  background-color: #fff;
  color: #000000;
  border: 1px solid #dddddd;
  box-sizing: border-box;
}

.dialog__button--cancel:hover {
  background-color: #dddddd;
}
.dialog-notification {
  z-index: 1055;
}
.dialog__body {
  display: flex;
  flex-direction: column;
  max-width: 400px;
}

.notified-item {
  display: flex;
  flex-direction: row;
  align-items: center;
  justify-content: flex-start;
  /* height: 24px;
  line-height: 24px; */
}

.notified-item + .notified-item {
  margin-top: 8px;
}

.dialog__icon {
  width: 32px;
  height: 32px;
  line-height: 32px;
  font-size: 32px;
}

.dialog__text {
  line-height: 16px;
}
.dialog__text ul {
  margin: 0;
  padding-left: 24px;
}
.dialog__text li {
  line-height: 22px;
}

.dialog__text--single {
  padding-left: 0 !important;
  padding-inline-start: 0;
}
.dialog__text--single li {
  list-style: none;
}
.color--info {
  color: #0062cc;
}

.color--error {
  color: #ff0000;
}

.color--success {
  color: #067933;
}
.color--warning {
  color: #ffd000;
}

.text__item + .text__item {
  margin-top: 4px;
}
.dialog__button--cancel {
  background-color: unset;
  color: #000;
  background-color: #dddddd;
}

.dialog__button--cancel:hover,
.dialog__button--cancel:focus {
  background-color: #8b8b8b;
}
</style>
