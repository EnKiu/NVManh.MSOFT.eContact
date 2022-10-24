<template>
  <div class="progressbar">
    <div class="progressbar__content">
      <progress :value="value" :max="max"></progress>
      <div class="progressbar__info">
        <span v-if="message">{{ message }}</span
        ><span v-else>Đang xử lý {{ value }}/{{ max }} tệp</span>
      </div>
    </div>
  </div>
</template>
<script>
export default {
  name: "ProgressBar",
  props: {
    value: {
      type: Number,
      default: 0,
      required: false,
    },
    max: {
      type: Number,
      default: 0,
      required: false,
    },
    message: {
      type: String,
      default: null,
      required: false,
    },
  },
  emits: [],
  created() {},
  methods: {},
  data() {
    return {};
  },
};
</script>
<style scoped>
.progressbar {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-color: #00000065;
  z-index: 11510;
  display: flex;
  align-items: center;
  justify-content: center;
}

.progressbar__content {
  position: relative;
  top: 30px;
  color: #fff;
}

.progressbar__info {
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  white-space: nowrap;
}

progress {
  height: 30px;
  width: 200px;
  border-radius: 0;
}
</style>
