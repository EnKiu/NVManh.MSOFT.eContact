<template>
  <m-dialog title="Thêm mới Album" @onClose="onClose">
    <template v-slot:content>
      <div class="album-info">
        <m-input
          label="Tên album"
          :validated="validated"
          v-model="album.AlbumName"
          :required="true"
        ></m-input>
        <m-text-area label="Mô tả" v-model="album.Description"></m-text-area>
      </div>
      <div class="toolbar">
        <button id="ADD-PICTURE" class="btn btn--default" @click="onAddPicture">
          <i class="icofont-image"></i> Thêm ảnh
        </button>
        <input
          hidden
          type="file"
          ref="filePictures"
          multiple="multiple"
          @change="onSelectPictures"
        />
      </div>
      <div class="picture__list">
        <div v-for="(pic, index) in pictures" :key="index" class="picture">
          <div class="picture__thumb"><img :src="getFile(pic)" alt="" /></div>
          <div class="picture__name">{{ pic.name }}</div>
          <div class="picture__length">{{ pic.size }} B</div>
          <div class="picture__action">
            <button class="btn btn--table" @click="onRemovePicture(index)">
              Xóa
            </button>
          </div>
        </div>
      </div>
    </template>
    <template v-slot:footer>
      <button class="btn btn--default" @click="onAddAlbum">
        <i class="icofont-verification-check"></i> Lưu Album
      </button>
    </template>
  </m-dialog>
</template>
<script>
export default {
  name: "PictureDetail",
  emits: ["onCloseAddNewDialog","afterAddAlbum"],
  props: [],
  computed: {},
  methods: {
    onAddAlbum() {
      this.validated = true;
      // Thực hiện validate:
      var isValid = this.validateAlbum();
      if (isValid) {
        var formData = new FormData();
        formData.append("album", JSON.stringify(this.album));
        for (let i = 0; i < this.pictures.length; i++) {
          formData.append(`file_${i}`, this.pictures[i]);
        }

        this.api({
            url: "/api/v1/Albums",
            data: formData,
            method: "POST",
          })
          .then((res) => {
            console.log(res);
            this.$emit("afterAddAlbum");
          });
      }
    },
    onAddPicture() {
      this.$refs.filePictures.click();
    },
    onRemovePicture(index) {
      this.pictures.splice(index, 1);
    },
    onSelectPictures() {
      //   var formData = new FormData();
      var file = this.$refs.filePictures;
      console.log(file);
      for (let fileItem of file.files) {
        this.pictures.push(fileItem);
      }
    },
    getFile(file) {
      return URL.createObjectURL(file);
    },
    onClose() {
      this.$emit("onCloseAddNewDialog");
    },
    validateAlbum() {
      if (!this.album.AlbumName) {
        return false;
      }
      return true;
    },
  },
  data() {
    return {
      pictures: [],
      album: {
        AlbumName: "Tên của Album",
        Description: "Đây là mô tả của Album",
      },
      validated: false,
    };
  },
};
</script>
<style scoped>
.picture__list {
  max-height: calc(100vh - 250px);
  overflow-y: auto;
  box-sizing: border-box;
  margin-top: 10px;
}
.picture {
  display: flex;
  align-items: center;
  justify-content: space-between;
  box-sizing: border-box;
}

.picture > * + * {
  margin-left: 10px;
}
.picture__thumb {
  width: 50px;
  flex-shrink: 0;
  flex-grow: 0;
  flex-basis: 50px;
}

.picture__thumb img {
  width: 100%;
}

.picture__name {
  flex: 1;
}

.picture__action button {
  height: 30px;
}
.toolbar {
  margin-top: 10px;
  width: 100%;
  display: flex;
  align-items: center;
  justify-content: flex-start;
}

#ADD-PICTURE {
  height: 30px;
  background-color: rgb(0, 193, 64);
}
</style>
