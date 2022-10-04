<template>
    <button ref="button" class="btn btn--default">{{ text }}
        <slot></slot>
    </button>
</template>

<script>
export default {
  name: "MButton",
  props: ["text","focus"],
  mounted() {
    if(this.focus == true)
      this.$refs["button"].focus();
  },
};
</script>

<style scoped>
@import url(../../styles/base/button.css);
</style>