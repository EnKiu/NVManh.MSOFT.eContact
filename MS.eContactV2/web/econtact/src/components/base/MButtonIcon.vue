<template>
  <button class="btn btn-icon">
    {{ text }}
    <slot></slot>
  </button>
</template>
<script>
export default {
  name: "MButtonIcon",
  props: ["text"],
};
</script>
<style scoped>
@import url(../../styles/base/button.css);
</style>