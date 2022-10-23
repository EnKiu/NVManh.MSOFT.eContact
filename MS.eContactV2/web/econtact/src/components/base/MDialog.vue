<template>
  <div class="dialog">
    <div class="dialog-wrapper">
      <button class="dialog__button-close" @click="onClose">
        <i class="icofont-close"></i>
      </button>
      <div class="dialog__header">
        <div class="dialog__header--title">{{ title }}</div>
      </div>
      <div class="dialog__content">
        <slot name="content"></slot>
      </div>
      <div class="dialog__footer">
        <slot name="footer"></slot>
        <button v-if="showButton" class="btn btn--cancel dialog__button--close"></button>
        <button
          v-if="showButton"
          class="btn btn-default dialog__button--save"
          @click="onSubmit"
        >
          {{ submitText }}
        </button>
      </div>
    </div>
  </div>
</template>
<script>
export default {
  name: "BaseDialog",
  emits: ["onSubmit", "onClose"],
  props: {
    title: {
      type: String,
      default: "Thông tin chi tiết",
      required: false,
    },
    showButton: {
      type: Boolean,
      default: false,
      required: false,
    },
    submitText: {
      type: String,
      default: "Lưu",
      required: false,
    },
  },
  methods: {
    onSubmit() {
      this.$emit("onSubmit");
    },
    onClose() {
      this.$emit("onClose");
    },
  },
};
</script>
<style scoped>
.dialog {
  position: fixed;
  max-width: 100%;
  display: flex;
  align-items: center;
  justify-content: center;
  background-color: rgba(0, 0, 0, 0.278);
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  padding: 24px;
  z-index: 1005;
}
.dialog-wrapper {
  position: relative;
  background-color: #fff;
  border-radius: 4px;
  max-width: 100%;
  box-sizing: border-box;
}

.dialog__content {
  padding: 24px;
}
.dialog__header {
  padding: 24px 24px 0 24px;
  min-width: 300px;
  font-size: 20px;
  font-weight: 700;
}

/* .dialog__button-close {
  position: absolute;
    display: flex;
    align-items: center;
    justify-content: center;
    top: 24px;
    right: 24px;
    padding: unset;
    border: none;
    background-color: unset;
    font-size: 24px;
    cursor: pointer;
    font-weight: 700;
} */

.dialog__button-close {
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

.dialog__button-close:hover {
  color: #ff0000;
}

.dialog__footer {
  display: flex;
  align-items: center;
  justify-content: flex-end;
  padding: 0 24px 24px 24px;
}

.dialog__footer button {
  height: 36px;
  min-width: 75px;
  color: #fff;
  cursor: pointer;
}

.dialog__footer button + button {
  margin-left: 16px;
}
</style>
