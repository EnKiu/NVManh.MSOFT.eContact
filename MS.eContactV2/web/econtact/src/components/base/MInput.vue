<template>
  <div class="input-wrapper">
    <label v-if="label"
      >{{ label }} (<span v-if="required" class="required">*</span>)</label
    >
    <input
      type="text"
      ref="minput"
      :focus="focus"
      :required="required"
      v-model="value"
      @input="onInput"
      :disabled="disabled"
      class="input"
    />
  </div>
</template>
<script>
export default {
  name: "MInput",
  props: {
    modelValue: { type: String, default: "", required: true },
    label: { type: String, default: null, required: false },
    focus: { type: Boolean, default: false, required: false },
    required: { type: Boolean, default: false, required: false },
    disabled: { type: Boolean, default: false, required: false },
  },
  emits: ["update:modelValue"],
  created() {
    this.value = this.modelValue;
  },
  methods: {
    onInput() {
      this.$emit("update:modelValue", this.value);
    },
  },
  watch: {
    modelValue: function (newValue) {
      this.value = newValue;
    },
  },
  data() {
    return {
      value: null,
    };
  },
};
</script>
<style scoped>
.required {
  color: #ff0000;
}

.input-wrapper {
}

.input-wrapper input {
  width: 100%;
  box-sizing: border-box;
}

.input-wrapper input:disabled {
}

.input-wrapper label + input {
  margin-top: 8px;
}

.input-wrapper + .input-wrapper {
  margin-top: 16px;
}
</style>
