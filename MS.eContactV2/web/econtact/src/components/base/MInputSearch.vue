<template>
  <div class="search-block">
    <span v-if="isShowClear" class="clear-button" @click="clearButtonOnClick"><i class="icofont-close"></i></span>
    <input
      class="input input-search icon icon-16 icon-search"
      ref="minput"
      type="text"
      :placeholder="placeholder"
      :value="modelValue"
      @input="updateValue"
    />
  </div>
</template>

<script>
export default {
  name: "MInputSearch",
  props: ["modelValue", "focus","placeholder"],
  emits: ["update:modelValue","onInput"],
  mounted() {
    if (this.focus == true) this.$refs["minput"].focus();
  },
  watch: {
    modelValue: function (newValue) {
      if(newValue){
        this.isShowClear = true;
      }else{
        this.isShowClear = false;
      }
    },
  },
  methods: {
    updateValue() {
      var newValue = this.$refs.minput.value;
      this.$emit("update:modelValue", newValue);
      this.$emit("onInput", newValue);
    },
    clearButtonOnClick(){
      this.$emit("update:modelValue", null);
      this.isShowClear = false;
    }
  },
  data() {
    return {
      isShowClear: false
    }
  },
};
</script>
<style scoped>
@import url(../../styles/base/input.css);

.search-block{
  position: relative;
}

.search-block input{
  padding-right: 36px;
}

.clear-button{
  display: block;
  position: absolute;
  right: 8px;
  top: 50%;
  transform: translate(-50%,-50%);
  padding: 2px;
  line-height: 12px;
  font-size: 12px;
  text-align: center;
  color: #fff;
  border-radius: 50%;
  background-color: #7e7e7e;
  cursor: pointer;
}
</style>