<template>
  <div class="input-wrapper">
    <label v-if="label"
      >{{ label }} <span v-if="required">(<span class="required">*</span>)</span>:</label
    >
    <textarea
      cols="15"
      rows="3"
      ref="minput"
      :placeholder="textPlaceholder"
      :focus="focus"
      :required="required"
      v-model="value"
      @input="onInput"
      :disabled="disabled"
      class="text-area"
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
    textPlaceholder: { type: String, default: "", required: false },
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
textarea {
  border: 1px solid #ccc;
  border-radius: 4px;
  outline: none;
  padding: 8px 16px;
}
.required {
  color: #ff0000;
}

.input-wrapper {
  display: flex;
  flex-direction: column;
}

.input-wrapper textarea {
  width: 100%;
  box-sizing: border-box;
}

.input-wrapper textarea:disabled {
}

.input-wrapper label + textarea {
  margin-top: 8px;
}

.input-wrapper + .input-wrapper {
  margin-top: 16px;
}
</style>
