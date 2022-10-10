<template>
  <m-dialog title="Thêm mới Album">
    <template v-slot:content>
      <div>
        <input type="file" ref="filePictures" multiple="multiple" @change="onSelectPictures" />
      </div>
      <div class="picture__list">
        <div v-for="(pic, index) in pictures" :key="index" class="picture">
          <div class="picture__thumb"><img :src="getFile(pic)" alt="" /></div>
          <div class="picture__name">{{ pic.name }}</div>
          <div class="picture__length">{{ pic.size }}</div>
        </div>
      </div>
    </template>
    <template v-slot:footer>
      <button class="btn btn--default" @click="onRegister">
        <i class="icofont-ui-add"></i> Thêm Album
      </button>
    </template>
  </m-dialog>
</template>
<script>
export default {
  name: "PictureDetail",
  emits: [],
  props: [],
  computed: {
   
  },
  methods: {
    onSelectPictures() {
    //   var formData = new FormData();

      var file = this.$refs.filePictures;
      console.log(file);
      this.pictures = file.files;
    },
    getFile(file){
        return URL.createObjectURL(file);
    }
  },
  data() {
    return {
      pictures: [],
    };
  },
};
</script>
<style scoped>
.picture{
    display: flex;
    align-items: center;
    justify-content: center;
}

.picture >*{
    margin-left: 10px;
}
.picture__thumb{
    width: 50px;
}

.picture__thumb img{
    width: 100%;
}
</style>
