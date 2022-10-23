<template>
  <div class="dialog-comment">
    <div class="comment__content">
      <button class="close-comment" @click="onClose">
        <i class="icofont-close"></i>
      </button>
      <div class="comment__title">{{ fullName }}:</div>
      <div class="comment">{{ comment }}</div>
    </div>
  </div>
</template>
<script>
export default {
  name: "EventComment",
  props: ["comment", "fullName"],
  methods: {
    onClose() {
      this.$emit("update:comment", null);
    },
  },
};
</script>
<style scoped>
.dialog-comment {
  position: fixed;
  top: 0;
  left: 0;
  bottom: 0;
  right: 0;
  background-color: #0000003c;
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 1005;
}

.comment__content {
  max-width: 300px;
  position: relative;
  background-color: #fff;
  border-radius: 4px;
  box-shadow: 0px 3px 6px #dedede;
  padding: 24px;
}
.comment__title {
  font-weight: 700;
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
}

.comment {
  font-style: italic;
}
</style>
