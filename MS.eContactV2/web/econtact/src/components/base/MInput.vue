<template>
  <div class="input-wrapper">
    <label v-if="label"
      >{{ label }} <span v-if="required">(<span class="required">*</span>)</span>:</label
    >
    <input
      :type="type"
      ref="minput"
      :required="required"
      :placeholder="placeholder"
      v-model="value"
      @input="onInput"
      :maxlength="maxLength"
      :autocomplete="autocomplete"
      :disabled="disabled"
      class="input"
      @keydown="onKeyDown"
      @blur="onBlur"
      :class="{ 'input--invalid': inValid }"
    />
    <div v-if="inValid" class="validate-error">
      <div class="error__content">
        Thông tin
        <span v-if="label"
          ><b>{{ label.toLowerCase() }}</b></span
        ><span v-else>này</span> không được phép để trống.
      </div>
      <!-- <div class="error__arrow">
        <div class="arrow"></div>
      </div> -->
    </div>
  </div>
</template>
<script>
// const focus = {
//   mounted: (el) => el.focus(),
// };
export default {
  name: "MInput",
  directives: {
    // enables v-focus in template
    // focus,
  },
  props: {
    modelValue: { type: String, default: "", required: true },
    placeholder: { type: String, default: "", required: false },
    autocomplete: { type: String, default: "", required: false },
    label: { type: String, default: null, required: false },
    type: { type: String, default: "text", required: false },
    isFocus: { type: Boolean, default: false, required: false },
    required: { type: Boolean, default: false, required: false },
    disabled: { type: Boolean, default: false, required: false },
    validated: { type: Boolean, default: false, required: false },
    onlyNumberChar: { type: Boolean, default: false, required: false },
    maxLength: { type: Number, required: false },
  },
  emits: ["update:modelValue", "onValidate", "update:validated", "onBlur"],
  created() {
    this.value = this.modelValue;
    this.selfValidated = this.validated;
  },
  mounted() {
    if (this.isFocus == true) {
      this.$nextTick(function () {
        this.$refs.minput.focus();
      });
    }
  },
  computed: {
    inValid: function () {
      if (
        (this.validated || this.selfValidated) &&
        this.required &&
        (this.modelValue == "" || this.modelValue == undefined || this.modelValue == null)
      ) {
        return true;
      } else {
        return false;
      }
    },
  },
  methods: {
    onInput() {
      this.$emit("update:modelValue", this.value);
    },
    onKeyDown(evt) {
      evt = evt ? evt : window.event;
      if (this.onlyNumberChar == true) {
        var charCode = evt.which ? evt.which : evt.keyCode;
        if (
          charCode > 31 &&
          (charCode < 48 || charCode > 57) &&
          (charCode < 96 || charCode > 105)
        ) {
          evt.preventDefault();
        }
      }
    },
    onBlur() {
      this.selfValidated = true;
      this.$emit("onBlur");
    },
  },
  watch: {
    validated: function (newValue) {
      if (newValue == true) {
        this.selfValidated = true;
        if (
          this.modelValue == "" ||
          this.modelValue == undefined ||
          this.modelValue == null
        ) {
          this.$emit("onValidate", false);
        } else {
          this.$emit("onValidate", true);
        }
      }
    },
    modelValue: function (newValue) {
      this.value = newValue;
      this.selfValidated = true;
      if (this.value == "" || this.value == undefined || this.value == null) {
        this.$emit("onValidate", false);
      } else {
        this.$emit("onValidate", true);
      }
    },
  },
  data() {
    return {
      value: null,
      selfValidated: false,
    };
  },
};
</script>
<style scoped>
input[type="number"] {
  text-align: right;
  padding-right: 16px;
}
.required {
  color: #ff0000;
}

.input-wrapper {
  position: relative;
  margin: 20px 0;
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
}

/* .validate-error {
  position: absolute;
  bottom: calc(100% - 22px);
  right: 0px;
  background-color: #ff0000;
  padding: 5px 10px;
  border-radius: 4px;
  color: #fff;
  display: flex;
  flex-direction: column;
  align-items: flex-end;
  justify-content: center;
} */
.validate-error {
  position: absolute;
  top: calc(100% + 2px);
  right: 0px;
  display: flex;
  font-size: 11px;
  flex-direction: column;
  align-items: flex-end;
  justify-content: center;
  color: #ff0000;
}
.error__arrow {
  position: absolute;
  top: calc(100% + 4px);
  width: 100%;
  height: 0px;
  display: flex;
  align-items: flex-end;
  justify-content: center;
}

.arrow {
  width: 10px;
  height: 10px;
  background-color: #ff0000;
  transform: rotate(45deg);
}
</style>
