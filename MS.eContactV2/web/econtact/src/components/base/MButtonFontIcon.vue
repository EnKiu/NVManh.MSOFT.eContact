<template>
  <button class="btn btn-icon-font">
    <div class="button__icon-font"><slot></slot></div>
    <span class="button__text">{{text}}</span>
  </button>
</template>
<script>
export default {
  name: "MButtonIcon",
  props: ["text", "icon-cls"],
};
</script>
<style scoped>
@import url(../../styles/base/button.css);
.btn-icon-font{
  display: flex;
  align-items: center;
  justify-content: flex-start;
}
.button__icon-font{
  width: 20px;
  height: 20px;
  font-size: 20px;
  text-align: center;
  line-height: 20px;
  margin-right: 8px;
}
</style>